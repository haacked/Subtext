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
using System.Xml.Serialization;
using Subtext.Web.Properties;

namespace Subtext.Web.SiteMap
{
    [XmlType(TypeName = "url")]
    public class UrlElement
    {
        private Uri _pageUrl;
        private decimal _priority;

        /// <summary>
        /// We need this contructor if we want to serialize the class.
        /// </summary>
        public UrlElement()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageUrl">URL of the page. This URL must begin with the protocol (such as http) and end with a trailing slash, if your web server requires it. This value must be less than 2048 characters.</param>
        /// <param name="lastModified">he date of last modification of the file. This date should be in W3C Datetime format. This format allows you to omit the time portion, if desired, and use YYYY-MM-DD.</param>
        /// <param name="changeFrequency">How frequently the page is likely to change. This value provides general information to search engines and may not correlate exactly to how often they crawl the page. Valid values are:
        /// * always
        /// * hourly
        /// * daily
        /// * monthly
        /// * yearly
        /// * never
        /// The value "always" should be used to describe documents that change each time they are accessed. The value "never" should be used to describe archived URLs.
        /// Please note that the value of this tag is considered a hint and not a command. Even though search engine crawlers consider this information when making decisions, they may crawl pages marked "hourly" less frequently than that, and they may crawl pages marked "yearly" more frequently than that. It is also likely that crawlers will periodically crawl pages marked "never" so that they can handle unexpected changes to those pages.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0. This value has no effect on your pages compared to pages on other sites, and only lets the search engines know which of your pages you deem most important so they can order the crawl of your pages in the way you would most like.
        /// The default priority of a page is 0.5.
        /// Please note that the priority you assign to a page has no influence on the position of your URLs in a search engine's result pages. Search engines use this information when selecting between URLs on the same site, so you can use this tag to increase the likelihood that your more important pages are present in a search index.
        /// Also, please note that assigning a high priority to all of the URLs on your site will not help you. Since the priority is relative, it is only used to select between URLs on your site; the priority of your pages will not be compared to the priority of pages on other sites.</param>
        public UrlElement(Uri pageUrl, DateTime lastModified, ChangeFrequency changeFrequency, decimal priority)
        {
            _pageUrl = pageUrl;
            LastModified = lastModified;
            ChangeFrequency = changeFrequency;
            Priority = priority;
        }

        [XmlElement(DataType = "anyURI", ElementName = "loc")]
        public string Location
        {
            get
            {
                string encodedString = _pageUrl.ToString();
                return encodedString;
            }
            set { _pageUrl = new Uri(value); }
        }

        [XmlElement(ElementName = "lastmod", DataType = "date")]
        public DateTime LastModified { get; set; }

        [XmlElement(ElementName = "changefreq")]
        public ChangeFrequency ChangeFrequency { get; set; }

        [XmlElement(ElementName = "priority")]
        public decimal Priority
        {
            get { return _priority; }
            set
            {
                if (value < 0.0M || value > 1.0M)
                {
                    throw new ArgumentOutOfRangeException(Resources.ArgumentOutOfRange_Priority);
                }
                _priority = value;
            }
        }
    }
}