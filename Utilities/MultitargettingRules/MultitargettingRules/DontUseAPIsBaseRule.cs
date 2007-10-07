using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System;
using Microsoft.FxCop.Sdk;
using Microsoft.FxCop.Sdk.Introspection;
using Microsoft.Cci;
using System.Reflection;

namespace Krzysztof.MultitargettingRules.Base
{
    public abstract class DontUseAPIsRuleBase : BaseIntrospectionRule
    {
        private HashSet bannedMembers = new HashSet();
        private HashSet bannedTypes = new HashSet();

        static DontUseAPIsRuleBase()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "trace.log.txt")));
        }

        protected DontUseAPIsRuleBase(string ruleName)
            : base(ruleName, "MultitargettingRules.MultitargettingRules", typeof(DontUseAPIsRuleBase).Assembly)
        {
            string ruleFile = ruleName + ".txt";

            Assembly assembly = this.GetType().Assembly;
            string ruleAssemblyDirectory = Path.GetDirectoryName(assembly.Location);
            string ruleAssemblyFile = Path.Combine(ruleAssemblyDirectory, ruleFile);

            if (File.Exists(ruleAssemblyFile))
            {
                using (StreamReader sr = new StreamReader(ruleAssemblyFile))
                {
                    PopulateBannedApis(sr);
                }
            }
            else
            {
                List<string> manifestNames = new List<string>(assembly.GetManifestResourceNames());
                if (manifestNames.Contains("MultitargettingRules." + ruleFile))
                {
                    using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("MultitargettingRules." + ruleFile)))
                    {
                        PopulateBannedApis(sr);
                    }
                }
                else
                {
                    Debug.Print("Could not load configuration.");
                }
            }
        }

        protected void PopulateBannedApis(StreamReader streamContainingBannedApis)
        {
            while (!streamContainingBannedApis.EndOfStream)
            {
                string api = streamContainingBannedApis.ReadLine();
                Debug.WriteLine(api);
                
                string[] tokens = api.Split(':');
                if (tokens.Length != 2)
                {
                    continue;
                }

                if (tokens[0] == "t")
                {
                    bannedTypes.Add(tokens[1]);
                }
                else if (tokens[0] == "m")
                {
                    bannedMembers.Add(tokens[1]);
                }
            }
        }

        public sealed override ProblemCollection Check(Member member)
        {
            Method method = member as Method;
            if (method != null)
            {
                return CheckMethod(method);
            }
            return base.Check(member);
        }

        private ProblemCollection CheckMethod(Method callingMethod)
        {
            foreach (Instruction instruction in callingMethod.Instructions)
            {
                if (instruction.OpCode == OpCode.Callvirt || instruction.OpCode == OpCode.Call)
                {
                    CheckCallInstruction(callingMethod, instruction);
                }

                if (instruction.OpCode == OpCode.Box)
                {
                    TypeNode typeToBox = instruction.Value as TypeNode;
                    if (typeToBox != null)
                    {
                        Debug.WriteLine(typeToBox.FullName);
                        if (bannedTypes.Contains(typeToBox.FullName))
                        {
                            ReportViolation(callingMethod, instruction, typeToBox.FullName);
                        }
                    }
                }
                if (instruction.OpCode == OpCode.Newobj)
                {
                    CheckCallInstruction(callingMethod, instruction);
                }
            }
            return base.Problems;
        }

        private void CheckCallInstruction(Method callingMethod, Instruction instruction)
        {
            Member calledMember = instruction.Value as Member;
            if (calledMember != null)
            {
                string calledType = calledMember.DeclaringType.FullName;

                if (bannedTypes.Contains(calledType))
                {
                    ReportViolation(callingMethod, instruction, calledMember.FullName);
                }

                Method calledMethod = instruction.Value as Method;
                if (calledMethod != null)
                {
                    string methodName = calledType + "." + calledMethod.Name + GetParameterList(calledMethod);
                    Debug.Print("CCI name {0}", methodName);
                    if (bannedMembers.Contains(methodName))
                    {
                        ReportViolation(callingMethod, instruction, methodName);
                    }
                }
            }
        }

        private string GetParameterList(Method calledMethod)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            bool added = false;
            foreach (Parameter p in calledMethod.Parameters)
            {
                if (added)
                {
                    sb.Append(",");
                }
                else
                {
                    added = true;
                }

                string name = p.Type.Namespace + "." + p.Type.Name;
                if (p.Type.IsGeneric)
                {
                    name = GetFullUnmangledNameWithTypeParameters2(p.Type);
                }
                sb.Append(name);
            }
            sb.Append(")");

            return sb.ToString();
        }

        private string GetFullUnmangledNameWithTypeParameters2(TypeNode typeNode)
        {
            StringBuilder sb = new StringBuilder();
            string name = typeNode.Name.Name;
            if (name.Contains("`"))
            {
                name = name.Substring(0, name.LastIndexOf('`'));
            }

            sb.Append(typeNode.Namespace);
            sb.Append(".");
            sb.Append(name);
            
            if (typeNode.IsGeneric)
            {
                sb.Append('<');

                bool added = false;
                foreach (TypeNode ta in typeNode.TemplateArguments)
                {
                    if (added)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        added = true;
                    }
            
                    string taName = ta.Name.Name;
                    if (ta.IsGeneric)
                    {
                        taName = GetFullUnmangledNameWithTypeParameters2(ta);
                    }
                    sb.Append(taName);
                }
                sb.Append('>');
            }
            return sb.ToString();
        }

        private void ReportViolation(Member member, Instruction instruction, string calledMember)
        {
            Resolution resolution = GetResolution(member.FullName, calledMember);
            Problem problem = new Problem(resolution);
            problem.SourceFile = instruction.SourceContext.SourceText;
            problem.SourceLine = instruction.SourceContext.StartLine;
            base.Problems.Add(problem);
        }
    }
}