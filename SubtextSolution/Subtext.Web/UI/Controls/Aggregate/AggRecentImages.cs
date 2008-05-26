using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Providers;
using System.Globalization;
using Subtext.Framework;
using System.Text.RegularExpressions;

namespace Subtext.Web.UI.Controls
{
    public partial class AggRecentImages : BaseControl
    {
        protected Repeater RecentImages;

        private string _appPath;
        private string _fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";
        private int _count;

        /// <summary>
        /// Prroperty to limit the number of images displayed. Default is 35.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int groupId = 0;

            if (Request.QueryString["GroupID"] != null)
            {
                try
                {
                    groupId = Int32.Parse(Request.QueryString["GroupID"]);
                }
                catch { }

            }

            DataTable dt = DbProvider.Instance().GetAggregateRecentImages(groupId);
            while (dt.Rows.Count > _count)
            {
                dt.Rows.RemoveAt(dt.Rows.Count - 1);
            }
            RecentImages.DataSource = dt;
            RecentImages.DataBind();
        }

        protected string GetEntryUrl(string host, string app, string entryName, DateTime dt)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}archive/{1:yyyy/MM/dd}/{2}.aspx", GetFullUrl(host, app), dt, entryName);
        }
         
        protected string GetFullUrl(string host, string app)
        {
            if (_appPath == null)
            {
                _appPath = HttpContext.Current.Request.ApplicationPath;
                if (!_appPath.ToLower(CultureInfo.InvariantCulture).EndsWith("/"))
                {
                    _appPath += "/";
                }
            }

            if (Request.Url.Port != 80)
            {
                host += ":" + Request.Url.Port;
            }

            return string.Format(_fullUrl, host, _appPath, app);
        }


        protected string GetImageUrl(string catID, string host, string app, string imageFile)
        {
            if (!String.IsNullOrEmpty(app))
            {
                app = "/" + app;
            }
            string baseImagePath = Images.HttpGalleryFilePath(Context, Int32.Parse(catID));
            string virtualPath = "http://" + host + string.Format(CultureInfo.InvariantCulture, "/images/{0}{1}/", Regex.Replace(host, @"\:|\.", "_"), app);
            return virtualPath + baseImagePath + "t_" + imageFile;
        }

        protected string GetAlbumUrl(string catID, string host, string app, string imageFile)
        {
            if (!String.IsNullOrEmpty(app))
            {
                app = "/" + app;
            }
            string baseImagePath = Images.HttpGalleryFilePath(Context, Int32.Parse(catID)).Replace("/", "");
            return "http://" + host + app + "/Gallery/" + baseImagePath + ".aspx";
        }

        protected string GetImageLink(string catID, string host, string app, string imageFile)
        {
            if (!String.IsNullOrEmpty(app))
            {
                app = "/" + app;
            }
            string baseImagePath = Images.HttpGalleryFilePath(Context, Int32.Parse(catID));
            string virtualPath = "http://" + host + string.Format(CultureInfo.InvariantCulture, "/images/{0}{1}/", Regex.Replace(host, @"\:|\.", "_"), app);
            return virtualPath + baseImagePath + "r_" + imageFile;
        }

    }
}
