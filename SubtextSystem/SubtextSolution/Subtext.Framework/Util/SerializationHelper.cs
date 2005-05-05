using System;
using System.IO;
using System.Xml.Serialization;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for SerializationHelper.
	/// </summary>
	public class SerializationHelper
	{
		private SerializationHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static object Load(Type type, string filename)
		{
			FileStream fs = null;
			try
			{
				// open the stream...
				fs = new FileStream(filename, FileMode.Open,FileAccess.Read);
				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(fs);
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				if(fs != null)
					fs.Close();
			}
		}


		public static void Save(object obj, string filename)
		{
			FileStream fs = null;
			// serialize it...
			try
			{
				fs = new FileStream(filename, FileMode.Create);
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				serializer.Serialize(fs, obj);	
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(fs != null)
					fs.Close();
			}

		}
	}
}
