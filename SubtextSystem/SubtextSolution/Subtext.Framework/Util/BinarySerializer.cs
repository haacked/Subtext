using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for BinarySerializer.
	/// </summary>
	public class BinarySerializer
	{
		private BinarySerializer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static byte[] Serialize(object value) 
		{

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			byte[] b;

			// Serialize the SiteSettings
			//
			binaryFormatter.Serialize(ms, value);

			// Set the position of the MemoryStream back to 0
			//
			ms.Position = 0;
            
			// Read in the byte array
			//
			b = new Byte[ms.Length];
			ms.Read(b, 0, b.Length);
			ms.Close();

			return b;
		}

		public static object Deserializer(byte[] serializedExtendedAttributes) 
		{

			if (serializedExtendedAttributes.Length == 0)
			{
				return null;
			}

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			try 
			{
				ms.Write(serializedExtendedAttributes, 0, serializedExtendedAttributes.Length);

				ms.Position = 0;

				return binaryFormatter.Deserialize(ms);


			} 
			finally
			{
				ms.Close();
			}        
		}
	}
}
