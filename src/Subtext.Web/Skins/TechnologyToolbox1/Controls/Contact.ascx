<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
<!-- Begin: Contact.ascx -->
<div class="post type-post hentry">
    <h2>
        Contact Form</h2>
    <div class="info">
        <asp:Label ID="lblMessage" runat="server" /></div>
    <div class="Form">
        <p>
            Please use the form below if you have any comments, questions, or suggestions.
        </p>
        <br />
        <dl class="Inputs">
            <dt>Name<asp:RequiredFieldValidator ID="vldNameRequired" runat="server" ErrorMessage="Please enter your name"
                ControlToValidate="tbName" Display="Dynamic"> *Please enter your name or nickname.</asp:RequiredFieldValidator>
            </dt>
            <dd>
                <asp:TextBox ID="tbName" CssClass="author" size="22" TabIndex="1" runat="server" />
            </dd>
            <dt>Email<asp:RequiredFieldValidator ID="emailRequiredValidator" runat="server" ErrorMessage="Please enter your email address"
                ControlToValidate="tbEmail" Display="Dynamic"> *Please enter your email.</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="vldEmailRegex" runat="server" ErrorMessage="The email address you've entered does not appear to be valid."
                    ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">*</asp:RegularExpressionValidator>
            </dt>
            <dd>
                <asp:TextBox ID="tbEmail" CssClass="author" size="22" TabIndex="2" runat="server" />
            </dd>
            <dt>Subject </dt>
            <dd>
                <asp:TextBox ID="tbSubject" CssClass="author" size="22" TabIndex="3" runat="server" /></td>
            </dd>
            <dt>Message<asp:RequiredFieldValidator ID="vldMessageRequired" runat="server" ErrorMessage="Please enter a message"
                ControlToValidate="tbMessage" Display="Dynamic"> *Please enter a message. Anything.</asp:RequiredFieldValidator>
            </dt>
            <dd>
                <asp:TextBox ID="tbMessage" runat="server" CssClass="author" Width="100%" Rows="10"
                    TabIndex="4" TextMode="MultiLine" />
            </dd>
        </dl>
        <div class="Action">
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="submit" />
        </div>
    </div>
</div>
<!-- End: Contact.ascx -->