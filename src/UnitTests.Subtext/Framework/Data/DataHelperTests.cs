using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
    /// <summary>
    /// Summary description for DataHelperTests.
    /// </summary>
    [TestFixture]
    public class DataHelperTests
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void ReadValue_WithValueFuncThrowingFormatException_ReturnsDefaultValue()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r["column"]).Returns(null);

            //act
            var result = reader.Object.ReadValue("column", value => {throw new FormatException();}, 8675309);

            //assert
            Assert.AreEqual(8675309, result);
        }

        [Test]
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

        [Test]
        public void AsEnumerable_WithMultipleRows_ReturnsEnumerationOfRows()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(new Queue<bool>(new[] { true, true, false }).Dequeue);
            reader.SetupGet(r => r["column"]).Returns(new Queue<object>(new object[] {123, 456}).Dequeue);

            //act
            var result = reader.Object.ReadEnumerable(r => r.ReadValue<Int32>("column")).ToList();

            //assert
            Assert.AreEqual(123, result[0]);
            Assert.AreEqual(456, result[1]);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void
            IDataReader_WithDateTimeColumnHavingSameNameAsDateTimeProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            DateTime now = DateTime.Now;
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(true).AtMostOnce();
            reader.SetupGet(r => r["DateProperty"]).Returns(now);
            reader.Setup(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.ReadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(now, result.DateProperty);
        }

        [Test]
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

        [Test]
        public void
            IDataReader_WithNullableIntColumnHavingSameNameAsProperty_PopulatesObjectWithNullablePropertySetCorrectly()
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
        [Test]
        public void LoadArchiveCountParsesDateCorrectly()
        {
            var reader = new TestDataReader();
            reader.AddRecord(1, 2, 2005, 23);
            reader.AddRecord(1, 23, 2005, 23);

            ICollection<ArchiveCount> archive = DataHelper.ReadArchiveCount(reader);
            Assert.AreEqual(2, archive.Count, "Should only have two records.");

            ArchiveCount first = null;
            ArchiveCount second = null;

            foreach(ArchiveCount count in archive)
            {
                if(first == null)
                {
                    first = count;
                    continue;
                }

                if(second == null)
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

        #region Teast class that implements IDataReader

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
                    if(_records.Count == 0)
                    {
                        throw new InvalidOperationException("No records in this reader.");
                    }

                    var record = (DataReaderRecord)_records[_currentIndex];
                    switch(name)
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