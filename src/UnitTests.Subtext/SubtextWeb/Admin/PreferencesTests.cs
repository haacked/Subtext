using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Web.Admin;

namespace UnitTests.Subtext.SubtextWeb.Admin
{
    [TestFixture]
    public class PreferencesTests
    {
        [Test]
        public void UsePlainHtmlEditor_IsFalse_ByDefault()
        {
            using (var httpRequest = new HttpSimulator("http://localhost").SimulateRequest())
            {
                //arrange / act
                var option = Preferences.UsePlainHtmlEditor;

                //post
                Assert.IsFalse(option);
            }
        }

        [Test]
        public void UsePlainHtmlEditor_SetValue()
        {
            using (var http = new HttpSimulator("http://localhost").SimulateRequest())
            {
                //arrange
                Preferences.UsePlainHtmlEditor = true;
                
                //act
                var option = Preferences.UsePlainHtmlEditor;
                
                //post
                Assert.IsTrue(option);
            }
        }
    }
}
