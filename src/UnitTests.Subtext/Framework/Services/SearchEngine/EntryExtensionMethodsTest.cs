using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services.SearchEngine;

namespace UnitTests.Subtext.Framework.Services.SearchEngine
{
    [TestFixture]
    public class EntryExtensionMethodsTest
    {
        [Test]
        public void EntryExtensionMethodsTest_ConvertToSearchEngineEntry_WithTags_ConvertsTagsToString()
        {
            Entry post = new Entry(PostType.BlogPost)
                             {
                                 Blog = new  Blog(){ Title="MyTitle", BlogGroupId=1},
                             };
            IList<String> tags = new List<string>() {"tag1","tag2"};
            SearchEngineEntry searchEntry = post.ConvertToSearchEngineEntry(tags);

            Assert.AreEqual("tag1,tag2", searchEntry.Tags);
        }

        [Test]
        public void EntryExtensionMethodsTest_ConvertToSearchEngineEntry_StripsHtmlTags()
        {
            Entry post = new Entry(PostType.BlogPost)
            {
                Blog = new Blog() { Title = "MyTitle", BlogGroupId = 1 },
                Body = "this is <b>bold</b> text"
            };
            SearchEngineEntry searchEntry = post.ConvertToSearchEngineEntry();

            Assert.AreEqual("this is bold text", searchEntry.Body);
        }

        [Test]
        public void EntryExtensionMethodsTest_ConvertToSearchEngineEntry_WithOutTags_ConvertsTagsToString()
        {
            Entry post = new Entry(PostType.BlogPost)
            {
                Blog = new Blog() { Title = "MyTitle", BlogGroupId = 1 },
                Body = "<a href=\"http://blah.com/subdir/tag1/\" rel=\"tag\">tag1</a><a href=\"http://blah.com/another-dir/tag2/\" rel=\"tag\">tag2</a>"
            };
            SearchEngineEntry searchEntry = post.ConvertToSearchEngineEntry();

            Assert.AreEqual("tag1,tag2", searchEntry.Tags);
        }
    }
}
