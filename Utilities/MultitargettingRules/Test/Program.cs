using System;

using System.Runtime;
using System.Security;
using System.Runtime.ConstrainedExecution;
using System.Reflection.Emit;
using System.Reflection;
using System.Collections.Generic;

namespace Test {
    class Program {
        static void Main() {
            Console.WriteLine("LatencyMode = {0}", GCSettings.LatencyMode);

            DateTimeOffset dto = DateTimeOffset.Now;
            Console.WriteLine("It's {0} now", dto);

            AppDomainSetup a = new AppDomainSetup();
            a.SandboxInterop = true;

            Console.WriteLine(GCCollectionMode.Default);

            Console.WriteLine(System.Deployment.Internal.InternalActivationContextHelper.GetApplicationManifestBytes(null));

            SecureString str = new SecureString();
            object obj = str;
            CriticalFinalizerObject cfo4 = obj as CriticalFinalizerObject;

            DynamicMethod dm = new DynamicMethod("", null, null);

            SignatureHelper.GetPropertySigHelper(null, CallingConventions.Any, typeof(object), null, null, null, null, null);
            TypeBuilder tb = null;
            tb.DefineProperty("", PropertyAttributes.None, CallingConventions.Any, typeof(object), null, null, null, null, null);

            AppDomain ad = null;
            ad.DefineDynamicAssembly(new AssemblyName(), AssemblyBuilderAccess.Run, new List<CustomAttributeBuilder>());
            ad.DefineDynamicAssembly(new AssemblyName(), AssemblyBuilderAccess.Run, "", null, null, null, null, true, null);
        }
    }
}