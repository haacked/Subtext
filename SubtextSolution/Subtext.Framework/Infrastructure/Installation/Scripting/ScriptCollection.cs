#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Subtext.Scripting
{
    /// <summary>
    /// A collection of <see cref="Script"/>s.
    /// </summary>
    public class ScriptCollection : Collection<Script>, ITemplateScript
    {
        readonly string _fullScriptText; //Original unexpanded script.
        TemplateParameterCollection _templateParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCollection"/> class.
        /// </summary>
        /// <param name="fullScriptText">The full script text.</param>
        internal ScriptCollection(string fullScriptText)
        {
            _fullScriptText = fullScriptText;
        }

        /// <summary>
        /// Gets the original full unexpanded script text.
        /// </summary>
        /// <value>The full script text.</value>
        public string FullScriptText
        {
            get { return _fullScriptText; }
        }

        /// <summary>
        /// Gets the expanded script text.
        /// </summary>
        /// <value>The expanded script text.</value>
        public string ExpandedScriptText
        {
            get
            {
                var builder = new StringBuilder();
                ApplyTemplatesToScripts();
                foreach(Script script in this)
                {
                    builder.Append(script.ScriptText);
                    builder.Append(Environment.NewLine);
                    builder.Append("GO");
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                }
                return builder.ToString();
            }
        }

        #region ITemplateScript Members

        /// <summary>
        /// Gets the template parameters embedded in the script.
        /// </summary>
        /// <returns></returns>
        public TemplateParameterCollection TemplateParameters
        {
            get
            {
                if(_templateParameters == null)
                {
                    _templateParameters = new TemplateParameterCollection();
                    foreach(Script script in this)
                    {
                        _templateParameters.AddRange(script.TemplateParameters);
                    }
                    _templateParameters.ValueChanged += TemplateParametersValueChanged;
                }

                return _templateParameters;
            }
        }

        #endregion

        /// <summary>
        /// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
        /// to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="Script"/>s to add to the collection. </param>
        public void AddRange(IEnumerable<Script> value)
        {
            if(value == null)
            {
                throw new ArgumentNullException("value");
            }

            foreach(Script script in value)
            {
                Add(script);
            }
        }

        internal void ApplyTemplatesToScripts()
        {
            foreach(TemplateParameter parameter in TemplateParameters)
            {
                foreach(Script script in this)
                {
                    if(script.TemplateParameters.Contains(parameter.Name))
                    {
                        script.TemplateParameters[parameter.Name].Value = parameter.Value;
                    }
                }
            }
        }

        private void TemplateParametersValueChanged(object sender, ParameterValueChangedEventArgs args)
        {
            ApplyTemplatesToScripts();
        }
    }
}