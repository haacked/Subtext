/*
	Prototype for a javascript BlogInfo object 
	that provides information to client scripts 
	much as a server version does.
*/
function blogInfo(virtualRoot, virtualBlogRoot)
{
	this.virtualRoot = virtualRoot;
	this.virtualBlogRoot = virtualBlogRoot;
	
	this.getVirtualRoot = getVirtualRoot;
	this.getVirtualBlogRoot = getVirtualBlogRoot;
	this.getScriptsVirtualRoot = getScriptsVirtualRoot;
	this.getImagesVirtualRoot = getImagesVirtualRoot;
	
	/*
	Returns the virtual root for the entire website.
	*/
	function getVirtualRoot()
	{
		return this.virtualRoot;
	}
	
	/*
	Gets the virtual root for the specific blog.
	*/
	function getVirtualBlogRoot()
	{
		return this.virtualBlogRoot;
	}
	
	/*
	Returns the virtual root to the default "scripts" directory 
	*/
	function getScriptsVirtualRoot()
	{
		return this.virtualRoot + "Scripts/";
	}
	
	/*
	Returns the virtual root to the default "scripts" directory 
	*/
	function getImagesVirtualRoot()
	{
		return this.virtualRoot + "Images/";
	}
}