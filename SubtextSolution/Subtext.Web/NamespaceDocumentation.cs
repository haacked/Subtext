#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

// This file contains documentation for the various Namespaces within this assembly.
// These classes are only used by NDoc to generate namespace documentation.
// They should all be internal sealed classes with private constructors.
//
// http://ndoc.sourceforge.net/reference/NDoc.Core.Reflection.BaseReflectionDocumenterConfig.UseNamespaceDocSummaries.html


#if DOCUMENTATION

namespace Subtext.Web
{
    /// <summary>
    /// This is the top level namespace for the Subtext Web application.  
    /// It contains classes and sub-name spaces focused on rendering 
    /// the contents of a blog.
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
}

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// This namespace contains all the controls used to render a blog.  
    /// Notice that these controls have corresponding declarations within 
    /// the .ascx files in any given skins directory.  These controls provide 
    /// the code and logic for those user controls.
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
}

namespace Subtext.Web.UI.Handlers
{
    /// <summary>
    /// Contains various classes that implement <see cref="System.Web.IHttpHandler" />.  
    /// Many handlers have been migrated to the Subtext.Framework namespace.
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
}

namespace Subtext.Web.UI.Pages
{
    /// <summary>
    /// Contains base page classes for the Subtext system.  Currently only 
    /// contains the <see cref="SubtextMasterPage" /> class.
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
}

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Contains the ASPX pages and code behind for the Admin section 
    /// used to configure and administrate a Subtext blog.
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
}

#endif