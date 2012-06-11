using System;
using MbUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
    [TestFixture]
    public class SerializationHelperTests
    {
        [Test]
        public void CanSerializeAndDeserializeToAndFromBase64()
        {
            TestStruct test;
            test.Foo = 42;
            test.Bar = "Test";

            string serialized = SerializationHelper.SerializeToBase64String(test);
            Assert.IsNotNull(serialized);
            Assert.IsTrue(serialized.Length > 0);

            var deserialized = SerializationHelper.DeserializeFromBase64String<TestStruct>(serialized);
            Assert.AreEqual(42, deserialized.Foo, "Deserialization failed.");
            Assert.AreEqual("Test", deserialized.Bar, "Deserialization failed.");
        }

        #region Nested type: TestStruct

        [Serializable]
        struct TestStruct
        {
            public string Bar;
            public int Foo;
        }

        #endregion
    }
}