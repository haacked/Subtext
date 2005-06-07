using System;
using System.EnterpriseServices;
using NUnit.Core;
using NUnit.Extensions.Royo;

namespace UnitTests.Subtext
{
	/// <summary>
	/// NUnit attribute that automatically rolls back database transactions 
	/// when a test is complete.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	[TestBuilder(typeof(CustomTestAttributeBase.CustomTestBuilder))]
	public class RollbackAttribute : CustomTestAttributeBase
	{
		/// <summary>
		/// Sets up the transaction context before the test.
		/// </summary>
		/// <param name="testResult">Test result.</param>
		/// <param name="testCase">Test case.</param>
		public override void BeforeTestRun(NUnit.Core.TestCaseResult testResult, NUnit.Core.TemplateTestCase testCase)
		{
			ServiceConfig config = new ServiceConfig();
			config.Transaction = TransactionOption.RequiresNew;
			ServiceDomain.Enter(config);
		}

		/// <summary>
		/// Rolls back the transaction context after the test.
		/// </summary>
		/// <param name="testResult">Test result.</param>
		/// <param name="testCase">Test case.</param>
		public override void AfterTestRun(NUnit.Core.TestCaseResult testResult, NUnit.Core.TemplateTestCase testCase)
		{
			if(ContextUtil.IsInTransaction)
			{
				//Abort the transaction.
				ContextUtil.SetAbort();
			}
			ServiceDomain.Leave();
		}
	}
}
