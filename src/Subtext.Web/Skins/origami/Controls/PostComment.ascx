<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div class="section" id="comment-reply-form">
    <h2 class="section-title">Your Reply.</h2>
    <div id="preview" style="display: none" class="normal-comment">
    </div>
    <div class="comment-box">
        <fieldset id="respond">
            <legend>Comment Form.</legend>
            <div id="errors"> </div>
            <div class="notes">
                <p>Fields denoted with a "<em>*</em>" are required.</p>
            </div>
            <div class="required clearfix">
                <label for="PostComment_ascx_tbName"><em>*</em>Your name:</label>
                <asp:TextBox ID="tbName" runat="server" CssClass="fixed" />
                <asp:RequiredFieldValidator ID="vldNameRequired" runat="server" ErrorMessage="Please enter your name"
                    ControlToValidate="tbName" Display="Dynamic" />
            </div>
            <div class="required clearfix">
                <label for="PostComment_ascx_tbTitle"><em>*</em>Subject:</label>
                <asp:TextBox ID="tbTitle" runat="server" CssClass="fixed"></asp:TextBox>
                <asp:RequiredFieldValidator ID="vldTitleRequired" runat="server" ErrorMessage="Please enter a title"
                    ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
            </div>
            <div>
                <small>You may also like to <a href="#" id="comment-form-toggle">
                    leave your email or website.</a></small>
                <div id="optional-fields" style="display: none;">
                    <div id="guest_url" class="clearfix">
                        <label for="PostComment_ascx_tbUrl">Your blog:</label>
                        <asp:TextBox ID="tbUrl" runat="server" CssClass="fixed" />
                        <asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl"
                            ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic"
                            ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
                    </div>
                    <div id="guest_email" class="clearfix">
                        <label for="PostComment_ascx_tbEmail">Your email:</label>
                        <asp:TextBox ID="tbEmail" runat="server" CssClass="fixed" />&nbsp;(will not be displayed)
                        <asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail"
                            ValidationExpression="^.*?@.+\..+$"
                            Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified."
                            EnableClientScript="true" />
                    </div>
                </div>
            </div>
            <div class="required clearfix">
                <label for="PostComment_ascx_tbComment"><em>*</em>Your message:</label>
                <asp:TextBox ID="tbComment" runat="server" Rows="7" Columns="55" CssClass="fixed livepreview" TextMode="MultiLine" />
                <br />
                <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment"
                    ControlToValidate="tbComment" />
            </div>
            <asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me?" Visible="false"
                Checked="true" />
            <div id="frm-btns" class="buttons">
                <asp:Button ID="btnSubmit" runat="server" Text="Publish Comment" CssClass="button" />
            </div>
            <h2 class="section-title">Preview Your Comment.</h2>
		    <div id="commentPreview" class="preview-comment"><div class="livepreview comment-body" >&nbsp;</div></div>
        </fieldset>
    </div>
</div> 