<%@ Page Language="C#" %>
<script runat="server">
    public void Page_Load(object sender, EventArgs e )
    {
		foreach(string key in Request.Form.Keys)
			Response.Write(key + "=" + Request.Form[key] + "&");
		Response.Write("Done");
    }
</script>