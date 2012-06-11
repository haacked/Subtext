FCKConfig.TagLinks = new Object();
FCKConfig.TagLinks.ExternalUrl = 'http://technorati.com/tags/';
FCKConfig.TagLinks.InternalUrl = '';
FCKConfig.TagLinks.ExternalCaption = 'Technorati tags: ';
FCKConfig.TagLinks.InternalCaption = 'Internal tags: ';
FCKConfig.TagLinks.IncludeHr = true;
FCKConfig.TagLinks.IncludeInternal = false;
FCKConfig.TagLinks.IncludeExternal = true;

FCKConfig.Plugins.Add( 'taglinks', 'en') ;

FCKConfig.ToolbarSets["SubText"] = [
	['Source','-','NewPage','Preview','-','Templates'],
	['Cut','Copy','Paste','PasteText','PasteWord','-','SpellCheck'],
	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	['FitWindow','-','About'],
	'/',
	['Bold','Italic','Underline','StrikeThrough','-','Subscript','Superscript'],
	['OrderedList','UnorderedList','-','Outdent','Indent'],
	['JustifyLeft','JustifyCenter','JustifyRight','JustifyFull'],
	['Link','Unlink','Anchor'],
	['Image','Table','Rule','Smiley'],
	'/',
	['Style','FontFormat','FontName','FontSize'],
	['TextColor','BGColor'],
	['Taglinks']
] ;
