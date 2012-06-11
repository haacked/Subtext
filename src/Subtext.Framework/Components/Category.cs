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

using System.Xml.Serialization;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Components
{
    public class Category : IIdentifiable
    {
        public string Title { get; set; }

        public int BlogId { get; set; }

        #region IIdentifiable Members

        [XmlAttribute("CategoryId")]
        public int Id { get; set; }

        #endregion
    }
}