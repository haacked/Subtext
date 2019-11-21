using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
    /// <summary>
    /// Summary description for DataHelperTests.
    /// </summary>
    [TestClass]
    public class DataHelperTests
    {
        [TestMethod]
        public void ReadFeedbackItem_ReadsParentEntrySyndicated_AsUTC()
        {
            // arrange
            var reader = new Mock<IDataReader>();
            DateTime dateSyndicated = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            reader.SetupGet(r => r["Id"]).Returns(1);
            reader.SetupGet(r => r["Title"]).Returns("Test");
            reader.SetupGet(r => r["Body"]).Returns("Foo");
            reader.SetupGet(r => r["EntryId"]).Returns(99);
            reader.SetupGet(r => r["Author"]).Returns("Author");
            reader.SetupGet(r => r["IsBlogAuthor"]).Returns(false);
            reader.SetupGet(r => r["Email"]).Returns("Foo");
            reader.SetupGet(r => r["Url"]).Returns("http://subtextproject.com/");
            reader.SetupGet(r => r["FeedbackType"]).Returns(1);
            reader.SetupGet(r => r["StatusFlag"]).Returns(1);
            reader.SetupGet(r => r["CommentAPI"]).Returns(false);
            reader.SetupGet(r => r["Referrer"]).Returns("Foo");
            reader.SetupGet(r => r["IpAddress"]).Returns("127.0.0.1");
            reader.SetupGet(r => r["UserAgent"]).Returns("Foo");
            reader.SetupGet(r => r["FeedbackChecksumHash"]).Returns("Foo");
            reader.SetupGet(r => r["DateCreatedUtc"]).Returns(dateSyndicated.AddDays(-2));
            reader.SetupGet(r => r["DateModifiedUtc"]).Returns(dateSyndicated.AddHours(-1));
            reader.SetupGet(r => r["ParentEntryName"]).Returns("FooBarParent");
            reader.SetupGet(r => r["ParentEntryCreateDateUtc"]).Returns(dateSyndicated.AddDays(-5));
            reader.SetupGet(r => r["ParentEntryDatePublishedUtc"]).Returns(dateSyndicated.AddDays(-4));

            // act
            var feedback = reader.Object.ReadFeedbackItem();

            // assert
            Assert.AreEqual(dateSyndicated.AddDays(-4), feedback.ParentDatePublishedUtc);
            Assert.AreEqual(DateTimeKind.Utc, feedback.DateCreatedUtc.Kind);
            Assert.AreEqual(DateTimeKind.Utc, feedback.DateModifiedUtc.Kind);
            Assert.AreEqual(DateTimeKind.Utc, feedback.ParentDateCreatedUtc.Kind);
            Assert.AreEqual(DateTimeKind.Utc, feedback.ParentDatePublishedUtc.Kind);
        }

        [TestMethod]
        public void ReadValue_WithValueMatchingType_ReturnsValueAsType()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(98008);

            //act
            var result = reader.Object.ReadValue<int>("column");

            //assert
            Assert.AreEqual(98008, result);
        }

        [TestMethod]
        public void ReadValue_WithValueReturningDbNull_ReturnsDefaultValue()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(DBNull.Value);

            //act
            var result = reader.Object.ReadValue("column", 8675309);

            //assert
            Assert.AreEqual(8675309, result);
        }

        [TestMethod]
        public void ReadValue_WithValueReturningNull_ReturnsDefaultValue()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(null);

            //act
            var result = reader.Object.ReadValue("column", 8675309);

            //assert
            Assert.AreEqual(8675309, result);
        }

        [TestMethod]
        public void ReadValue_WithValueFuncThrowingFormatException_ReturnsDefaultValue()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(null);

            //act
            var result = reader.Object.ReadValue("column", value => { throw new FormatException(); }, 8675309);

            //assert
            Assert.AreEqual(8675309, result);
        }

        [TestMethod]
        public void ReadValue_WithValueFuncThrowingIndexOutOfRangeException_ReturnsDefaultValue()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(null);

            //act
            var result = reader.Object.ReadValue("column", value => { throw new IndexOutOfRangeException(); }, 8675309);

            //assert
            Assert.AreEqual(8675309, result);
        }

        [TestMethod]
        public void AsEnumerable_WithMultipleRows_ReturnsEnumerationOfRows()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(new Queue<bool>(new[] { true, true, false }).Dequeue);
            reader.SetupGet(r => r["column"]).Returns(new Queue<object>(new object[] { 123, 456 }).Dequeue);

            //act
            var result = reader.Object.ReadEnumerable(r => r.ReadValue<Int32>("column")).ToList();

            //assert
            Assert.AreEqual(123, result[0]);
            Assert.AreEqual(456, result[1]);
        }

        [TestMethod]
        public void AsPagedCollection_WithMultipleRows_ReturnsPagedCollection()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(new Queue<bool>(new[] { true, true, false, true }).Dequeue);
            reader.SetupGet(r => r["column"]).Returns(new Queue<object>(new object[] { 123, 456 }).Dequeue);
            reader.SetupGet(r => r["TotalRecords"]).Returns(2);
            reader.Setup(r => r.NextResult()).Returns(true);

            //act
            var result = reader.Object.ReadPagedCollection(r => r.ReadValue<int>("column"));

            //assert
            Assert.AreEqual(123, result[0]);
            Assert.AreEqual(456, result[1]);
            Assert.AreEqual(2, result.MaxItems);
        }

        [TestMethod]
        public void ReadObject_WithUriProperty_TriesAndParsesUrlAndSetsIt()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["Url"]).Returns("http://subtextproject.com/");
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual("http://subtextproject.com/", result.Url.ToString());
        }

        [TestMethod]
        public void ReadObject_WithComplexProperty_DoesNotTryAndSetIt()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["ComplexObject"]).Throws(new IndexOutOfRangeException());
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(null, result.ComplexObject);
        }

        [TestMethod]
        public void ReadObject_WithReadOnlyProperty_DoesNotTryAndSetIt()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["ReadOnlyBoolean"]).Throws(new IndexOutOfRangeException());
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(false, result.ReadOnlyBoolean);
        }

        [TestMethod]
        public void IDataReader_WithIntColumnHavingSameNameAsProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["IntProperty"]).Returns(42);
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(42, result.IntProperty);
        }

        [TestMethod]
        public void IDataReader_WithStringColumnHavingSameNameAsProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["StringProperty"]).Returns("Hello world");
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual("Hello world", result.StringProperty);
        }

        [TestMethod]
        public void IDataReader_WithDateTimeColumnHavingSameNameAsDateTimeProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            DateTime now = DateTime.UtcNow;
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["DateProperty"]).Returns(now);
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(now, result.DateProperty);
        }

        [TestMethod]
        public void IDataReader_WithNullColumn_DoesNotSetProperty()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["NullableIntProperty"]).Returns(DBNull.Value);
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(null, result.NullableIntProperty);
        }

        [TestMethod]
        public void IDataReader_WithNullableIntColumnHavingSameNameAsProperty_PopulatesObjectWithNullablePropertySetCorrectly()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["NullableIntProperty"]).Returns(23);
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(23, result.NullableIntProperty);
        }

        /// <summary>
        /// Makes sure that we parse the date correctly.
        /// </summary>
        [TestMethod]
        public void LoadArchiveCountParsesDateCorrectly()
        {
            // Arrange
            var reader = new TestDataReader();
            reader.AddRecord(1, 2, 2005, 23);
            reader.AddRecord(1, 23, 2005, 23);

            // Act
            ICollection<ArchiveCount> archive = DataHelper.ReadArchiveCount(reader);

            // Assert
            Assert.AreEqual(2, archive.Count, "Should only have two records.");

            ArchiveCount first = null;
            ArchiveCount second = null;

            foreach (ArchiveCount count in archive)
            {
                if (first == null)
                {
                    first = count;
                    continue;
                }

                if (second == null)
                {
                    second = count;
                    continue;
                }
            }

            Assert.AreEqual(DateTime.ParseExact("01/02/2005", "MM/dd/yyyy", CultureInfo.InvariantCulture), first.Date,
                            "Something happened to the date parsing.");
            Assert.AreEqual(DateTime.ParseExact("01/23/2005", "MM/dd/yyyy", CultureInfo.InvariantCulture), second.Date,
                            "Something happened to the date parsing.");
        }

        #region Test class that implements IDataReader

        #region Nested type: DataReaderRecord

        internal struct DataReaderRecord
        {
            public int Count;
            public int Day;
            public int Month;
            public int Year;

            public DataReaderRecord(int month, int day, int year, int count)
            {
                Month = month;
                Day = day;
                Year = year;
                Count = count;
            }
        }

        #endregion

        #region Nested type: TestDataReader

        internal class TestDataReader : IDataReader
        {
            readonly ArrayList _records = new ArrayList();
            int _currentIndex = -1;

            #region IDataReader Members

            public string GetName(int i)
            {
                throw new NotImplementedException();
            }

            public string GetDataTypeName(int i)
            {
                throw new NotImplementedException();
            }

            public Type GetFieldType(int i)
            {
                throw new NotImplementedException();
            }

            public object GetValue(int i)
            {
                throw new NotImplementedException();
            }

            public int GetValues(object[] values)
            {
                throw new NotImplementedException();
            }

            public int GetOrdinal(string name)
            {
                throw new NotImplementedException();
            }

            public bool GetBoolean(int i)
            {
                throw new NotImplementedException();
            }

            public byte GetByte(int i)
            {
                throw new NotImplementedException();
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public char GetChar(int i)
            {
                throw new NotImplementedException();
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public Guid GetGuid(int i)
            {
                throw new NotImplementedException();
            }

            public short GetInt16(int i)
            {
                throw new NotImplementedException();
            }

            public int GetInt32(int i)
            {
                throw new NotImplementedException();
            }

            public long GetInt64(int i)
            {
                throw new NotImplementedException();
            }

            public float GetFloat(int i)
            {
                throw new NotImplementedException();
            }

            public double GetDouble(int i)
            {
                throw new NotImplementedException();
            }

            public string GetString(int i)
            {
                throw new NotImplementedException();
            }

            public decimal GetDecimal(int i)
            {
                throw new NotImplementedException();
            }

            public DateTime GetDateTime(int i)
            {
                throw new NotImplementedException();
            }

            public IDataReader GetData(int i)
            {
                throw new NotImplementedException();
            }

            public bool IsDBNull(int i)
            {
                throw new NotImplementedException();
            }

            public int FieldCount
            {
                get { throw new NotImplementedException(); }
            }

            public object this[int i]
            {
                get { throw new NotImplementedException(); }
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public bool NextResult()
            {
                throw new NotImplementedException();
            }

            public bool Read()
            {
                return ++_currentIndex < _records.Count;
            }

            public DataTable GetSchemaTable()
            {
                throw new NotImplementedException();
            }

            public int Depth
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsClosed
            {
                get { throw new NotImplementedException(); }
            }

            public int RecordsAffected
            {
                get { throw new NotImplementedException(); }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public object this[string name]
            {
                get
                {
                    if (_records.Count == 0)
                    {
                        throw new InvalidOperationException("No records in this reader.");
                    }

                    var record = (DataReaderRecord)_records[_currentIndex];
                    switch (name)
                    {
                        case "Month":
                            return record.Month;
                        case "Day":
                            return record.Day;
                        case "Year":
                            return record.Year;
                        case "Count":
                            return record.Count;
                    }
                    throw new InvalidOperationException("Unexpected column '" + name + "'");
                }
            }

            #endregion

            public void AddRecord(int month, int day, int year, int count)
            {
                _records.Add(new DataReaderRecord(month, day, year, count));
            }

            public void AddRecord(DataReaderRecord record)
            {
                _records.Add(record);
            }
        }

        #endregion

        #endregion

        #region Nested type: ObjectWithProperties

        public class ObjectWithProperties
        {
            public int IntProperty { get; set; }
            public int? NullableIntProperty { get; set; }
            public string StringProperty { get; set; }
            public bool ReadOnlyBoolean { get; private set; }
            public Blog ComplexObject { get; set; }
            public DateTime DateProperty { get; set; }
            public Uri Url { get; set; }
        }

        #endregion
    }
}