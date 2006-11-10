using System;
using System.Net;
using MbUnit.Framework;
using Subtext.Akismet;

namespace UnitTests.Subtext.Akismet
{
	[TestFixture]
	public class AkismetCommentTests
	{
		[Test]
		public void CanCreateCommentAndSetProperties()
		{
			Comment comment = new Comment(IPAddress.Parse("127.0.0.1"), "fake-user-agent");
			comment.Author = "Test";
			comment.AuthorEmail = "test@test.com";
			comment.AuthorUrl = new Uri("http://haacked.com/");
			comment.CommentType = "comment";
			comment.Content = "Akismet kicks butt!";
			comment.Permalink = new Uri("http://haacked.com/123.aspx");
			comment.Referrer = "http://nobody-referred";
			Assert.IsNotNull(comment.ServerEnvironmentVariables);
			Assert.AreEqual(comment.IPAddress, IPAddress.Parse("127.0.0.1"));
			Assert.AreEqual(comment.UserAgent, "fake-user-agent");
			Assert.AreEqual("Test", comment.Author);
			Assert.AreEqual("test@test.com", comment.AuthorEmail);
			Assert.AreEqual(new Uri("http://haacked.com/"), comment.AuthorUrl);
			Assert.AreEqual("comment", comment.CommentType);
			Assert.AreEqual("Akismet kicks butt!", comment.Content);
			Assert.AreEqual(new Uri("http://haacked.com/123.aspx"), comment.Permalink);
			Assert.AreEqual("http://nobody-referred", comment.Referrer);
		}
	}
}
