using System;
using System.IO;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Tests of the Images class.
	/// </summary>
	/// <remarks>
	/// All tests should use the TestDirectory directory. For example, to create that 
	/// directory, just do this: Directory.Create(TestDirectory);
	/// </remarks>
	[TestFixture]
	public class ImageTests
	{
		private const string TestDirectory = "unit-test-dir";
		static Byte[] singlePixelBytes = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

		[Test]
		public void GetFileStreamReturnsNullForNullPostedFile()
		{
			Assert.IsNull(Images.GetFileStream(null), "Should return null and not throw exception");
		}

		[Test]
		[RollBack]
		public void CanGetImagesByCategoryId()
		{
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "UnitTestImages", CategoryType.ImageCollection);

			Assert.AreEqual(0, Images.GetImagesByCategoryID(categoryId, true).Count);

			Image image = CreateImageInstance();
			image.IsActive = true;
			image.CategoryID = categoryId;

			int imageId = Images.InsertImage(image, singlePixelBytes);

			ImageCollection images = Images.GetImagesByCategoryID(categoryId, true);
			Assert.AreEqual(1, images.Count, "Expected to get our one image.");
			Assert.AreEqual(imageId, images[0].ImageID);
		}

		[RowTest]
		[Row(@"z:\abc-ae323340-eghe-23423423.jpg", true)]
		[Row(@"z:\abc-ae323340-eghe-23423423.txt", false)]
		public void ValidateFileReturnsCorrectAnswer(string fileName, bool expected)
		{
			Assert.AreEqual(expected, Images.ValidateFile(fileName));
		}

		[Test]
		[RollBack]
		public void CanSaveImage()
		{
			string filePath = Path.GetFullPath(@TestDirectory + Path.DirectorySeparatorChar + "test.gif");
			
			Assert.IsTrue(Images.SaveImage(singlePixelBytes, filePath));
			FileAssert.Exists(filePath);
		}

		[Test]
		public void CanMakeAlbumImages()
		{
			Image image = new Image();
			image.Title = "Test Image";
			image.Height = 1;
			image.Width = 1;
			image.IsActive = true;
			image.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
			image.FileName = "test.gif";
			
			//Write original image.
			
			Images.SaveImage(singlePixelBytes, image.OriginalFilePath);
			FileAssert.Exists(image.OriginalFilePath);

			Images.MakeAlbumImages(image);
			FileAssert.Exists(image.ResizedFilePath);
			FileAssert.Exists(image.ThumbNailFilePath);
		}

		[Test]
		[RollBack]
		public void CanInsertAndDeleteImage()
		{
			int imageId = 0;
			Image image = CreateImageInstance();
			image.IsActive = true;
			Image loadedImage = null;
			try
			{
				imageId = Images.InsertImage(image, singlePixelBytes);
				loadedImage = Images.GetSingleImage(imageId, false);
				Assert.IsNotNull(loadedImage);
				Assert.AreEqual(image.CategoryID, loadedImage.CategoryID);
			}
			finally
			{
				if(loadedImage != null)
					Images.DeleteImage(loadedImage);
				Assert.IsNull(Images.GetSingleImage(imageId, false));
			}
		}

		private static Image CreateImageInstance()
		{
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, TestDirectory);
			Image image = new Image();
			image.Title = "Test Image";
			image.BlogId = Config.CurrentBlog.Id;
			image.CategoryID = categoryId;
			image.Height = 1;
			image.Width = 1;
			image.IsActive = true;
			image.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
			image.FileName = "test.gif";
			return image;
		}

		[Test]
		public void CanCheckDirectory()
		{
			string dir = Path.GetFullPath(TestDirectory);
			Images.EnsureDirectory(dir);
			Assert.IsTrue(Directory.Exists(dir));
		}

		[Test]
		public void ValidateFileReturnsFalseForExistingFile()
		{
			Directory.CreateDirectory(TestDirectory);
			using (StreamWriter writer = File.CreateText(Path.Combine(TestDirectory, "test.gif")))
			{
				writer.Write("Ignored data");
			}
			Assert.IsFalse(Images.ValidateFile(Path.Combine(TestDirectory, "test.gif")));
		}

		#region ExceptionTests
		[Test]
		[ExpectedArgumentNullException]
		public void CheckDirectoryThrowsArgumentNullException()
		{
			Images.EnsureDirectory(null);
		}

		[Test]
		[ExpectedArgumentException]
		public void CheckDirectoryThrowsArgumentException()
		{
			Images.EnsureDirectory("");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void DeleteImageThrowsArgumentNullException()
		{
			Images.DeleteImage(null);
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
		[ExpectedArgumentException]
		public void SaveImageThrowsArgumentExceptionForNullFileName()
		{
			Images.SaveImage(new byte[0], "");
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

		[SetUp]
		public void SetUp()
		{
			if (Directory.Exists(TestDirectory))
				Directory.Delete(TestDirectory, true);
		}

		[TearDown]
		public void TearDown()
		{
			if (Directory.Exists(TestDirectory))
				Directory.Delete(TestDirectory, true);
		}
	}
}

