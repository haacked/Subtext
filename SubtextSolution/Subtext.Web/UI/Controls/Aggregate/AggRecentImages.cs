using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Data;
using System.Globalization;
using Subtext.Framework;
using System.Text.RegularExpressions;

namespace Subtext.Web.UI.Controls
{
    public class AggRecentImages : BaseControl
    {
        protected Repeater RecentImages;

        private int _Count;

        /// <summary>
        /// Prroperty to limit the number of images displayed. Default is 35.
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int groupID = 0;

            if (Request.QueryString["GroupID"] != null)
            {
                Int32.TryParse(Request.QueryString["GroupID"], out groupID);
            }

        	DataSet ds = StoredProcedures.DNWGetRecentImages(BlogInfo.AggregateBlog.Host, groupID).GetDataSet();
			if (ds.Tables.Count > 0)
			{
				DataTable dt = ds.Tables[0];
				while (dt.Rows.Count > _Count)
					dt.Rows.RemoveAt(dt.Rows.Count - 1);
				RecentImages.DataSource = dt;
				RecentImages.DataBind();
			}
        }

        protected string GetEntryUrl(string host, string app, string entryName, DateTime dt)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}archive/{1:yyyy/MM/dd}/{2}.aspx", GetFullUrl(host, app), dt, entryName);
        }
         

        private string appPath;
    	readonly string fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";
        protected string GetFullUrl(string host, string app)
        {
            if (appPath == null)
            {
                appPath = HttpContext.Current.Request.ApplicationPath;
                if (!appPath.ToLower(CultureInfo.InvariantCulture).EndsWith("/"))
                {
                    appPath += "/";
                }
            }

            if (Request.Url.Port != 80)
            {
                host += ":" + Request.Url.Port;
            }

            return string.Format(fullUrl, host, appPath, app);

        }


        protected string GetImageUrl(string catID, string host, string app, string imageFile)
        {
            if (app != string.Empty)
                app = "/" + app;
			string baseImagePath = Images.GalleryVirtualUrl(Int32.Parse(catID));
            string virtualPath = "http://" + host + string.Format(CultureInfo.InvariantCulture, "/images/{0}{1}/", Regex.Replace(host, @"\:|\.", "_"), app);
            return virtualPath + baseImagePath + "t_" + imageFile;
        }

        protected string GetAlbumUrl(string catID, string host, string app, string imageFile)
        {
            if (app != string.Empty)
                app = "/" + app;
			string baseImagePath = Images.GalleryVirtualUrl(Int32.Parse(catID)).Replace("/", "");
            return "http://" + host + app + "/Gallery/" + baseImagePath + ".aspx";
        }

        protected string GetImageLink(string catID, string host, string app, string imageFile)
        {
            if (app != string.Empty)
                app = "/" + app;
			string baseImagePath = Images.GalleryVirtualUrl(Int32.Parse(catID));
            string virtualPath = "http://" + host + string.Format(CultureInfo.InvariantCulture, "/images/{0}{1}/", Regex.Replace(host, @"\:|\.", "_"), app);
            return virtualPath + baseImagePath + "r_" + imageFile;
        }

    }
}
