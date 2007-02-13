<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div class="section" id="comment-reply-form">
    <h2 class="section-title">
        Your Reply.</h2>
    <div id="preview" style="display: none" class="normal-comment">
    </div>
    <div class="comment-box">
        <fieldset id="respond">
            <legend>Comment Form.</legend>
            <div id="errors"></div>
            <div class="notes"><p>Fields denoted with a "<em>*</em>" are required.</p></div>
            <div class="required clearfix">
                <label for="PostComment.ascx_tbName"><em>*</em>Your name:</label>
                <asp:TextBox ID="tbName" runat="server" CssClass="fixed" />
                <asp:RequiredFieldValidator ID="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
            </div>
            <div class="required clearfix">
                <label for="PostComment.ascx_tbTitle"><em>*</em>Subject:</label>
                <asp:TextBox ID="tbTitle" runat="server" CssClass="fixed"></asp:TextBox>
                <asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
            </div>
            <div>
				<small>You may also like to <a href="/pages/javascript-error" onclick="toggleCommentForm(); return false;"> leave your email or website.</a></small>
				<div id="guest_url" class="clearfix" style="display: none;">
					<label for="PostComment.ascx_tbUrl">
						Your blog:</label>
					<asp:TextBox ID="tbUrl" runat="server" CssClass="fixed" />
					<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
				</div>
				<div id="guest_email" class="clearfix" style="display: none;">
					<label for="PostComment.ascx_tbEmail">
						Your email:</label>
					<asp:TextBox ID="tbEmail" runat="server" CssClass="fixed" />&nbsp;(will not be displayed)
					<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
				</div>
				</div>
				<div class="required clearfix">
					<label for="PostComment.ascx_tbComment">
						<em>*</em>Your message:</label>
					<asp:TextBox ID="tbComment" runat="server" Rows="7" cols="55" CssClass="fixed" TextMode="MultiLine" onkeyup="reloadPreviewDiv();" />
					<br />
					<asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" />
				</div>
				<asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me?" Visible="false" Checked="false" />
				<div id="frm-btns" class="buttons">
					<asp:Button ID="btnSubmit" runat="server" Text="Publish Comment" CssClass="button" />
				</div>
				</fieldset>
			</div>
</div> 