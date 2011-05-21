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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Subtext.Web.Admin
{
    // TODO: target, defaults to none; change Name to Title or Description, what does what? Desc can be ALT
    [Serializable]
    [XmlRoot(ElementName = "RootPage", IsNullable = true)]
    public class PageLocation
    {
        private PageLocationCollection _childLocations;
        private string _description;
        private string _id;
        private bool _isRoot;
        private PageLocation _parent;
        private string _target = String.Empty;
        private string _title;
        private string _url;

        public PageLocation()
        {
            _childLocations = new PageLocationCollection();
        }

        public PageLocation(Type pageType, string title, string url)
            : this()
        {
            _id = pageType.BaseType.FullName;
            _title = title;
            _url = url;
        }

        public PageLocation(string id, string title, string url)
            : this()
        {
            _id = id;
            _title = title;
            _url = url;
        }

        public PageLocation(string id, string title, string url, string description)
            : this()
        {
            _id = id;
            _title = title;
            _url = url;
            _description = description;
        }

        #region Accessors

        [XmlElement(ElementName = "Page", IsNullable = true)]
        public PageLocationCollection ChildLocations
        {
            get { return _childLocations; }
            set { _childLocations = value; }
        }

        [XmlAttribute("description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool IsRoot
        {
            get { return _isRoot; }
        }

        [XmlAttribute("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [XmlAttribute("id")]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public PageLocation Parent
        {
            get { return _parent; }
        }

        public bool HasChildren
        {
            get { return _childLocations.Count > 0; }
        }

        [XmlAttribute("url")]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        [XmlAttribute("target")]
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        #endregion

        public static PageLocation GetRootPage()
        {
            var result = new PageLocation();
            result._isRoot = true;
            return result;
        }

        public static PageLocation GetRootPage(PageLocation root)
        {
            root._isRoot = true;
            return root;
        }

        public void AppendChild(PageLocation newChild)
        {
            // TODO: Q? test != self or not?

            newChild.SetParent(this);

            if (SiteMap.Instance.ContainsID(newChild.ID))
            {
                SiteMap.Instance[newChild.ID] = newChild;
            }
            else
            {
                SiteMap.Instance.AddPage(newChild);
            }

            _childLocations.Add(newChild);
        }

        internal void SetParent(PageLocation parentPage)
        {
            _parent = parentPage;
        }

        public IEnumerable<PageLocation> GetAncestors()
        {
            return GetAncestors(false);
        }

        public IEnumerable<PageLocation> GetAncestors(bool includeSelf)
        {
            var ancestors = new List<PageLocation>();

            if (includeSelf)
            {
                ancestors.Add(this);
            }

            PageLocation currentAncestor = _parent;
            while (null != currentAncestor)
            {
                ancestors.Add(currentAncestor);
                if (currentAncestor.IsRoot)
                {
                    break;
                }
                currentAncestor = currentAncestor.Parent;
            }

            return ancestors;
        }

        // RemoveChild

        // GetBreadCrumbLinks -- preloaded and formatted?
    }
}