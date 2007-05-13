using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using Subtext.Framework.Util.TimeZoneUtil;

namespace Subtext.Tools
{
	public partial class WindowsTimeZoneCollectionGenerator : Form
	{
		public WindowsTimeZoneCollectionGenerator()
		{
			InitializeComponent();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveDialog = GetSaveDialog();

			if (saveDialog.ShowDialog() == DialogResult.OK)
			{
				RegistryPermission permission = new RegistryPermission(
						RegistryPermissionAccess.Read,
						"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones");

				try
				{
					permission.Demand();

					using (StreamWriter writer = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
					{
						Type tzcType = typeof(WindowsTimeZoneCollection);
						XmlSerializer ser = new XmlSerializer(tzcType);
						ser.Serialize(writer, LoadTimeZonesFromRegistry());
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: LoadTimeZonesFromRegistry " + ex);
				}
			}
		}

		private static SaveFileDialog GetSaveDialog()
		{
			SaveFileDialog saveDialog = new SaveFileDialog();

			saveDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
			saveDialog.FileName = "WindowsTimeZoneCollection";
			saveDialog.AddExtension = true;
			saveDialog.DefaultExt = "xml";
			saveDialog.InitialDirectory = @"c:\";
			saveDialog.Title = "Location to save file.";
			saveDialog.OverwritePrompt = true;
			return saveDialog;
		}

		private static WindowsTimeZoneCollection LoadTimeZonesFromRegistry()
		{
			WindowsTimeZoneCollection tzs = new WindowsTimeZoneCollection();

			RegistryKey timeZoneListKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
			string[] timeZoneKeyNames = timeZoneListKey.GetSubKeyNames();
			foreach (string timeZoneKeyName in timeZoneKeyNames)
			{
				RegistryKey timeZoneKey = timeZoneListKey.OpenSubKey(timeZoneKeyName);
				WindowsTimeZone windowsTimeZone = new WindowsTimeZone();
				WindowsTZI tzi = new WindowsTZI();
				tzi.bias = 0;

				windowsTimeZone.DisplayName = timeZoneKey.GetValue("Display") as string;
				windowsTimeZone.DaylightZoneName = timeZoneKey.GetValue("Dlt") as string;
				windowsTimeZone.StandardZoneName = timeZoneKey.GetValue("Std") as string;
				windowsTimeZone.ZoneIndex = (int)timeZoneKey.GetValue("Index");
				tzi.InitializeFromByteArray(timeZoneKey.GetValue("TZI") as byte[], 0);
				windowsTimeZone.WinTZI = tzi;

				tzs.Add(windowsTimeZone);
			}
			tzs.SortByTimeZoneBias();
			return tzs;
		}
	}
}