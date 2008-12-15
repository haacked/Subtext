using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MbUnit.Framework;

namespace UnitTests.Subtext
{
	[TestFixture]
	public class SelfTests
	{
		[Test]
		public void SingleResultStubDataReaderReturnsCorrectValues()
		{
			DateTime testDate = DateTime.Now;
			StubResultSet resultSet = new StubResultSet("col0", "col1", "col2");
			resultSet.AddRow(1, "Test", testDate);
			resultSet.AddRow(2, "Test2", testDate.AddDays(1));
						
			StubDataReader reader = new StubDataReader(resultSet);

			//Advance to first row.
			Assert.IsTrue(reader.Read(), "Expected data.");
			
			Assert.AreEqual(1, reader["col0"], "Misread row 0, col 0.");
			Assert.AreEqual("Test", reader["col1"], "Misread row 0, col 1.");
			Assert.AreEqual(testDate, reader["col2"], "Misread row 0, col 3.");
			Assert.AreEqual(1, reader.GetInt32(0), "Misread row 0, col 0.");
			Assert.AreEqual("Test", reader.GetString(1), "Misread row 0, col 1.");
			Assert.AreEqual(testDate, reader.GetDateTime(2), "Misread row 0, col 3.");
			Assert.AreEqual(1, reader[0], "Misread row 0, col 0.");
			Assert.AreEqual("Test", reader[1], "Misread row 0, col 1.");
			Assert.AreEqual(testDate, reader[2], "Misread row 0, col 3.");
			
			//Advance to second row.
			Assert.IsTrue(reader.Read(), "Expected data.");
			
			Assert.AreEqual(2, reader.GetInt32(0), "Misread row 1, col 0.");
			Assert.AreEqual("Test2", reader.GetString(1), "Misread row 1, col 1.");
			Assert.AreEqual(testDate.AddDays(1), reader.GetDateTime(2), "Misread row 1, col 3.");
			Assert.AreEqual(2, reader[0], "Misread row 1, col 0.");
			Assert.AreEqual("Test2", reader[1], "Misread row 1, col 1.");
			Assert.AreEqual(testDate.AddDays(1), reader[2], "Misread row 1, col 3.");
			Assert.AreEqual(2, reader["col0"], "Misread row 1, col 0.");
			Assert.AreEqual("Test2", reader["col1"], "Misread row 1, col 1.");
			Assert.AreEqual(testDate.AddDays(1), reader["col2"], "Misread row 1, col 3.");
		}
		
		[Test]
		public void MultipleResultStubDataReaderReturnsCorrectValues()
		{
			DateTime testDate = DateTime.Now;
			StubResultSet resultSet = new StubResultSet("col0", "col1", "col2");
			resultSet.AddRow(1, "Test", testDate);

			StubResultSet anotherResultSet = new StubResultSet("first", "second");
			anotherResultSet.AddRow((decimal)1.618, "Foo");
			anotherResultSet.AddRow((decimal)2.718, "Bar");
			anotherResultSet.AddRow((decimal)3.142, "Baz");

			StubDataReader reader = new StubDataReader(resultSet, anotherResultSet);

			//Advance to first row.
			Assert.IsTrue(reader.Read(), "Expected data.");

			Assert.AreEqual(1, reader["col0"]);

			//Advance to second ResultSet.
			Assert.IsTrue(reader.NextResult(), "Expected next result set");

			//Advance to first row.
			Assert.IsTrue(reader.Read(), "Expected data.");
			Assert.AreEqual((decimal)1.618, reader["first"]);
			Assert.AreEqual("Foo", reader["second"]);
			Assert.IsTrue(reader.Read(), "Expected data.");
			Assert.AreEqual((decimal)2.718, reader["first"]);
			Assert.AreEqual("Bar", reader["second"]);
			Assert.IsTrue(reader.Read(), "Expected data.");
			Assert.AreEqual((decimal)3.142, reader["first"]);
			Assert.AreEqual("Baz", reader["second"]);
		}
	}
	
	/// <summary>
	/// This class fakes up a data reader.
	/// </summary>
	public class StubDataReader : IDataReader
	{
		ICollection<StubResultSet> stubResultSets;
		private int currentResultsetIndex = 0;
	
		/// <summary>
		/// Initializes a new instance of the <see cref="StubDataReader"/> class. 
		/// Each row in the arraylist is a result set.
		/// </summary>
		/// <param name="stubResultSets">The result sets.</param>
		public StubDataReader(ICollection<StubResultSet> stubResultSets)
		{
			this.stubResultSets = stubResultSets;
		}
	
		/// <summary>
		/// Initializes a new instance of the <see cref="StubDataReader"/> class. 
		/// Each row in the arraylist is a result set.
		/// </summary>
		/// <param name="resultSets">The result sets to add.</param>
		public StubDataReader(params StubResultSet[] resultSets)
		{
			this.stubResultSets = new List<StubResultSet>();
			foreach(StubResultSet resultSet in resultSets)
			{
				this.stubResultSets.Add(resultSet);
			}
		}

		public void Close()
		{
		}

		public bool NextResult()
		{
			if(currentResultsetIndex >= this.stubResultSets.Count)
				return false;
			
			return (++currentResultsetIndex < this.stubResultSets.Count);
		}

		public bool Read()
		{
			return CurrentResultSet.Read();
		}

		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a value indicating the depth of nesting for the current row.
		/// </summary>
		/// <value></value>
		public int Depth
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsClosed
		{
			get { return false; }
		}

		public int RecordsAffected
		{
			get { return 1; }
		}

		public void Dispose()
		{
		}

		public string GetName(int i)
		{
			return CurrentResultSet.GetFieldNames()[i];
		}

		public string GetDataTypeName(int i)
		{
			return this.CurrentResultSet.GetFieldNames()[i];
		}

		public Type GetFieldType(int i)
		{
			//KLUDGE: Since we're dynamically creating this, I'll have to 
			//		  look at the actual data to determine this.
			//		  We'll loook at the first row since it's the most likely 
			//			to have data.
			return this.stubResultSets.First()[i].GetType();
		}

		public object GetValue(int i)
		{
			return CurrentResultSet[i];
		}

		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name)
		{
			return this.CurrentResultSet.GetIndexFromFieldName(name);
		}

		public bool GetBoolean(int i)
		{
			return (bool)CurrentResultSet[i];
		}

		public byte GetByte(int i)
		{
			return (byte)CurrentResultSet[i];
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			//TODO: Need to test this method.
			
			byte[] totalBytes = (byte[])CurrentResultSet[i];

			int bytesRead = 0;
			for(int j = 0; j < length; j++)
			{
				long readIndex = fieldOffset + j;
				long writeIndex = bufferoffset + j;
				if(totalBytes.Length <= readIndex)
					throw new ArgumentOutOfRangeException("fieldOffset", string.Format("Trying to read index '{0}' is out of range. (fieldOffset '{1}' + current position '{2}'", readIndex, fieldOffset, j));
				
				if(buffer.Length <= writeIndex)
					throw new ArgumentOutOfRangeException("bufferoffset", string.Format("Trying to write to buffer index '{0}' is out of range. (bufferoffset '{1}' + current position '{2}'", readIndex, bufferoffset, j));
				
				buffer[writeIndex] = totalBytes[readIndex];
				bytesRead++;
			}
			return bytesRead;
		}

		public char GetChar(int i)
		{
			return (char)CurrentResultSet[i];
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			return (Guid)CurrentResultSet[i];
		}

		public short GetInt16(int i)
		{
			return (short)CurrentResultSet[i];
		}

		public int GetInt32(int i)
		{
			return (int)CurrentResultSet[i];
		}

		public long GetInt64(int i)
		{
			return (long)CurrentResultSet[i];
		}

		public float GetFloat(int i)
		{
			return (float)CurrentResultSet[i];
		}

		public double GetDouble(int i)
		{
			return (double)CurrentResultSet[i];
		}

		public string GetString(int i)
		{
			return (string)CurrentResultSet[i];
		}

		public decimal GetDecimal(int i)
		{
			return (decimal)CurrentResultSet[i];
		}

		public DateTime GetDateTime(int i)
		{
			return (DateTime)CurrentResultSet[i];
		}

		public IDataReader GetData(int i)
		{
			StubDataReader reader = new StubDataReader(this.stubResultSets);
			reader.currentResultsetIndex = i;
			return reader;
		}

		public bool IsDBNull(int i)
		{
			//TODO: Deal with value types.
			return null == CurrentResultSet[i];
		}

		public int FieldCount
		{
			get
			{
				return CurrentResultSet.GetFieldNames().Length;
			}
		}

		public object this[int i]
		{
			get
			{
				return CurrentResultSet[i];
			}
		}

		public object this[string name]
		{
			get
			{
				return CurrentResultSet[name];
			}
		}
		
		private StubResultSet CurrentResultSet
		{
			get
			{
				return this.stubResultSets.ElementAt(currentResultsetIndex);
			}
		}
	}

	/// <summary>
	/// Represents a result set for the StubDataReader.
	/// </summary>
	public class StubResultSet
	{
		int currentRowIndex = -1;
        List<StubRow> rows = new List<StubRow>();
        Dictionary<string, int> fieldNames = new Dictionary<string, int>();

		/// <summary>
		/// Initializes a new instance of the <see cref="StubResultSet"/> class with the column names.
		/// </summary>
		/// <param name="fieldNames">The column names.</param>
		public StubResultSet(params string[] fieldNames)
		{
			for(int i = 0; i < fieldNames.Length; i++)
			{
				this.fieldNames.Add(fieldNames[i], i);
			}
		}
		
		public string[] GetFieldNames()
		{
            string[] keys = new string[fieldNames.Keys.Count];
            fieldNames.Keys.CopyTo(keys, 0);
            return keys;
		}
		
		public string GetFieldName(int i)
		{
			return GetFieldNames()[i];
		}
		
		/// <summary>
		/// Adds the row.
		/// </summary>
		/// <param name="values">The values.</param>
		public void AddRow(params object[] values)
		{
			if(values.Length != fieldNames.Count )
			{
				throw new ArgumentOutOfRangeException("values", string.Format("The Row must contain '{0}' items", fieldNames.Count));
			}
			rows.Add(new StubRow(values));
		}
		
		public int GetIndexFromFieldName(string name)
		{
			if (!this.fieldNames.ContainsKey(name))
				throw new IndexOutOfRangeException(string.Format("The key '{0}' was not found in this data reader.", name));
			return this.fieldNames[name];
		}
		
		public bool Read()
		{
			return ++this.currentRowIndex < this.rows.Count;
		}
		
		public StubRow CurrentRow
		{
			get 
			{
				return this.rows[this.currentRowIndex];
			}
		}
		
		public object this[string key]
		{
			get
			{
				return CurrentRow[GetIndexFromFieldName(key)];
			}
		}
		
		public object this[int i]
		{
			get
			{
				return CurrentRow[i];
			}
		}
	}
	
	public class StubRow
	{
		object[] rowValues;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="StubRow"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		public StubRow(params object[] values)
		{
			this.rowValues = values;
		}
		
		/// <summary>
		/// Gets the <see cref="Object"/> with the specified i.
		/// </summary>
		/// <value></value>
		public object this[int i]
		{
			get
			{
				return rowValues[i];
			}
		}
	}
}
