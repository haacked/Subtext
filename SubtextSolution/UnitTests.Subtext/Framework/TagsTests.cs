using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class TagsTests
	{
[RowTest]
[Row("BASIC", "GFXNH")]
[Row("ZEBRAHORSE", @"dOL\KRY\]O")]
public void Test(string input, string expectedOutput)
{
	Assert.AreEqual(expectedOutput, prcEncryptPassword(input));
}

static string prcEncryptPassword(string clearTextPassword)
{
	if (clearTextPassword == null)
		throw new ArgumentNullException("clearTextPassword", "Must specify a clear text password to encrypt.");

	//This is a conversion of the password encryption from BOSS
	clearTextPassword = clearTextPassword.Trim();

	int index = 0;
	string[] encrypted = new string[clearTextPassword.Length];
		 
	foreach (char character in clearTextPassword)
	{
		// Change to new encrypted char 
		char encryptedCharacter = (char)(character + clearTextPassword.Length);
		encrypted[index++] = encryptedCharacter.ToString(CultureInfo.InvariantCulture);
	}
	return String.Join(string.Empty, encrypted);
}


		[RowTest]
		[Row(-1, 1, 1)]
		[Row(0, 1, 2)]
		[Row(.25, 1, 3)]
		[Row(.49, 1, 4)]
		[Row(.9, 1, 5)]
		[Row(1.9, 1, 6)]
		public void CanComputeWeight(double factor, double stdDev, int expected)
		{
			Assert.AreEqual(expected, Tags.ComputeWeight(factor, stdDev));
		}
	}
}
