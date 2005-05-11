using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Contains methods for binary serialization and deserialization 
	/// of objects.
	/// </summary>
	public sealed class BinarySerializer
	{
		private BinarySerializer()
		{
		}

		/// <summary>
		/// Serializes the specified value (and its object graph) 
		/// into an in-memory byte array which is returned.
		/// </summary>
		/// <param name="value">Value.</param>
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

		/// <summary>
		/// Deserializers the specified serialized extended attributes.
		/// </summary>
		/// <param name="serializedExtendedAttributes">Serialized extended attributes.</param>
		/// <returns></returns>
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
