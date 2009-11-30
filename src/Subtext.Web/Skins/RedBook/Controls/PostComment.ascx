<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>

    <div id="commentform">
    <fieldset>
	    <legend>Post a comment</legend>
	    <p>
			<label for="PostComment_ascx_tbTitle" AccessKey="T"><u>T</u>itle:</label> <asp:TextBox id="tbTitle" runat="server" Size="40" TabIndex="1" CssClass="textbox" />
			<br/>
			<asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
		</p>
	    <p>
			<label for="PostComment_ascx_tbName" AccessKey="N"><u>N</u>ame:</label> <asp:TextBox id="tbName" runat="server" Size="40" TabIndex="2" CssClass="textbox" />
			<br/>
			<asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
		</p>
	    <p>
			<label for="PostComment_ascx_tbEmail" AccessKey="E"><u>E</u>mail: <em>Not Displayed</em></label> <asp:TextBox id="tbEmail" runat="server" Size="40" TabIndex="2" CssClass="textbox" />
			<br/>
			<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
	    </p>
	    <p>
			<label for="PostComment_ascx_tbUrl" AccessKey="W"><u>W</u>ebsite:</label> <asp:TextBox id="tbUrl" runat="server" Size="40" TabIndex="3" CssClass="textbox" />
			<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
	    </p>
	    <p>
		    <label for="PostComment_ascx_tbComment" AccessKey="C"><u>C</u>omment:</label> 
		    <asp:TextBox id="tbComment" runat="server" Rows="10" Columns="50"
				    TabIndex="4"
				    CssClass="textarea"
				    TextMode="MultiLine" />
				    <br/>
		    <asp:RequiredFieldValidator id="vldCommentBody" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" EnableClientScript="true" />
	    </p>
	    <asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="False" TabIndex="5" />
	    <p>
			<st:SubtextCaptchaControl id="captcha" runat="server" ErrorMessage="Please enter the correct word" />
			<st:CompliantButton id="btnCompliantSubmit" CssClass="buttonSubmit" runat="server" Text="Post" TabIndex="6" CausesValidation="true" />
			<asp:Label id="Message" runat="server" ForeColor="Red" />
		</p>
	    <div id="stylesheetTest"></div>
    </fieldset>
    </div>

