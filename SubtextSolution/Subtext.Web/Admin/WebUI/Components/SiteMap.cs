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
using System.Collections;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Subtext.Web.Admin
{
	public class SiteMap
	{
		protected const string DEFAULT_FILENAME = "navigation.config";

		private static SiteMap _instance = new SiteMap();		
		private Hashtable _pages;
		private PageLocation _root;
		private bool _isConfigured;
		
		protected SiteMap()
		{			
			_pages = new Hashtable();
		}

		public static SiteMap Instance
		{
			get { return _instance; }
		}

		public PageLocation this[string index]
		{
			get { return (PageLocation)_pages[index]; }
			set { _pages[index] = value; }
		}

		public PageLocation Root
		{
			get { return _root; }
		}

		public bool IsConfigured
		{
			get { return _isConfigured; }
		}

		public static void LoadConfiguration()
		{
			LoadConfiguration(DEFAULT_FILENAME);
		}

		// Change to ConfigHandler?
		public static void LoadConfiguration(string filePath)
		{
			string filepath = HttpContext.Current.Request.MapPath(filePath);

			XmlDocument doc = new XmlDocument();
			doc.Load(filepath);
			
			XmlNode pageLocations = doc.SelectSingleNode("/Navigation/RootPage");
			if (null != pageLocations)
			{
				System.Text.Encoding encoding = Utilities.GetEncoding(filepath);				
				byte[] buffer = encoding.GetBytes(pageLocations.OuterXml);
				MemoryStream stream = new MemoryStream(buffer);
				stream.Position = 0;
				XmlSerializer serializer = new XmlSerializer(typeof(PageLocation));	
				PageLocation newRoot = (PageLocation)serializer.Deserialize(stream);
				_instance.SetRoot(PageLocation.GetRootPage(newRoot));		
				_instance.PopulateLookupList();
			}

			_instance._isConfigured = true;
		}

		public bool ContainsID(string id)
		{
			return _pages.ContainsKey(id);
		}

		public PageLocation[] GetAncestors(string id)
		{
			return GetAncestors(id, true);
		}

		public PageLocation[] GetAncestors(string id, bool includeSelf)
		{
			if (_pages.ContainsKey(id))	
				return (_pages[id] as PageLocation).GetAncestors(includeSelf);
			else
				return null;
		}

		public void AddPage(PageLocation value)
		{
			_pages.Add(value.ID, value);
		}

		protected void ClearPages()
		{
			_pages.Clear();
		}

		protected void SetRoot(PageLocation root)
		{
			_root = root;
		}

		protected void PopulateLookupList()
		{
			ClearPages();
			RecursePageLocations(_root);
		}

		protected void RecursePageLocations(PageLocation currentLocation)
		{
			if (currentLocation.HasChildren)
			{
				foreach (PageLocation childLocation in currentLocation.ChildLocations)
				{
					childLocation.SetParent(currentLocation);
					RecursePageLocations(childLocation);
				}
			}

			AddPage(currentLocation);
		}
	}
}

