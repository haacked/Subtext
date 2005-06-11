using System;
using System.EnterpriseServices;
using NUnit.Core;
using NUnit.Extensions.Royo;

namespace UnitTests
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
		/// Creates a new <see cref="RollbackAttribute"/> instance.
		/// </summary>
		public RollbackAttribute() : base()
		{}

		/// <summary>
		/// Creates a new <see cref="RollbackAttribute"/> instance.
		/// </summary>
		/// <param name="expectedExceptionType">Expected exception type.</param>
		public RollbackAttribute(Type expectedExceptionType) : this(expectedExceptionType, null)
		{
		}

		/// <summary>
		/// Creates a new <see cref="RollbackAttribute"/> instance.
		/// </summary>
		/// <param name="expectedExceptionType">Expected exception type.</param>
		/// <param name="expectedMessage">The expected exception message</param>
		public RollbackAttribute(Type expectedExceptionType, string expectedMessage) : base()
		{
			this._expectedException = expectedExceptionType;
			this._expectedMessage = expectedMessage;
		}

		/// <summary>
		/// Gets or sets the expected exception type.
		/// </summary>
		/// <value></value>
		public Type ExceptionType
		{
			get { return _expectedException; }
			set { _expectedException = value; }
		}

		Type _expectedException = null;

		/// <summary>
		/// Gets or sets the expected message of the expected exception.
		/// </summary>
		/// <value></value>
		public string ExpectedMessage
		{
			get { return _expectedMessage; }
			set { _expectedMessage = value; }
		}

		string _expectedMessage = null;

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
