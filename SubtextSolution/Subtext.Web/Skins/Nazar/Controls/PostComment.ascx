<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>

<div id="comments">
	<div id="commentform">
		<h3>Enter your comment here</h3>
		<p align="center">Don't leave your e-mail address. HTML is not allowed. </p>
		<p align="center"><b>OFFENSIVE COMMENTS WILL BE DELETED!!!</b></p>
		<div class="label">
			<label for="PostComment.ascx_tbTitle">Title:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbTitle" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" />
		</div>
		<br />
		<div class="label">
			<label for="PostComment.ascx_tbName">Name:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbName" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
		</div>
		<br />
		<div class="label">
			<label for="PostComment.ascx_tbUrl">Website:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbUrl" runat="server" CssClass="fixed"></asp:TextBox>
		</div>
		<br />
		<div class="label">
			<label for="PostComment.ascx_tbComment">Comment:</label>
		</div>
		<div class="commentButtons">
			<img id="buttonBold"
				runat="server"
				onclick="wrapWithBold(document.getElementById('PostComment.ascx_tbComment')); return false;" 
				alt="italic"
				src="~/skins/Joomla/images/comment_button_bold.gif" />
			<img id="buttonItalic"
				runat="server"
				onclick="wrapWithItalic(document.getElementById('PostComment.ascx_tbComment')); return false;" 
				alt="Italic"
				src="~/skins/Joomla/images/comment_button_italic.gif" />
			<img id="buttonUnderline"
				runat="server"
				onclick="wrapWithUnderline(document.getElementById('PostComment.ascx_tbComment')); return false;" 
				alt="Underline"
				src="~/skins/Joomla/images/comment_button_underline.gif" />
			<img id="buttonBlockquote"
				runat="server"
				onclick="wrapWithBlockquote(document.getElementById('PostComment.ascx_tbComment')); return false;" 
				alt="Blockquote"
				src="~/skins/Joomla/images/comment_button_blockquote.gif" />
			<img id="buttonHyperlink"
				runat="server"
				onclick="wrapWithLink(document.getElementById('PostComment.ascx_tbComment')); return false;" 
				alt="Hyperlink"
				src="~/skins/Joomla/images/comment_button_hyperlink.gif" />
				Click on the buttons for allowed HTML mark-ups
		</div>
				
		<div class="input">
			<asp:TextBox id="tbComment" runat="server" rows="7" cols="55" CssClass="fixed"
				TextMode="MultiLine" onkeyup="reloadPreviewDiv();"></asp:TextBox>
			<br/>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator>
		</div>
		<div class="input">
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?"></asp:CheckBox>
		</div>
		<br />
		<div class="input">
			<asp:Button id="btnSubmit" runat="server" Text="Send Comment" CssClass="button" />
			<input type="reset" name="reset" value="Reset" class="button" />
			<asp:Label id="Message" runat="server" ForeColor="Red" />
		</div>
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
	txtarea = document.getElementById('PostComment.ascx_tbComment');
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
