<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="comments">
	<h3>Your comment:</h3>
	<table id="commentform">
		<tr>
			<td align="right">Title:</td>
			<td>
				<asp:TextBox id="tbTitle" runat="server" CssClass="fixed" Columns="45" />
				<asp:RequiredFieldValidator id="vldTitleRequired" runat="server" Display="Dynamic" ControlToValidate="tbTitle" ErrorMessage="Please enter a title"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td align="right">Name:</td>
			<td><asp:TextBox id="tbName" runat="server" CssClass="fixed" Columns="45" />
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name"
					ControlToValidate="tbName" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td align="right">Email:</td>
			<td>
				<asp:TextBox id="tbEmail" runat="server" CssClass="fixed" Columns="45" />
				<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
			</td>
		</tr>
		<tr>
			<td align="right">Website:</td>
			<td>
				<div class="input"><asp:TextBox id="tbUrl" runat="server" CssClass="fixed" Columns="45"></asp:TextBox>
					<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
				</div>
			</td>
		</tr>
		<tr>
			<td align="right" valign="bottom">
				<div class="label">
					<label for="PostComment_ascx_tbComment">Comment:</label>
					<asp:RequiredFieldValidator id="vldCommentBody" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" EnableClientScript="true" />
				</div>
			</td>
			<td>
				<div class="commentButtons">
					<img id=buttonBold 
						title="Bold"
						onclick="wrapWithBold(document.getElementById('PostComment.ascx_tbComment')); return false;" 
						src="~/skins/KeyWest/images/comment_button_bold.jpg" runat="server">
					<img id=buttonItalic 
						onclick="wrapWithItalic(document.getElementById('PostComment.ascx_tbComment')); return false;" 
						alt="Italic"
						src="~/skins/KeyWest/images/comment_button_italic.jpg" runat="server">
					<img id=buttonUnderline 
						onclick="wrapWithUnderline(document.getElementById('PostComment.ascx_tbComment')); return false;" 
						alt="Underline"
						src="~/skins/KeyWest/images/comment_button_underline.jpg" runat="server">
					<img id=buttonBlockquote 
						onclick="wrapWithBlockquote(document.getElementById('PostComment.ascx_tbComment')); return false;" 
						alt="Blockquote"
						src="~/skins/KeyWest/images/comment_button_blockquote.jpg" runat="server">
					<img id=buttonHyperlink 
						onclick="wrapWithLink(document.getElementById('PostComment.ascx_tbComment')); return false;" 
						alt="Hyperlink" 
						src="~/skins/KeyWest/images/comment_button_hyperlink.jpg" runat="server">
				</div>
			</td>
		</tr>
		<tr>
			<td>
			</td>
			<td>
				<div class="input">
					<asp:TextBox id="tbComment" CssClass="fixed" runat="server" TextMode="MultiLine" cols="55" rows="7"></asp:TextBox>
				</div>
			</td>
		</tr>
		<TR>
			<TD colSpan="2"></TD>
		</TR>
	</table>
	<div>
		<div class="label">&nbsp;</div>
		<div class="input"><asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="True"></asp:CheckBox></div>
		<div class="clear"></div>
		<div class="label">&nbsp;</div>
		<div class="input"><asp:Button id="btnSubmit" runat="server" Text="Comment" CssClass="button" /><input type="reset" name="reset" value="Reset" class="button"><asp:Label id="Message" runat="server" ForeColor="Red" /></div>
		<div class="clear"></div>
		<div class="label">&nbsp;</div>
	</div>
</div>
<script>
function mozWrap(txtarea, lft, rgt) {
	if(!lft) lft = "";
	if(!rgt) rgt = "";
    var scrollPosition = txtarea.scrollTop;
	var insertLength = lft.length + rgt.length;
	var selLength = txtarea.textLength;
	var selStart = txtarea.selectionStart;
	var selEnd = txtarea.selectionEnd;
	if (selEnd==1 || selEnd==2) selEnd=selLength;
	var s1 = (txtarea.value).substring(0,selStart);
	var s2 = (txtarea.value).substring(selStart, selEnd);
	var s3 = (txtarea.value).substring(selEnd, selLength);
	txtarea.value = s1 + lft + s2 + rgt + s3;
	txtarea.selectionStart = txtarea.selectionEnd = selEnd + insertLength;
	txtarea.focus();
    txtarea.scrollTop = scrollPosition;
}
	
function IEWrap(txtarea, lft, rgt, doc) {
    if(!doc)
	    doc = document;
	document.all['PostComment.ascx_tbComment'].focus();
    strSelection = document.selection.createRange().text;
    if(strSelection != "") {
	doc.selection.createRange().text = lft + strSelection + rgt;
    } else {
	doc.selection.createRange().text = lft + rgt;
    }
}
// doc is the document object that all of this is on.
function wrapSelection(txtarea, lft, rgt, doc) {
	txtarea = document.getElementById('PostComment_ascx_tbComment');
    if(!doc)
	    doc = document;

    if (doc.all && doc.selection && doc.selection.createRange) {IEWrap(txtarea, lft, rgt, doc);}
    else if (doc.getElementById) {mozWrap(txtarea, lft, rgt);}
}
function wrapWithLink(txtarea, link, doc) {
    if(link == null)
	link = prompt("Enter URL:","");
    if (link != null) {
	var lft='<a href="' + link + '">';
	var rgt="</a>";
	wrapSelection(txtarea, lft, rgt, doc);
    } else {
	    wrapSelection(txtarea, '<a href="" target="_blank">', '</a>', doc);
    }
    return;
}	
function insertText(txtarea, text){wrapSelection(txtarea, text, "");}
function wrapWithBold(txtarea) {wrapSelection(txtarea, "<b>", "</b>");}
function wrapWithItalic(txtarea){wrapSelection(txtarea, "<i>", "</i>");}
function wrapWithUnderline(txtarea){wrapSelection(txtarea, "<u>", "</u>");}
function wrapWithBlockquote(txtarea){wrapSelection(txtarea, "<blockquote>\n", "\n</blockquote>\n");}

</script>
