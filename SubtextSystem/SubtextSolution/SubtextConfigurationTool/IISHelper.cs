using System;
using System.Collections;
using System.DirectoryServices;
using System.Globalization;

namespace SubtextConfigurationTool
{
	/// <summary>
	/// Summary description for WebSitesList.
	/// </summary>
	public sealed class IISHelper
	{
		public static ArrayList EnumerateVirtualDirectories()
		{
			ArrayList list = new ArrayList();
			DirectoryEntry node = new DirectoryEntry("IIS://localhost/W3SVC");

			foreach (DirectoryEntry e in node.Children)
			{
				if( e.SchemaClassName == "IIsWebServer")
				{
					if(e.Properties["ServerComment"].Value.ToString() == "Default Web Site")
					{
						foreach(DirectoryEntry child in e.Children.Find("ROOT", "IIsWebVirtualDir").Children)
						{
							string friendlyName = child.Properties["AppFriendlyName"].Value.ToString().Trim();
							string path = GetPhysicalPath(child);
							if(friendlyName.Length > 0 && friendlyName != "Default Application")
							{
								list.Add(new VirtualDirectory(friendlyName, path));
							}
						}						
					}
				}
			}
			return list;
		}

		public static string GetPhysicalPath(DirectoryEntry dir)
		{
			try
			{
				string path = dir.Properties["path"][0].ToString();
				return path;
			}
			catch(Exception)
			{
				DirectoryEntry parent;
				try
				{
					parent = dir.Parent;
				}
				catch(Exception)
				{
					return "";
				}
				string parentPath = GetPhysicalPath(parent);
				string TrimmedName = dir.Path.Substring(dir.Path.LastIndexOf(@"/")+1);
				return string.Format(CultureInfo.InvariantCulture, @"{0}\{1}",
					parentPath,TrimmedName);
			}
		}	 
		
		private IISHelper()
		{}

		
	}
	public class VirtualDirectory
	{
		public string PhysicalPath
		{
			get { return _physicalPath; }
		}

		string _physicalPath;

		public string FriendlyName
		{
			get { return _friendlyName; }
		}

		string _friendlyName;

		public VirtualDirectory(string name, string path)
		{
			_friendlyName = name;
			_physicalPath = path;
		}
	}

}
