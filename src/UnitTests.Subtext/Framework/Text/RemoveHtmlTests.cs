using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    /// <summary>
    /// Unit tests of the <see cref="HtmlHelper.RemoveHtml"/> method and 
    /// just that method.
    /// </summary>
    [TestFixture]
    public class RemoveHtmlTests
    {
        [Test]
        public void NullHtml_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(null));
        }

        [Test]
        public void Html_WithEmptyString_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(string.Empty));
        }

        [Test]
        public void Html_WithNoTags_ReturnsTextOnly()
        {
            string html = "This has no tags!";
            Assert.AreEqual(html, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithOnlyATag_ReturnsEmptyString()
        {
            string html = "<foo>";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithOnlyConsecutiveTags_ReturnsEmptyString()
        {
            string html = "<foo><bar><baz />";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTextBeforeTag_ReturnsText()
        {
            string html = "Hello<foo>";
            Assert.AreEqual("Hello", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTextAfterTag_ReturnsText()
        {
            string html = "<foo>World";
            Assert.AreEqual("World", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTextBetweenTags_ReturnsText()
        {
            string html = "<p><foo>World</foo></p>";
            Assert.AreEqual("World", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithClosingTagInAttrValue_StripsEntireTag()
        {
            string html = "<foo title=\"/>\" />";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithSingleQuotedAttrContainingDoubleQuotesAndEndTagChar_StripsEntireTag()
        {
            string html = @"<foo ='test""/>title' />";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithDoubleQuotedAttributeContainingSingleQuotesAndEndTagChar_StripsEntireTag()
        {
            string html = @"<foo =""test'/>title"" />";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithNonQuotedAttribute_StripsEntireTagWithoutStrippingText()
        {
            string html = @"<foo title=test>title />";
            Assert.AreEqual("title />", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithNonQuotedAttributeContainingDoubleQuotes_StripsEntireTagWithoutStrippingText()
        {
            string html = @"<p title = test-test""-test>title />Test</p>";
            Assert.AreEqual("title />Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithNonQuotedAttributeContainingQuotedSection_StripsEntireTagWithoutStrippingText()
        {
            string html = @"<p title = test-test""- >""test> ""title />Test</p>";
            Assert.AreEqual(@"""test> ""title />Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTagClosingCharInAttributeValueWithNoNameFollowedByText_ReturnsText()
        {
            string html = @"<foo = "" />title"" />Test";
            Assert.AreEqual("Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTextThatLooksLikeTag_ReturnsText()
        {
            string html = @"<çoo = "" />title"" />Test";
            Assert.AreEqual(html, HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithCommentOnly_ReturnsEmptyString()
        {
            string s = "<!-- this go bye bye>";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithNonDashDashComment_ReturnsEmptyString()
        {
            string s = "<! this go bye bye>";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithTwoConsecutiveComments_ReturnsEmptyString()
        {
            string s = "<!-- this go bye bye><!-- another comment>";
            Assert.AreEqual(string.Empty, HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithTextBeforeComment_ReturnsText()
        {
            string s = "Hello<!-- this go bye bye -->";
            Assert.AreEqual("Hello", HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithTextAfterComment_ReturnsText()
        {
            string s = "<!-- this go bye bye -->World";
            Assert.AreEqual("World", HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithAngleBracketsButNotHtml_ReturnsText()
        {
            string s = "<$)*(@&$(@*>";
            Assert.AreEqual(s, HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithCommentInterleavedWithText_ReturnsText()
        {
            string s = "Hello <!-- this go bye bye --> World <!--> This is fun";
            Assert.AreEqual("Hello  World  This is fun", HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithCommentBetweenNonTagButLooksLikeTag_DoesStripComment()
        {
            string s = @"<ç123 title=""<!bc def>"">";
            Assert.AreEqual(@"<ç123 title="""">", HtmlHelper.RemoveHtml(s));
        }

        [Test]
        public void Html_WithTagClosedByStartTag_StripsFirstTag()
        {
            string html = "<foo<>Test";
            Assert.AreEqual("<>Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTagClosedByStartComment_StripsFirstTag()
        {
            //Note in Firefox, this renders: <!--foo>Test
            string html = "<foo<!--foo>Test";
            Assert.AreEqual("Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTagClosedByProperComment_StripsFirstTag()
        {
            string html = "<FOO<!-- FOO -->Test";
            Assert.AreEqual("Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_WithTagClosedByEmptyComment_StripsFirstTag()
        {
            string html = "<foo<!>Test";
            Assert.AreEqual("Test", HtmlHelper.RemoveHtml(html));
        }

        [Test]
        public void Html_ThatBrokeSubtextBefore_NowDoesntPegCPU()
        {
            string html =
                @"Hi Friends,<br /><br />I have some different problem to validate the HTML<br /><br />html code is something like <br /><br /><html><br />.<br />.<br />.<br /><body ...><br /><br /><table ..><br /><tr ..><td><br />  <img src=""test.asp?t=123"" border=0 alt=test /><br />  ...<br /><br />  <script type='text/javascript'><br />  <!--<br />     function test(xvar){<br />        var text = xvar;<br />        .................<br />     }<br />   --><br /></td><br /></tr><br /></body><br /></html<br /><br /><br />Now i need to validate all tag attributes value should be with ""<br /><br />e.g <br />img tag alt and border attribute values are without "" <br />there are various tags like table and div has same problem <br /><br /><br />can anyone help me find attributes value without quotation mark and replace with quotation mark (please note that some javascript or URL's with query para should not should not effect)<br /><br /><br />Sincerely thanks in advance<br /><br />";
            string expected =
                @"Hi Friends,I have some different problem to validate the HTMLhtml code is something like ...    ...         function test(xvar){        var text = xvar;        .................     }   -->Now i need to validate all tag attributes value should be with ""e.g img tag alt and border attribute values are without "" there are various tags like table and div has same problem can anyone help me find attributes value without quotation mark and replace with quotation mark (please note that some javascript or URL's with query para should not should not effect)Sincerely thanks in advance";
            Assert.AreEqual(expected, HtmlHelper.RemoveHtml(html));
        }
    }
}