using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Extensibility;
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
		[RollBack2]
		public void CanLoadImageFromReader()
		{
			UnitTestHelper.SetupBlog();

			StubResultSet resultSet = new StubResultSet("CategoryID", "File", "Height", "Width", "ImageID", "Active", "Title");
			resultSet.AddRow(987, "MyImage.png", 150, 200, 123, true, "My Image");
			StubDataReader reader = new StubDataReader(resultSet);

			reader.Read();
			Image image = DataHelper.LoadImage(reader);
			Assert.AreEqual(987, image.CategoryID);
			Assert.AreEqual("MyImage.png", image.FileName);
			Assert.AreEqual(150, image.Height);
			Assert.AreEqual(200, image.Width);
			Assert.AreEqual(123, image.ImageID);
			Assert.AreEqual(true, image.IsActive);
			Assert.AreEqual("My Image", image.Title);
		}
		
		[Test]
		public void CanLoadEntryDayCollectionFromReader()
		{
            DateTime dateAdded = DateTime.ParseExact("1/23/2006 4:00 PM", "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);

			StubResultSet resultSet = new StubResultSet("DateAdded", "PostType");
			resultSet.AddRow(dateAdded, PostType.BlogPost);
			resultSet.AddRow(dateAdded.AddHours(1), PostType.BlogPost);

			StubDataReader reader = new StubDataReader(resultSet);
			ICollection<EntryDay> entryCollection = DataHelper.LoadEntryDayCollection(reader, false);
			Assert.AreEqual(1, entryCollection.Count);
			
		}
		
		[Test]
		public void ReturnNullIfEmptyReturnsStringForNonEmpty()
		{
			Assert.AreEqual("test", DataHelper.ReturnNullIfEmpty("test"));
		}
		
		[Test]
		public void CheckNullStringReturnsStringForNonNullString()
		{
			Assert.AreEqual(string.Empty, DataHelper.CheckNullString(string.Empty), "Should return same string");
			Assert.AreEqual("test", DataHelper.CheckNullString("test"), "Should return same string");
		}
		
		[Test]
		public void CheckNullStringReturnsNullForDBNull()
		{
			Assert.IsNull(DataHelper.CheckNullString(DBNull.Value), "Should return null");
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
