<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Error.ascx.cs" Inherits="Subtext.Web.Skins._System.Controls.Error" %>
<div class="skin-error" style="clear: both; border: solid 2px #c00; color: #c00; padding: 10px;">
    <h2 style="margin-bottom: 10px;">
        <asp:Image ID="Image1" ImageUrl="~/images/icons/ico_critical.gif" runat="server" ImageAlign="Left" />
        <span style="margin-left: 5px; font-weight: bold;"><%= Exception.Message %></span>
    </h2>
    
    <p><label>Control Virtual Path: </label> <strong><%= Exception.ControlPath %></strong></p>
    
    <% if(ShowErrorDetails) { %>
        
        <div class="stack-trace" style="margin-top: 8px;">
            <label style="display: block; margin: 0; padding: 0; font-weight: bold;">Message:</label>
            <div style="margin:0; padding: 0;">
                <%= Exception.InnerException.Message %>
            </div>
            <label style="display: block; margin: 0; margin-top: 4px; padding: 0; font-weight: bold;">Stack Trace:</label>
            <div style="margin:0; padding: 0;">
                <%= Exception.InnerException.ToString() %>
            </div>
        </div>
        
    <% } else {%>
        <p>To view more details about the error, login as an Admin or run the site on localhost.</p>
    <% }%>
    
</div>