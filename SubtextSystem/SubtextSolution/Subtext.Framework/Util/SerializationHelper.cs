using System;
using System.IO;
using System.Xml.Serialization;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Class with methods for saving and loading objects as 
	/// serialized instances.
	/// </summary>
	public sealed class SerializationHelper
	{
		private SerializationHelper()
		{
		}

		/// <summary>
		/// Loads the specified type.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="filename">Filename.</param>
		/// <returns></returns>
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


		/// <summary>
		/// Saves the specified object at the specified location 
		/// as a serialized file.
		/// </summary>
		/// <param name="obj">Obj.</param>
		/// <param name="filename">Filename.</param>
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
