using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design;

namespace Subtext.Web.Controls.Designers
{
	/// <summary>
	/// Designer for the HelpToolTip class.
	/// </summary>
	public class HelpToolTipDesigner : ContainerControlDesigner
	{
		public HelpToolTipDesigner() : base()
		{
		}
		
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection helpTooltipActions = new DesignerActionListCollection();
				helpTooltipActions.Add(new HelpToolTipActionList(this.Component));
				return helpTooltipActions;
			}
		}
	}
	
	class HelpToolTipActionList : DesignerActionList
	{
		private HelpToolTip helpToolTip;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="HelpToolTipActionList"/> class.
		/// </summary>
		/// <param name="helpToolTipControl">The help tool tip control.</param>
		public HelpToolTipActionList(IComponent helpToolTipControl) : base(helpToolTipControl)
		{
			this.helpToolTip = helpToolTipControl as HelpToolTip;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the smart tag panel should automatically be displayed when it is created.
		/// </summary>
		/// <value></value>
		/// <returns>true if the panel should be shown when the owning component is created; otherwise, false. The default is false.</returns>
		public override bool AutoShow
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets or sets the help text of the HelpToolTip control.
		/// </summary>
		/// <value>The help text.</value>
		public string HelpText
		{
			get { return this.helpToolTip.HelpText; }
			set
			{
				PropertyDescriptor propDesc = TypeDescriptor.GetProperties(this.helpToolTip)["HelpText"];
				propDesc.SetValue(this.helpToolTip, value);
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection helpTooltipItems = new DesignerActionItemCollection();

			DesignerActionHeaderItem header = new DesignerActionHeaderItem("Help Tool Tip");
			helpTooltipItems.Add(header);
			
			// Key Help tooltip properties.
			DesignerActionPropertyItem tip = new DesignerActionPropertyItem("HelpText", "Help Text", "Display", "The help message to display in the tooltip when user clicks on the control.");
			helpTooltipItems.Add(tip);
			
			return helpTooltipItems;
		}
	}
}
