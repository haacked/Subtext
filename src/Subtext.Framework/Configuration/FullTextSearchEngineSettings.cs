#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Lucene.Net.Analysis;

namespace Subtext.Framework.Configuration
{
    public class FullTextSearchEngineSettings
    {
        public static readonly FullTextSearchEngineSettings Settings = (FullTextSearchEngineSettings)ConfigurationManager.GetSection("FullTextSearchEngineSettings");

        private string _stopWordsString;

        public FullTextSearchEngineSettings()
        {
            Language = "English";
            StopWords = StopAnalyzer.ENGLISH_STOP_WORDS;
            Parameters = new TuningParameters();
            MinimumScore = 0.1f;
        }

        public string Language { get; set; }
        public TuningParameters Parameters { get; set; }
        [XmlElement("StopWords")]
        public string StopWordsString
        {
            //private get { return _stopWordsString; }
            set 
            { 
                _stopWordsString = value;
                StopWords = _stopWordsString.Split(',');
            }
        }
        [XmlIgnore]
        public string[] StopWords { get; private set; }

        public float MinimumScore { get; set; }

    }

    public class TuningParameters
    {

        public TuningParameters()
        {
            TitleBoost = 2f;
            TagsBoost = 4f;
            BodyBoost = 1f;
            EntryNameBoost = 1f;
            MoreLikeThisBoost = true;
            MinimumDocumentFrequency = 5;
            MinimumTermFrequency = 2;
         }

        /// <summary>
        /// Boost to apply to the title of an entry. Default is 2.
        /// </summary>
        public Single TitleBoost { get; set; }
        /// <summary>
        /// Boost to apply to the tags of an entry. Default is 4.
        /// </summary>
        public Single TagsBoost { get; set; }
        /// <summary>
        /// Boost to apply to the body of an entry. Default is 1.
        /// </summary>
        public Single BodyBoost { get; set; }
        /// <summary>
        /// Boost to apply to the name of an entry. Default is 1.
        /// </summary>
        public Single EntryNameBoost { get; set; }
        /// <summary>
        /// Boost terms in query based on score.
        /// </summary>
        public Boolean MoreLikeThisBoost { get; set; }
        /// <summary>
        /// The frequency at which words will be ignored which do not occur in at least this
        /// many docs. Default is 5.
        /// </summary>
        public int MinimumDocumentFrequency { get; set; }
        /// <summary>
        /// The frequency below which terms will be ignored in the source doc.
        /// Default is 2.
        /// </summary>
        public int MinimumTermFrequency { get; set; }
    }
}
