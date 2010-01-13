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
            Assert.AreEqual(new []{"e","a","in","che"}, settings.StopWords);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithEmptyConfig_TuningParametersAreDefault()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.IsNotNull(settings.Parameters);
            Assert.AreEqual(1f, settings.Parameters.EntryNameBoost);
        }

        [Test]
        public void FullTextSearchEngineSettings_WithConfig_MinimumDocumentFrequencyIsSet()
        {
            FullTextSearchEngineSettings settings = FullTextSearchEngineSettings.Settings;
            Assert.IsNotNull(settings.Parameters);
            Assert.AreEqual(10, settings.Parameters.MinimumDocumentFrequency);
        }
        
    }
}
