﻿/*
FCKCommands.RegisterCommand(commandName, command)
       commandName - Command name, referenced by the Toolbar, etc...
       command - Command object (must provide an Execute() function).
*/
// Register the related commands.
var TaglinksCommand=function(){
    this.Name='Taglinks';
};
TaglinksCommand.prototype.AddLinks=function(tArr, caption, url, isInternal){
    if (FCKConfig.TagLinks.IncludeHr == true) FCK.ExecuteNamedCommand('InsertHorizontalRule');
    FCK.InsertHtml(caption);
    for (i = 0; i < tArr.length; i++) 
    {
        var a = FCK.EditorDocument.createElement('a');
        a.rel = 'tag';

        if (isInternal == true) a.href += '/Default.aspx';
        var t = FCK.EditorDocument.createTextNode(tArr[i]);
        a.appendChild(t);
        FCK.InsertElement(a);
        if (i < tArr.length - 1) FCK.InsertHtml(', ');
    }
}

TaglinksCommand.prototype.Execute=function(){
    var tags = prompt('Enter tags separated by comas', '');
    if (!tags || tags.length == 0) return;
    tagsArr = tags.split(',');
    FCK.ExecuteNamedCommand('SelectAll');
    FCKSelection.Collapse(false);
    if (FCKConfig.TagLinks.IncludeExternal == true)
        this.AddLinks(tagsArr, FCKConfig.TagLinks.ExternalCaption, FCKConfig.TagLinks.ExternalUrl, false);
    if (FCKConfig.TagLinks.IncludeInternal == true)
    {
        if (FCKConfig.TagLinks.InternalUrl == '') {
            if (FCKBrowserInfo.IsGeckoLike == true)
                s= FCK.LinkedField.baseURI;
            else
                s= FCK.LinkedField.document.location.href;
            i = s.toLowerCase().indexOf('/admin/');
            if (i >= 0) s = s.substr(0, i+1);
            s = s + 'Tags/';
            FCKConfig.TagLinks.InternalUrl = s;
        }
        this.AddLinks(tagsArr, FCKConfig.TagLinks.InternalCaption, FCKConfig.TagLinks.InternalUrl, true);
    }
    FCK.EditorWindow.scrollTo(0, FCK.EditorWindow.innerHeight);
}

TaglinksCommand.prototype.GetState=function(){
    return FCK_TRISTATE_OFF;}
    
FCKCommands.RegisterCommand('Taglinks', new TaglinksCommand());
    
// Create the "Taglinks" toolbar button.
var oTaglinks = new FCKToolbarButton('Taglinks', FCKLang['TaglinksBtn']);
oTaglinks.IconPath = FCKPlugins.Items['taglinks'].Path + 'images/taglinks.gif' ;
// 'Taglinks' is the name used in the Toolbar config.
FCKToolbarItems.RegisterItem( 'Taglinks', oTaglinks ) ;
