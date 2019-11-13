<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs"
    Inherits="TechnologyToolbox.Caelum.Website.Controls.Captcha.CaptchaControl" %>
<script language="javascript" type="text/javascript" src="<%=ResolveClientUrl("~/Controls/Captcha/Scripts/s3Capcha.js")%>"></script>
<script language="javascript" type="text/javascript">
    $(document).ready(function () { $('#captcha').s3Capcha(); });

    <%--
        If the CAPTCHA is being used on a page where viewstate is disabled and
        an UpdatePanel is used to perform a partial page update, then we need to
        configure the CAPTCHA images again, using the technique described for
        jQuery and ASP.NET UpdatePanels:

        http://stackoverflow.com/questions/256195/jquery-document-ready-and-updatepanels

        Also note that if no UpdatePanel is being used (e.g. on
        /Contact/Default.aspx) then the Sys "namespace" will be undefined.
    --%>
    if (typeof Sys != 'undefined') {
        <%--
            After a comment is submitted for a blog post (using an
            UpdatePanel to perform a partial page update), the "captcha"
            element will no longer be available, so this time use a function
            that checks if the element exists before attempting to configure it
        --%>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(configureCaptcha);
    }

    function configureCaptcha() {
        if ($('#captcha') != null) {
            $('#captcha').s3Capcha();
        }
    }
</script>
<div id="captcha">
    <asp:Literal ID="CaptchaPlaceholder" runat="server"></asp:Literal>
    <asp:CustomValidator runat="server" ID="CaptchaValidator"
        CssClass="validator" ErrorMessage="The correct image is not selected."
        Text="(invalid)" OnServerValidate="ValidateCaptchaControl" />
</div>
