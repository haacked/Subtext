using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework.Data
{
	/// <summary>
	/// Summary description for DataHelperTests.
	/// </summary>
	[TestFixture]
	public class DataHelperTests
	{
        public class ObjectWithProperties {
            public int IntProperty { get; set; }
            public int? NullableIntProperty { get; set; }
            public string StringProperty { get; set; }
            public bool ReadOnlyBoolean { get; private set; }
            public BlogInfo ComplexObject { get; set; }
            public DateTime DateProperty { get; set; }
        }

        [Test]
        public void Object_WithComplexProperty_DoesNotTryAndSetIt()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["ComplexObject"]).Throws(new IndexOutOfRangeException());
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(null, result.ComplexObject);
        }

        [Test]
        public void Object_WithReadOnlyProperty_DoesNotTryAndSetIt()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["ReadOnlyBoolean"]).Throws(new IndexOutOfRangeException());
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(false, result.ReadOnlyBoolean);
        }

        [Test]
        public void IDataReader_WithIntColumnHavingSameNameAsProperty_PopulatesObjectWithPropertySetCorrectly() { 
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["IntProperty"]).Returns(42);
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(42, result.IntProperty);
        }

        [Test]
        public void IDataReader_WithStringColumnHavingSameNameAsProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["StringProperty"]).Returns("Hello world");
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual("Hello world", result.StringProperty);
        }

        [Test]
        public void IDataReader_WithDateTimeColumnHavingSameNameAsDateTimeProperty_PopulatesObjectWithPropertySetCorrectly()
        {
            //arrange
            DateTime now = DateTime.Now;
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["DateProperty"]).Returns(now);
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(now, result.DateProperty);
        }

        [Test]
        public void IDataReader_WithNullColumn_DoesNotSetProperty()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["NullableIntProperty"]).Returns(DBNull.Value);
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual((int?)null, result.NullableIntProperty);
        }

        [Test]
        public void IDataReader_WithNullableIntColumnHavingSameNameAsProperty_PopulatesObjectWithNullablePropertySetCorrectly()
        {
            //arrange
            var reader = new Mock<IDataReader>();
            reader.Expect(r => r.Read()).Returns(true).AtMostOnce();
            reader.ExpectGet(r => r["NullableIntProperty"]).Returns(23);
            reader.Expect(r => r.Read()).Returns(false);

            //act
            var result = reader.Object.LoadObject<ObjectWithProperties>();

            //assert
            Assert.AreEqual(23, result.NullableIntProperty);
        }

		/// <summary>
		/// Makes sure that we parse the date correctly.
		/// </summary>
		[Test]
		public void LoadArchiveCountParsesDateCorrectly()
		{
			TestDataReader reader = new TestDataReader();
			reader.AddRecord(1, 2, 2005, 23);
			reader.AddRecord(1, 23, 2005, 23);
			
			ICollection<ArchiveCount> archive = DataHelper.LoadArchiveCount(reader);
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

                if (second == null)
                {
                    second = count;
                    continue;
                }
		    }
		    
			Assert.AreEqual(DateTime.ParseExact("01/02/2005", "MM/dd/yyyy", CultureInfo.InvariantCulture), first.Date, "Something happened to the date parsing.");
            Assert.AreEqual(DateTime.ParseExact("01/23/2005", "MM/dd/yyyy", CultureInfo.InvariantCulture), second.Date, "Something happened to the date parsing.");
		}

		#region Teast class that implements IDataReader
		internal struct DataReaderRecord
		{
			public DataReaderRecord(int month, int day, int year, int count)
			{
				Month = month;
				Day = day;
				Year = year;
				Count = count;
			}
			public int Month;
			public int Day;
			public int Year;
			public int Count;
		}

		internal class TestDataReader : IDataReader
		{
			int _currentIndex = -1;
			ArrayList _records = new ArrayList();
			
			internal TestDataReader()
			{}

			public void AddRecord(int month, int day, int year, int count)
			{
				_records.Add(new DataReaderRecord(month, day, year, count));
			}

			public void AddRecord(DataReaderRecord record)
			{
				_records.Add(record);
			}

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
						throw new InvalidOperationException("No records in this reader.");

					DataReaderRecord record = (DataReaderRecord)_records[_currentIndex];
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
		}
		#endregion
	}
}