using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// The standard button control emits the language attribute which is not 
	/// XHTML compliant.  This is a button to use if you wish to remain XHTML compliant. 
	/// </summary>
	public class CompliantButton : Button
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompliantButton"/> class.
		/// </summary>
		public CompliantButton() : base()
		{
		}

		/// <summary>
		/// Basically a reimplementation of the base 
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
			if (((this.Page != null) && this.CausesValidation) && (this.Page.Validators.Count > 0))
			{
				string text1 = GetClientValidateEvent();
				if(base.Attributes.Count > 0)
				{
					string text2 = base.Attributes["onclick"];
					if (text2 != null)
					{
						text1 = text2 + text1;
						base.Attributes.Remove("onclick");
					}
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, text1);
			}
			AddAttributesToRenderBase(writer);
		}

		private static string GetClientValidateEvent()
		{
			return "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate(); ";
		}
 
		private void AddAttributesToRenderBase(HtmlTextWriter writer)
		{
			string text1;
			if (this.ID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			}
			if (this.AccessKey != null && this.AccessKey.Length > 0)
			{
				text1 = this.AccessKey;
				if (text1.Length > 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, text1);
				}
			}
			if (!this.Enabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (this.TabIndex > 0)
			{
				int num1 = this.TabIndex;
				if (num1 != 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, num1.ToString(NumberFormatInfo.InvariantInfo));
				}
			}
			if (this.ToolTip != null && this.ToolTip.Length > 0)
			{
				text1 = this.ToolTip;
				if (text1.Length > 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Title, text1);
				}
			}
			if (this.ControlStyleCreated)
			{
				this.ControlStyle.AddAttributesToRender(writer, this);
			}
			if (this.Attributes != null)
			{
				AttributeCollection collection1 = this.Attributes;
				IEnumerator enumerator1 = collection1.Keys.GetEnumerator();
				while (enumerator1.MoveNext())
				{
					string text2 = (string) enumerator1.Current;
					writer.AddAttribute(text2, collection1[text2]);
				}
			}
		}


	}
}
