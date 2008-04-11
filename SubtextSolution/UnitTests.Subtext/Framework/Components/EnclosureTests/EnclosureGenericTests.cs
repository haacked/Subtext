#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using System.Threading;
using System.Globalization;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureGenericTests
    {
        [RowTest]
        [Row(100, "100 bytes")]
        [Row(1024, "1 KB")]
        [Row(1162, "1,13 KB")]
        [Row(7862732, "7,5 MB")]
        [MultipleCulture("it-IT,en-US")]
        public void SizeIsFormattedCorrectly(long size, string expected)
        {
            Enclosure enc = new Enclosure();
            enc.Size = size;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT");
            Assert.AreEqual(expected, enc.FormattedSize, "Not the right formatting");
        }
    }
}
