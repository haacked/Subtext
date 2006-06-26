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
using System.IO;
using System.Xml.Serialization;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Class with methods for saving and loading objects as 
	/// serialized instances.
	/// </summary>
	public static class SerializationHelper
	{
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
			finally
			{
				if(fs != null)
					fs.Close();
			}
		}

        /// <summary>
        /// Loads the specified type based on the specified stream.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stream">stream containing the type.</param>
        /// <returns></returns>
        public static object Load(Type type, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(stream);
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
			finally
			{
				if(fs != null)
					fs.Close();
			}

		}
	}
}
