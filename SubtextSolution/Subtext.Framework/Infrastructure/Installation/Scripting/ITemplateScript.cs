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

namespace Subtext.Scripting
{
    /// <summary>
    /// Template scripts contain template parameters that require user supplied values. 
    /// Defaults may be specified.  This works the same way as template parameters in 
    /// SQL Server.
    /// </summary>
    /// <remarks>
    /// For more information about template parameters, see
    /// <see href="http://haacked.com/archive/2005/07/01/7433.aspx" /> this post.
    /// </remarks>
    public interface ITemplateScript
    {
        /// <summary>
        /// Gets the template parameters embedded in the script.
        /// </summary>
        /// <returns></returns>
        TemplateParameterCollection TemplateParameters { get; }
    }
}