using System;
using System.IO;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class ImageTests
	{
		[RowTest]
		[Row(@"z:\abc-ae323340-eghe-23423423.jpg", true)]
		[Row(@"z:\abc-ae323340-eghe-23423423.txt", false)]
		public void ValidateFileReturnsCorrectAnswer(string fileName, bool expected)
		{
			Assert.AreEqual(expected, Images.ValidateFile(fileName));
		}

		[Test]
		public void ValidateFileReturnsFalseForExistingFile()
		{
			try
			{
				using (StreamWriter writer = File.CreateText("test.gif"))
				{
					writer.Write("Test");
				}
				Assert.IsFalse(Images.ValidateFile("test.gif"));
			}
			finally
			{
				if(File.Exists("test.gif"))
					File.Delete("test.gif");
			}
		}

		#region ExceptionTests
		[Test]
		[ExpectedArgumentNullException]
		public void CheckDirectoryThrowsArgumentNullException()
		{
			Images.CheckDirectory(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void DeleteImageThrowsArgumentNullException()
		{
			Images.DeleteImage(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void GetFileNameThrowsArgumentNullException()
		{
			Images.GetFileName(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertImageThrowsArgumentNullException()
		{
			Images.InsertImage(null, new byte[0]);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void MakeAlbumImagesThrowsArgumentNullException()
		{
			Images.MakeAlbumImages(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void SaveImageThrowsArgumentNullExceptionForNullBuffer()
		{
			Images.SaveImage(null, "x");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void SaveImageThrowsArgumentNullExceptionForNullFileName()
		{
			Images.SaveImage(new byte[0], null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void TryDeleteFileThrowsArgumentNullException()
		{
			Images.TryDeleteFile(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateThrowsArgumentNullExceptionForNullImage()
		{
			Images.Update(null, new byte[0]);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateThrowsArgumentNullExceptionForNullBuffer()
		{
			Images.Update(new Image(), null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateImageThrowsArgumentNullException()
		{
			Images.UpdateImage(null);
		}
		#endregion
	}
}
