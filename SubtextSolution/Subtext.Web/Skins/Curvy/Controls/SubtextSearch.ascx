<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchbox">
<asp:TextBox id="txtSearch" text="search..." onfocus="if(this.value=='search...') this.value='';" runat="server" onblur="if(this.value=='') this.value='search...';" class="searchinput"></asp:TextBox>
<asp:Button height="23" id="btnSearch" runat="server" text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" class="searchbutton" CausesValidation="False"></asp:Button>
	<div class="results">
		<asp:Repeater id="SearchResults" runat="server">
			<HeaderTemplate>
				<div class="title">Results</div>
			</HeaderTemplate>
			<ItemTemplate>
				<div id="item">
					<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"> 
					<%# DataBinder.Eval(Container.DataItem, "Title") %> 
					</a>
				</div>
			</ItemTemplate>
		</asp:Repeater>
	</div>
</div>

<script>
function clickButton(e, buttonid){ 
      var bt = document.getElementById(buttonid); 
      if (typeof bt == 'object'){ 
            if(navigator.appName.indexOf("Netscape")>(-1)){ 
                  if (e.keyCode == 13){ 
                        bt.click(); 
                        return false; 
                  } 
            } 
            if (navigator.appName.indexOf("Microsoft Internet Explorer")>(-1)){ 
                  if (event.keyCode == 13){ 
                        bt.click(); 
                        return false; 
                  } 
            } 
      }
} 
</script>