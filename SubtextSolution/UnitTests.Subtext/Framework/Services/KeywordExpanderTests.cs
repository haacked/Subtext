using System.Collections.Generic;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class KeywordExpanderTests
    {
        [Test]
        public void Replace_WithStringContainingKeyword_ExpandsKeyword() { 
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord { 
                Word = "sucky example", 
                Text = "cool example", 
                Url = "http://example.com/", 
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("This is a sucky example");

            //assert
            Assert.AreEqual("This is a <a href=\"http://example.com/\">cool example</a>", result);
        }

        [Test]
        public void Replace_WithStringContainingKeyword_ExpandsKeywordWithFirstMatchOnly()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "sucky example",
                Text = "cool example",
                Url = "http://example.com/",
                ReplaceFirstTimeOnly = true,
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("This is a sucky example");

            //assert
            Assert.AreEqual("This is a <a href=\"http://example.com/\">cool example</a>", result);
        }

        [Test]
        public void Replace_WithStringContainingKeyword_ExpandsKeywordWithTitle()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "sucky example",
                Text = "cool example",
                Url = "http://example.com/",
                Title = "the title"
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("This is a sucky example");

            //assert
            Assert.AreEqual("This is a <a href=\"http://example.com/\" title=\"the title\">cool example</a>", result);
        }

        [Test]
        public void Replace_WithKeywordSurroundedByUnderscores_IsNotExpanded()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "is",
                Text = "is",
                Url = "http://example.com/{0}",
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform(" _is_ ");

            //assert
            Assert.AreEqual(" _is_ ", result);
        }

        [Test]
        public void Replace_WithStringContainingKeyword_IsNotCaseSensitive()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "is",
                Text = "is",
                Url = "http://example.com/",
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform(" it IS true ");

            //assert
            Assert.AreEqual(" it <a href=\"http://example.com/\">is</a> true ", result);
        }

        [Test]
        public void Replace_WithStringContainingKeywordSpecifiedAsCaseSensitive_IsCaseSensitive()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "is",
                Text = "is",
                Url = "http://example.com/",
                CaseSensitive = true
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform(" it IS true ");

            //assert
            Assert.AreEqual(" it IS true ", result);
        }

        [Test]
        public void Replace_WithStringContainingKeywordInsideAnchorTagAttribute_DoesNotExpandKeyword()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "keyword",
                Text = "keyword",
                Url = "http://example.com/",
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("<a title=\"keyword\" href=\"http://x\">test</a>");

            //assert
            Assert.AreEqual("<a title=\"keyword\" href=\"http://x\">test</a>", result);
        }

        [Test]
        public void Replace_WithStringContainingKeywordInsideAnchorTagInnerText_DoesNotExpandKeyword()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "keyword",
                Text = "keyword",
                Url = "http://example.com/",
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("<a href=\"http://x\">a keyword test</a>");

            //assert
            Assert.AreEqual("<a href=\"http://x\">a keyword test</a>", result);
        }

        [Test]
        public void Replace_WithStringContainingKeywordInAnotherWord_DoesNotExpandKeyword()
        {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "is",
                Text = "is",
                Url = "http://example.com/",
            });
            var keywordExpander = new KeywordExpander(keywords);

            //act
            string result = keywordExpander.Transform("This should not expand");

            //assert
            Assert.AreEqual("This should not expand", result);
        }

        [Test]
        public void Ctor_WithRepository_GetsKeywordsFromRepository() {
            //arrange
            var keywords = new List<KeyWord>();
            keywords.Add(new KeyWord
            {
                Word = "is",
                Text = "is",
                Url = "http://example.com/",
            });
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetKeyWords()).Returns(keywords);
            
            //act
            var keywordExpander = new KeywordExpander(keywords);

            //assert
            Assert.AreEqual(keywords, keywordExpander.Keywords);
        }
    }
}
