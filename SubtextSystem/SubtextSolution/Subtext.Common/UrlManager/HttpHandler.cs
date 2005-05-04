#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Subtext.Common.UrlManager
{

	public enum HandlerType
	{
		Direct,
		Factory,
		Page
	};

	/// <summary>
	/// Summary description for HttpHandler.
	/// </summary>
	public class HttpHandler
	{
		public HttpHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private readonly object HandlerLock = new object();

		private string _pattern;
		[XmlAttribute("pattern")]
		public string Pattern
		{
			get {return this._pattern;}
			set {this._pattern = value;}
		}

		private string _type;
		[XmlAttribute("type")]
		public string Type
		{
			get {return this._type;}
			set {this._type = value;}
		}

		private string _PageLocation;
		[XmlAttribute("pageLocation")]
		public string PageLocation
		{
			get {return this._PageLocation;}
			set {this._PageLocation = value;}
		}

		private string _fullPageLocation;
		public string FullPageLocation
		{
			get 
			{
				if(this._fullPageLocation == null && PageLocation != null)
				{
					this._fullPageLocation = HttpContext.Current.Server.MapPath("~/" + PageLocation);
				}
				return this._fullPageLocation;
			}
		}

		private string _controls;
		[XmlAttribute("controls")]
		public string Controls
		{
			get {return this._controls;}
			set {this._controls = value;}
		}

		public string[] BlogControls
		{
			get
			{
				if(this.Controls != null)
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
				if(this._httpHanlderType == null)
				{
					lock(HandlerLock)
					{
						if(this._httpHanlderType == null)
						{
							this._httpHanlderType = System.Type.GetType(Type);
						}
					}
				}
				return this._httpHanlderType;
			}		
		}

		private ConstructorInfo constructor = null;
		public object Instance()
		{
			if(constructor == null)
			{
				lock(HandlerLock)
				{
					if(constructor == null)
					{
						System.Type t = System.Type.GetType(this.Type);
						constructor = t.GetConstructor(new Type[0]);
					}
				}
			}
			return constructor.Invoke(null);
		}

		private Regex _urlRegex;
		//Will throw an exception without Ignore Attribute..err...
		//Cache the Regex so that it does not have to be constantly recreated. This will also allow us to use Compiled expressions as well.
		[XmlIgnoreAttribute]
		public Regex UrlRegex
		{
			//Should we add a check here for a null Pattern. We will throw an exception if the Pattern is null...but not much else we can
			//do about it either
			get
			{
				if(_urlRegex == null)
				{
					_urlRegex = new Regex(this.Pattern,RegexOptions.IgnoreCase|RegexOptions.Compiled);
				}
				return _urlRegex;
			}
		}

		public bool IsMatch(string url)
		{
			return UrlRegex.IsMatch(url);
		}


		private HandlerType _handlerType = HandlerType.Page;
		[XmlAttribute("handlerType")]
		public HandlerType HandlerType
		{
			get {return this._handlerType;}
			set {this._handlerType = value;}
		}
	}
}
