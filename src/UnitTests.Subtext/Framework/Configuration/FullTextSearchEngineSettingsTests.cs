using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using MbUnit.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Configuration
{
    [TestFixture]
    public class FullTextSearchEngineSettingsTests
    {

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_LanguageIsEnglish()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.AreEqual("English", settings.Language);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_StopWordsIsDefaultOne()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.AreEqual(StopAnalyzer.ENGLISH_STOP_WORDS_SET, settings.StopWords);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithStopWordsSpecified_StopWordsAreTheCorrectOnes()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            settings.StopWordsString = "e,a,in,che";
            Assert.IsTrue(settings.StopWords.Contains("e"));
            Assert.IsTrue(settings.StopWords.Contains("a"));
            Assert.IsTrue(settings.StopWords.Contains("in"));
            Assert.IsTrue(settings.StopWords.Contains("che"));
            Assert.IsFalse(settings.StopWords.Contains("ch"));
        }

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_TuningParametersAreDefault()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.IsNotNull(settings.Parameters);
            Assert.AreEqual(1f, settings.Parameters.EntryNameBoost);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithConfig_MinimumDocumentFrequencyIsAsSet()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.IsNotNull(settings.Parameters);
            Assert.AreEqual(10, settings.Parameters.MinimumDocumentFrequency);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_IndexFolderLocationIsAdd_Data()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.AreEqual("~/App_Data", settings.IndexFolderLocation);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_IsEnabledIsTrue()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.IsTrue(settings.IsEnabled);
        }
        
    }
}
