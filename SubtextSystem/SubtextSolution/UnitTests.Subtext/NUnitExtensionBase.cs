using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using UnitTests;
using TestCase = NUnit.Core.TestCase;

// Code generously made available by Roy Osherove and modified for Subtext use.
// http://weblogs.asp.net/rosherove/articles/ExntendingNunit221.aspx
namespace NUnit.Extensions.Royo
{
	public interface ICustomTestCaseEventsListener
	{
		void BeforeTestRun(TestCaseResult testResult,TemplateTestCase testCase);
		void AfterTestRun(TestCaseResult testResult,TemplateTestCase testCase);
	}

	/// <summary>
	/// Abstract base class for custom NUnit attributes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	[TestBuilder(typeof(CustomTestAttributeBase.CustomTestBuilder))]
	public abstract class CustomTestAttributeBase : Attribute, ICustomTestCaseEventsListener
	{
		public class CustomTestBuilder : ITestBuilder
		{
			public TestCase Make(Type fixtureType, MethodInfo method, object attribute)
			{
				Console.WriteLine("Creating custom fixture...");
				return new CustomTestCase(fixtureType, method, attribute as CustomTestAttributeBase);
			}
		}

		/// <summary>
		/// Method called before a test runs.
		/// </summary>
		/// <param name="testResult">Test result.</param>
		/// <param name="testCase">Test case.</param>
		public abstract void BeforeTestRun(TestCaseResult testResult, TemplateTestCase testCase);
		
		/// <summary>
		/// Method called after a test run.
		/// </summary>
		/// <param name="testResult">Test result.</param>
		/// <param name="testCase">Test case.</param>
		public abstract void AfterTestRun(TestCaseResult testResult, TemplateTestCase testCase);
	}

	/// <summary>
	/// Used to build custom exceptions. I've added the logic for 
	/// ExpectedException test cases because using a custom attribute 
	/// kills all others.
	/// </summary>
	public class CustomTestCase : TemplateTestCase
	{
		private CustomTestAttributeBase customTestImplementor = null;
		private Type expectedException = null;
		private string expectedMessage = null;

		public override void doRun(TestCaseResult testResult)
		{
			Console.WriteLine("calling preprocess....");
			if (customTestImplementor!=null)
			{
				customTestImplementor.BeforeTestRun(testResult,this);
			}

			base.doRun (testResult);
			
			if (customTestImplementor!=null)
			{
				customTestImplementor.AfterTestRun(testResult,this);
			}
		}

		/// <summary>
		/// Creates a new <see cref="CustomTestCase"/> instance.
		/// </summary>
		/// <param name="fixtureType">Fixture type.</param>
		/// <param name="method">Method.</param>
		/// <param name="customTestImplementor">Custom test implementor.</param>
		public CustomTestCase(Type fixtureType, MethodInfo method, CustomTestAttributeBase customTestImplementor) : base (fixtureType, method)
		{
			this.customTestImplementor = customTestImplementor;
			Initialize(method);
		}

		protected override void ProcessNoException (TestCaseResult testResult)
		{
			if(this.expectedException == null)
			{
				testResult.Success();
			}
			else
			{
				testResult.Failure(expectedException.Name + " was expected", null);
			}
		}

		protected override void ProcessException (Exception exception, TestCaseResult testResult)
		{
			if(this.expectedException == null)
			{
				RecordException(exception, testResult);
			}
			else
			{
				ProcessExpectedException(exception, testResult);
			}
		}

		private void Initialize( MethodInfo method )
		{
			Attribute attribute = GetAttribute(method, typeof(RollbackAttribute), false);
			if(attribute == null)
				attribute = GetAttribute(method, typeof(ExpectedExceptionAttribute), false);

			if ( attribute == null )
				return;

			this.expectedException = (System.Type)Reflect.GetPropertyValue(
				attribute, 
				"ExceptionType", 
				BindingFlags.Public | BindingFlags.Instance );
			this.expectedMessage = (string)Reflect.GetPropertyValue( 
				attribute, 
				"ExpectedMessage", 
				BindingFlags.Public | BindingFlags.Instance );
		}

		static Attribute GetAttribute(MethodInfo member, Type attributeType, bool inherit)
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType() == attributeType)
					return attribute;
			return null;
		}

		/// <summary>
		/// Examine a type and get a property with a particular name.
		/// In the case of overloads, the first one found is returned.
		/// </summary>
		/// <param name="type">The type to examine</param>
		/// <param name="name">The name of the method</param>
		/// <param name="bindingFlags">BindingFlags to use</param>
		/// <returns>A PropertyInfo or null</returns>
		public static PropertyInfo GetNamedProperty( Type type, string name, BindingFlags bindingFlags )
		{
			return type.GetProperty( name, bindingFlags );
		}

		void ProcessExpectedException(Exception exception, TestCaseResult testResult)
		{
			if (expectedException.Equals(exception.GetType()))
			{
				if (expectedMessage != null && !expectedMessage.Equals(exception.Message))
				{
					string message = string.Format("Expected exception to have message: \"{0}\" but received message \"{1}\"", 
						expectedMessage, exception.Message);
					testResult.Failure(message, exception.StackTrace);
				} 
				else 
				{
					testResult.Success();
				}
			}
			else if ( testFramework.IsAssertException( exception ) )
			{
				RecordException(exception,testResult);
			}
			else
			{
				string message = "Expected: " + expectedException.Name + " but was " + exception.GetType().Name;
				testResult.Failure(message, exception.StackTrace);
			}
		}
	}
}
