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
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var helpTooltipActions = new DesignerActionListCollection();
                helpTooltipActions.Add(new HelpToolTipActionList(Component));
                return helpTooltipActions;
            }
        }
    }

    class HelpToolTipActionList : DesignerActionList
    {
        private readonly HelpToolTip helpToolTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpToolTipActionList"/> class.
        /// </summary>
        /// <param name="helpToolTipControl">The help tool tip control.</param>
        public HelpToolTipActionList(IComponent helpToolTipControl) : base(helpToolTipControl)
        {
            helpToolTip = helpToolTipControl as HelpToolTip;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart tag panel should automatically be displayed when it is created.
        /// </summary>
        /// <value></value>
        /// <returns>true if the panel should be shown when the owning component is created; otherwise, false. The default is false.</returns>
        public override bool AutoShow
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the help text of the HelpToolTip control.
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText
        {
            get { return helpToolTip.HelpText; }
            set
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(helpToolTip)["HelpText"];
                propDesc.SetValue(helpToolTip, value);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var helpTooltipItems = new DesignerActionItemCollection();

            var header = new DesignerActionHeaderItem("Help Tool Tip");
            helpTooltipItems.Add(header);

            // Key Help tooltip properties.
            var tip = new DesignerActionPropertyItem("HelpText", "Help Text", "Display",
                                                     "The help message to display in the tooltip when user clicks on the control.");
            helpTooltipItems.Add(tip);

            return helpTooltipItems;
        }
    }
}