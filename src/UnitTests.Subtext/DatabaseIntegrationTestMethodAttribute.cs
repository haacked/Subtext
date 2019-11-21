using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace UnitTests.Subtext
{
    internal class DatabaseIntegrationTestMethodAttribute : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var transactionScope = new TransactionScope();

            try
            {
                return base.Execute(testMethod);
            }
            finally
            {
                transactionScope.Dispose();
            }
        }
    }
}
