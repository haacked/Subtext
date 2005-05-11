using System;
using NUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
	/// <summary>
	/// Tests of the <seealso cref="BinarySerializer"/> utility class.
	/// </summary>
	[TestFixture]
	public class BinarySerializerTests
	{
		/// <summary>
		/// Tests round trip serialization.
		/// </summary>
		[Test]
		public void SerializeRoundTrip()
		{
			SerializableObject original = new SerializableObject();
			byte[] serialized = BinarySerializer.Serialize(original);
			
			object deserialized = BinarySerializer.Deserializer(serialized);
			SerializableObject copy = (SerializableObject)deserialized;

			Assert.AreEqual(original.SomeNumber, copy.SomeNumber, "SomeNumber changed in the round trip.");
			Assert.AreEqual(original.SomeString, copy.SomeString, "SomeString changed in the round trip.");
		}

		/// <summary>
		/// Tests serializing an object which we compare against a known value.
		/// </summary>
		[Test]
		public void SerializeProducesExpectedValue()
		{
			object o = 10;
			byte[] serialized = BinarySerializer.Serialize(o);

			byte[] expected =
			{	0, 1, 0, 0, 0, 255, 255, 255, 255, 1
				, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0
				, 12, 83, 121, 115, 116, 101, 109, 46
				, 73, 110, 116, 51, 50, 1, 0, 0, 0, 7
				, 109, 95, 118, 97, 108, 117, 101, 0
				, 8, 10, 0, 0, 0, 11
			};
			
			for(int i = 0; i < serialized.Length; i++)
			{
				Assert.AreEqual(expected[i], serialized[i], "Ok, someone changed the serialization format. Good to know.");
			}
		}
	}

	[Serializable]
	internal class SerializableObject
	{
		public int SomeNumber
		{
			get { return _someNumber; }
			set { _someNumber = value; }
		}

		int _someNumber;

		public string SomeString
		{
			get { return _someString; }
			set { _someString = value; }
		}

		string _someString;
	}
}
