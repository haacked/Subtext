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

using System;
using CookComputing.XmlRpc;

namespace Subtext.Framework.XmlRpc
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMember("categories", Description = "Contains categories for the post.")]
        public string[] categories;

        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public
            DateTime? dateCreated;

        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string
            description;

        public Enclosure? enclosure;
        [XmlRpcMember("mt_excerpt")]
        public string excerpt;
        public string link;
        public string permalink;

        [XmlRpcMember(
            Description = "Not required when posting. Depending on server may "
                          + "be either string or integer. "
                          + "Use Convert.ToInt32(postid) to treat as integer or "
                          + "Convert.ToString(postid) to treat as string")]
        public object postid;

        public Source source;

        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string
            title;

        public string userid;
        public string wp_slug;
    }
}