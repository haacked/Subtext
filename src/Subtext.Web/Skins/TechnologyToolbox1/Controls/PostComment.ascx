<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<%@ Register src="~/Controls/Captcha/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>
<!-- Begin: PostComment.ascx -->
<div id="commentForm">
    <h3>Add Comment</h3>
    <fieldset>
        <div>
            <asp:Label runat="server" AssociatedControlID="tbName" Text="Name" />
            <asp:TextBox runat="server" ID="tbName" CssClass="required" MaxLength="32" size="20" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbName" Display="Dynamic"
                ErrorMessage="Name must be specified." Text="(required)"
                CssClass="validator required" ForeColor="" />
        </div>
        <div>
            <asp:Label runat="server" AssociatedControlID="tbTitle" Text="Title" />
            <asp:TextBox runat="server" ID="tbTitle" CssClass="required" MaxLength="64" size="20" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbTitle"
                Display="Dynamic" ErrorMessage="Title must be specified." Text="(required)"
                CssClass="validator required" ForeColor="" />
        </div>
        <div>
            <asp:Label runat="server" AssociatedControlID="tbEmail" Text="Email Address" />
            <asp:TextBox runat="server" ID="tbEmail" MaxLength="254" size="20" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbEmail"
                ValidationExpression="^.*?@.+\..+$" Display="dynamic"
                ErrorMessage="Email Address is not required, but if specified it must be valid."
                Text="(invalid)" CssClass="validator" ForeColor="" />
            <div class="field-info">
                Optional, but recommended (especially if you have a <a href="http://www.gravatar.com"
                    target="_blank">Gravatar</a>). Note that your email address will not appear
                with your comment.</div>
        </div>
        <div>
            <asp:Label runat="server" AssociatedControlID="tbUrl" Text="URL" />
            <asp:TextBox runat="server" ID="tbUrl" MaxLength="254" size="20" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUrl"
                ValidationExpression="\b(https?)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]*[-A-Za-z0-9+&@#/%=~_|]"
                Display="Dynamic" ErrorMessage="URL is not required, but if specified it must be valid."
                Text="(invalid)" CssClass="validator" ForeColor="" />
            <div class="field-info">
                If URL is specified, it will be included as a link with your name.</div>
        </div>
        <div>
            <asp:CheckBox runat="server"  ID="chkRemember" Checked="True" Text="Remember me"
                CssClass="checkbox" />
        </div>
        <div>
            <asp:Label runat="server" AssociatedControlID="tbComment" Text="Comment" />
            <asp:TextBox runat="server" ID="tbComment" CssClass="required" TextMode="MultiLine"
                Rows="10" Columns="60" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbComment"
                Display="Dynamic" ErrorMessage="Comment must be specified." Text="(required)"
                CssClass="validator required" ForeColor="" /></div>
    </fieldset>
    <uc1:Captcha runat="server" ValidationGroup="SubtextComment" />
    <div class="button-panel">
        <asp:Button runat="server" ID="btnSubmit" Text="Submit" />
    </div>
    <asp:ValidationSummary runat="server" ValidationGroup="SubtextComment"
        CssClass="validation-summary" ForeColor=""
        HeaderText="There is a problem with your request. Please correct and try again."  />
    <%--
        The "Message" error label is required (if it is not provided, a
        NullReferenceException may occur in the OnSubmitButtonClick event handler
        in PostComment.cs -- for example, when a duplicate comment is entered)
    --%>
    <asp:Label runat="server" id="Message" ForeColor="#bd1c1c" />
</div>
<%--
    HACK: Override scrollTo function to avoid issue where ValidationSummary
    causes ASP.NET to scroll to the top
    (http://stackoverflow.com/questions/699171/asp-net-validation-summary-causes-page-to-jump-to-top)
--%>
<script type="text/javascript">
    window.scrollTo = function () { } 
</script> 
<!-- End: PostComment.ascx -->
