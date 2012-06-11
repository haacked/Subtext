using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MbUnit.Framework;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void ForEach_WithCollection_IteratesEachItem()
        {
            // arrange
            IEnumerable<int> numbers = new[]{5, 4, 3};
            string concatenated = string.Empty;

            // act
            numbers.ForEach(number => concatenated += number);

            // assert
            Assert.AreEqual("543", concatenated);
        }

        [Test]
        public void AddRange_WithCollection_AddsAllItemsToCollection()
        {
            // arrange
            IEnumerable<int> numbers = new[] { 5, 4, 3 };
            ICollection<int> items = new List<int>();

            // act
            items.AddRange(numbers);

            // assert
            Assert.AreEqual(3, numbers.Count());
        }

        [Test]
        public void GetBoolean_WithNameValueCollectionHavingCorrespondingValue_ReturnsBoolean()
        {
            // arrange
            var collection = new NameValueCollection {{"Key", "true"}};

            // act
            var result = collection.GetBoolean("Key");

            // assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetBoolean_WithNullValueForKey_ReturnsFalse()
        {
            // arrange
            var collection = new NameValueCollection {{"Key", null}};

            // act
            var result = collection.GetBoolean("Key");

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetBoolean_WithNoValueForKey_ReturnsFalse()
        {
            // arrange
            var collection = new NameValueCollection();

            // act
            var result = collection.GetBoolean("Key");

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetBoolean_WithInvalidValueForKey_ReturnsFalse()
        {
            // arrange
            var collection = new NameValueCollection {{"Key", "blah"}};

            // act
            var result = collection.GetBoolean("Key");

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetEnum_WithEnumValue_IgnoresCase()
        {
            // arrange
            var collection = new NameValueCollection { { "Key", "foo" } };

            // act
            var result = collection.GetEnum<TestEnum>("Key");

            // assert
            Assert.AreEqual(TestEnum.Foo, result);
        }

        public enum TestEnum
        {
            Foo,
            Bar,
        }

        [Test]
        public void Accumulate_WithTwoEnumerations_AccumulatesOneIntoTheOther()
        {
            // arrange
            var containers = new[] { new KeyValuePair<int, List<string>>(0, new List<string>()), new KeyValuePair<int, List<string>>(1, new List<string>()), new KeyValuePair<int, List<string>>(2, new List<string>()), new KeyValuePair<int, List<string>>(3, new List<string>()) };
            var items = new[] {new KeyValuePair<int, string>(1, "A"), new KeyValuePair<int, string>(1, "B"), new KeyValuePair<int, string>(3, "C")};

            // act
            containers.Accumulate(items, container => container.Key, item => item.Key, (container, item) => container.Value.Add(item.Value));

            // assert
            Assert.AreEqual(0, containers.First().Value.Count);
            var secondContainer = containers.ElementAt(1);
            Assert.AreEqual(2, secondContainer.Value.Count);
            Assert.AreEqual("A", secondContainer.Value[0]);
            Assert.AreEqual("B", secondContainer.Value[1]);
            Assert.AreEqual(0, containers.ElementAt(2).Value.Count);
            var fourthContainer = containers.ElementAt(3);
            Assert.AreEqual(1, fourthContainer.Value.Count);
            Assert.AreEqual("C", fourthContainer.Value[0]);
        }
    }
}
