using System;
using MbUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
	/// <summary>
	/// Unit tests for the <see cref="Keywords"/> class.
	/// </summary>
	[TestFixture]
	public class KeywordsTests
	{
		[RowTest]
		[Row("This is a test", "is", "is not", "This is not a test")]
		[Row("This is a test", "This", "XXX", "XXX is a test")]
		[Row("This is a test.", "this", "XXX", "XXX is a test.")]
		[Row("This <b>is</b> a test.", "is", "is not", "This <b>is not</b> a test.")]
		[Row("This is a test.", "test", "farce", "This is a farce.")]
		[Row("This is a_test.", "test", "farce", "This is a_test.")]
		[Row("This is a test", "st", "XXX", "This is a test")]
		public void KeywordReplaceTests(string source, string oldValue, string newValue, string expected)
		{
			//TODO:??? [Row("This is'nt a test.", "is", "farce", "This is'nt a test.")]
			//TODO:??? [Row("This is&#8217;nt a test.", "is", "farce", "This is&#8217;nt a test.")]
			//Note keyword replace are case sensitive.
			Assert.AreEqual(expected, Keywords.Replace(source, oldValue, newValue));
		}
	}
}
