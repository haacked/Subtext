<%@ Page Language="C#" MasterPageFile="~/aspx/Admin/Feedback/Feedback.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Subtext.Web.Admin.Feedback.EditPage" Title="Subtext Admin - Edit Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="feedbackContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <asp:PlaceHolder ID="Edit" runat="server">
        <fieldset id="editPost">
            <legend>Edit</legend>
            <label>Posted By: <asp:HyperLink ID="hlAuthorEmail" runat="server"><asp:Label runat="server" ID="lblName"><asp:Label runat="server" ID="lblEmail"></asp:Label></asp:Label></asp:HyperLink></label>
            <label>Comment Url: <asp:HyperLink id="hlEntryLink" Runat="server" /></label>
            
            <p class="Label">
                <label for="Editor_Edit_txbTitle" accesskey="t">Comment <u>T</u>itle		
            &nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" CssClass="error" ErrorMessage="* Your post must have a title" />
            </label>
            </p>
            <p>
                <asp:TextBox id="txbTitle" runat="server" CssClass="textbox" />
            </p>
            <p class="Label">
                <label for="Editor_Edit_txbWebsite" accesskey="w">Comment <u>W</u>ebsite
                <asp:CustomValidator ID="valtxbWebsite" runat="server" Text="Invalid website format. Must be a URI" ControlToValidate="txbWebsite"></asp:CustomValidator>
                </label>
            </p>
            <p>
            <asp:TextBox id="txbWebsite" runat="server" CssClass="textbox" />
            </p>
                
            <label for="Editor_Edit_richTextEditor" accesskey="b">Comment <u>B</u>ody
            &nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" CssClass="error" ErrorMessage="Your post must have a body" />
            </label>
            <st:RichTextEditor id="richTextEditor" runat="server" />

            <div>
                <asp:Button id="lkbPost" runat="server" Text="Post" CssClass="button" OnClick="lkbPost_Click" />&nbsp;&nbsp;
                <a href="<%= CancelUrl %>">Cancel</a>
            </div>
        </fieldset>
    </asp:PlaceHolder>
</asp:Content>
