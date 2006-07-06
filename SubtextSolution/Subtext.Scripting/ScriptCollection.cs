#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
		string _fullScriptText; //Original unexpanded script.
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
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="Script"/>s to add to the collection. </param>
		public void AddRange(IEnumerable<Script> value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");
			
			foreach(Script script in value)
			{
				this.Add(script);
			}
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
				StringBuilder builder = new StringBuilder();
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

		internal void ApplyTemplatesToScripts()
		{
			foreach(TemplateParameter parameter in this.TemplateParameters)
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
					_templateParameters.ValueChanged += _templateParameters_ValueChanged;
				}

				return _templateParameters;
			}
		}

		private void _templateParameters_ValueChanged(object sender, ParameterValueChangedEventArgs args)
		{
			ApplyTemplatesToScripts();
		}
	}
}
