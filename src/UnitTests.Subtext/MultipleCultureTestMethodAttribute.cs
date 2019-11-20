using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Subtext
{
    internal class MultipleCultureTestMethodAttribute : TestMethodAttribute
    {
        private readonly string[] _cultures;

        public MultipleCultureTestMethodAttribute(string cultures)
        {
            if (cultures == null)
            {
                throw new ArgumentNullException("cultures");
            }

            this._cultures = cultures.Split(new char[] { ',' });
        }

        public MultipleCultureTestMethodAttribute(string[] cultures)
        {
            if (cultures == null)
            {
                throw new ArgumentNullException("cultures");
            }

            this._cultures = cultures;
        }

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var testResults = new List<TestResult>();

            var currentCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                foreach (var culture in this._cultures)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

                    var cultureTestResults = base.Execute(testMethod);

                    testResults.AddRange(cultureTestResults);
                }

                return testResults.ToArray();
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }
    }
}
