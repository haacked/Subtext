#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml.Serialization;
using Subtext.Framework.Threading;

namespace Subtext.Framework.UrlManager
{
	/// <summary>
	/// Class used to match URLS to the controls that render the URL.
	/// </summary>
	public class HttpHandler
	{
		private readonly object handlerLock = new object();

		private string _pattern;
		[XmlAttribute("pattern")]
		public string Pattern
		{
			get { return this._pattern; }
			set { this._pattern = value; }
		}

		private string _type;
		[XmlAttribute("type")]
		public string Type
		{
			get { return this._type; }
			set { this._type = value; }
		}

		private string _PageLocation;
		/// <summary>
		/// <p>Gets or sets the page location.  This is the location 
		/// of the page used when the handler type is <see cref="Page"/>. 
		/// </p>
		/// <p>
		/// The format for the page location should be a path relative to 
		/// the ApplicationPath (no beginning slash).
		/// </p>
		/// </summary>
		/// <example>
		/// If Subtext is installed in a virtual directory name "Subtext.Web" and 
		/// the virtual path to the page is "/Subtext.Web/SomeDirectory/MyPage.aspx", 
		/// the PageLocation should be "SomeDirectory/MyPage.aspx".
		/// </example>
		/// <value></value>
		[XmlAttribute("pageLocation")]
		public string PageLocation
		{
			get { return this._PageLocation; }
			set { this._PageLocation = value; }
		}

		/// <summary>
		/// <p>Gets or sets the actual directory location.  This is the location 
		/// of the directory used to find the actual file when the handler type 
		/// is <see cref="Page"/>. 
		/// </p>
		/// <p>
		/// The format for the directory location should be a path relative to 
		/// the ApplicationPath (no beginning slash).
		/// </p>
		/// </summary>
		/// <example>
		/// <p>If Subtext is installed in a virtual directory name "Subtext.Web" and 
		/// the virtual path to the directory is "/Subtext.Web/Admin/", 
		/// the PageLocation should be "Admin".
		/// </p>
		/// <p>
		/// Thus requests for http://host/Subtext.Web/MyBlog/Admin/SomePage.aspx 
		/// will be handled by http://host/Subtext.Web/Admin/SomePage.aspx
		/// </p>
		/// </example>
		/// <value></value>
		[XmlAttribute("directoryLocation")]
		public string DirectoryLocation
		{
			get { return this._directoryLocation; }
			set { this._directoryLocation = value; }
		}
		string _directoryLocation = string.Empty;

		private string _fullPageLocation;

		/// <summary>
		/// Gets the full physical path to the page specified 
		/// by <see cref="PageLocation"/>.
		/// </summary>
		/// <value></value>
		public string FullPageLocation
		{
			get
			{
				if (this._fullPageLocation == null && PageLocation != null)
				{
					this._fullPageLocation = HttpContext.Current.Request.MapPath("~/" + PageLocation);
				}
				return this._fullPageLocation;
			}
		}

		private string _controls;
		[XmlAttribute("controls")]
		public string Controls
		{
			get { return this._controls; }
			set { this._controls = value; }
		}

		public string[] BlogControls
		{
			get
			{
				if (this.Controls != null)
				{
					return this.Controls.Split(',');
				}
				return null;
			}
		}

		private Type _httpHanlderType;
		public Type HttpHanlderType
		{
			get
			{
				if (this._httpHanlderType == null)
				{
					using (TimedLock.Lock(handlerLock))
					{
						if (this._httpHanlderType == null)
						{
							this._httpHanlderType = System.Type.GetType(Type);
						}
					}
				}
				return this._httpHanlderType;
			}
		}

		private ConstructorInfo constructor;
		public object Instance()
		{
			if (constructor == null)
			{
				using (TimedLock.Lock(handlerLock))
				{
					if (constructor == null)
					{
						Type t = System.Type.GetType(this.Type);
						constructor = t.GetConstructor(new Type[0]);
					}
				}
			}
			return constructor.Invoke(null);
		}

		private Regex _urlRegex;
		//Will throw an exception without Ignore Attribute..err...
		//Cache the Regex so that it does not have to be constantly recreated. 
		//This will also allow us to use Compiled expressions as well.
		[XmlIgnoreAttribute]
		public Regex UrlRegex
		{
			//Should we add a check here for a null Pattern. We will throw an exception if the Pattern is null...but not much else we can
			//do about it either
			get
			{
				if (_urlRegex == null)
				{
					_urlRegex = new Regex(this.Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
				}
				return _urlRegex;
			}
		}

		/// <summary>
		/// Returns true if the regex matches the specified URL.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <returns></returns>
		public bool IsMatch(string url)
		{
			return UrlRegex.IsMatch(url);
		}


		private HandlerType _handlerType = HandlerType.Page;
		[XmlAttribute("handlerType")]
		public HandlerType HandlerType
		{
			get { return this._handlerType; }
			set { this._handlerType = value; }
		}
	}
}
