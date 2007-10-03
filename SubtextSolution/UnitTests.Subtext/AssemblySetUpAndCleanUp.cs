using System;
using System.Configuration;
using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Installation;
using UnitTests.Subtext;

[assembly: AssemblyCleanup(typeof(AssemblySetUpAndCleanUp))]
namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
		public static void SetUp()
		{
		}
		
		[TearDown]
		public static void TearDown()
		{
		}
	}
}
