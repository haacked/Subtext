using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Moq;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class SlugGeneratorTests
    {
        [Test]
        public void ConvertTitleToSlug_WithNullEntry_ThrowsArgumentNullException() {
            //arrange
            var generator = new SlugGenerator(null);

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => generator.GetSlugFromTitle(null));
        }

        [Test]
        public void ConvertTitleToSlug_WithNullOrEmptyTitle_ThrowsArgumentException() {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = string.Empty };

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => generator.GetSlugFromTitle(entry));
        }

        [Test]
        public void Ctor_WithNullFriendlySettings_UsesDefaults()
        {
            //arrange
            var generator = new SlugGenerator(null);
            
            //act
            var settings = generator.SlugSettings;

            //act
            Assert.IsTrue(settings.Enabled);
            Assert.AreEqual("-", settings.SeparatingCharacter);
            Assert.AreEqual(TextTransform.LowerCase, settings.TextTransformation);
            Assert.AreEqual(10, settings.WordCountLimit);
        }

        [Test]
        public void ConvertTitleToSlug_WithSpacesInTitle_ReplacesSpacesInTitle() {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this-is-a-test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithNullCharacterDelimiterAndSpacesInTitle_RemovesSpacesAndPascalCasesTitle()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", "");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = " this is a test " };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("ThisIsATest", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithTextTransformToUpperCase_TransformsUrlToUpperCase()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", ".");
            config.Add("TextTransform", "UpperCase");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("THIS.IS.A.TEST", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithTitleHavingExtraWhitespace_NormalizesWhitespace()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", "-");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "    this  is   a test\r\n" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this-is-a-test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithMoreWordsThanAllowed_TruncatesRemainingWords()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "2");
            config.Add("separatingCharacter", "_");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this_is", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithInvalidSeparator_UsesDefault()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", "*");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this-is-a-test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithDotSeparator_UsesDot()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", ".");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this.is.a.test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithInternationalizedTitles_ConvertsToAnsiUrlSlug()
        {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = "Åñçhòr çùè Héllò wörld" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("anchor-cue-hello-world", slug);
        }

        public void ConvertTitleToSlug_WithNonEuropeanInternationalizedTitles_ConvertsToUrlEncodedTitle()
        {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = "안-녕하십니까" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("%ec%95%88-%eb%85%95%ed%95%98%ec%8b%ad%eb%8b%88%ea%b9%8c", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithNonWordCharacters_RemoveNonWordCharacters() {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = @"[!""'`;:~@#foo$%^&-bar*(){\[}\]?+/=\\|<>_baz" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar_baz", slug);
        }

        [Test]
        public void ConvertTitleToSlug_UsingPeriod_NormalizesPeriods()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", ".");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "this. is...a test." };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this.is.a.test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_UsingDash_NormalizesDashes()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", "-");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "-this - is - a - test-" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("this-is-a-test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithTitleEndingInPeriod_RemovesTrailingPeriod()
        {
            //arrange
            var config = new NameValueCollection();
            config.Add("limitWordCount", "10");
            config.Add("separatingCharacter", "-");
            var settings = new FriendlyUrlSettings(config);
            var generator = new SlugGenerator(settings);
            var entry = new Entry(PostType.BlogPost) { Title = "a test." };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("a-test", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithAllNumericTitle_PrependsLetterNToAvoidConflicts()
        {
            //arrange
            var generator = new SlugGenerator(null);
            var entry = new Entry(PostType.BlogPost) { Title = @"1234" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("n_1234", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingExistingEntry_AppendsAgainToSlug() {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-again", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingTwoExistingEntries_AppendsAgainToSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Yet-Again", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-yet-again", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingThreeExistingEntries_AppendsUniqueSuffixToSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Yet-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-And-Again", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-and-again", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingFourExistingEntries_AppendsUniqueSuffixToSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Yet-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-And-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Once-Again", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-once-again", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingFiveExistingEntries_AppendsUniqueSuffixToSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Yet-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-And-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Once-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Once-More", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-once-more", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugMatchingSixExistingEntries_AppendsUniqueSuffixToSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry("foo-bar", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Yet-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-And-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Once-Again", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-Once-More", false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-To-Beat-A-Dead-Horse", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act
            string slug = generator.GetSlugFromTitle(entry);

            //act
            Assert.AreEqual("foo-bar-to-beat-a-dead-horse", slug);
        }

        [Test]
        public void ConvertTitleToSlug_WithSlugAndAllPrefixesMatchingExistingEntries_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry(It.IsAny<string>(), false, false)).Returns(new Entry(PostType.BlogPost));
            repository.Setup(r => r.GetEntry("foo-bar-again", false, false)).Returns((Entry)null);
            var generator = new SlugGenerator(null, repository.Object);
            var entry = new Entry(PostType.BlogPost) { Title = @"foo bar" };

            //act, assert
            UnitTestHelper.AssertThrows<InvalidOperationException>(() => generator.GetSlugFromTitle(entry));
        }
    }
}
