using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Web.UI.Controls;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestFixture]
    public class EntryListTests
    {
        [Test]
        public void TestBodyTruncation()
        {
            Entry entry;
            string returned;
            //empty body, set to truncate body to 10 words
            entry=new Entry(PostType.BlogPost);
            returned=EntryList.ShowTruncatedBody(entry,10);
            Assert.AreEqual("<p></p>", returned);

            //body with only one word

            entry = new Entry(PostType.BlogPost);
            entry.Body = "Test";
            returned = EntryList.ShowTruncatedBody(entry, 10);
            Assert.AreEqual("<p>Test</p>", returned);

            //body with same number of words as limit
            //should be returned as is.

            entry = new Entry(PostType.BlogPost);
            entry.Body = "This is a test blog post.";
            returned = EntryList.ShowTruncatedBody(entry, 6);
            Assert.AreEqual("<p>This is a test blog post.</p>", returned);

            //body with more than 10 words, should be truncated to 6 and have ... dots

            entry = new Entry(PostType.BlogPost);
            entry.Body = "This is a test blog post with more than 10 words";
            returned = EntryList.ShowTruncatedBody(entry,6);
            Assert.AreEqual("<p>This is a test blog post...</p>", returned);
        }
    }
}
