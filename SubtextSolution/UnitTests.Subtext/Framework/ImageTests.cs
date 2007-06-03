using System;
using System.IO;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class ImageTests
	{
		static Byte[] singlePixelBytes = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

		[Test]
		[Ignore]
		[RollBack]
		public void CanGetImagesByCategoryId()
		{
			//TODO: FIX!
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Test", CategoryType.ImageCollection);

			Assert.AreEqual(0, Images.GetImagesByCategoryID(categoryId, true).Count);

			Image image = CreateImageInstance();
			image.IsActive = true;
			image.CategoryID = categoryId;
			int imageId = Images.InsertImage(image, singlePixelBytes);

			ImageCollection images = Images.GetImagesByCategoryID(categoryId, true);
			Assert.AreEqual(1, images.Count);
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
			string filePath = Path.GetFullPath(@"test\test.gif");
			try
			{
				Assert.IsTrue(Images.SaveImage(singlePixelBytes, filePath));
				FileAssert.Exists(filePath);
			}
			finally
			{
				if(File.Exists(filePath))
					File.Delete(filePath);

				if(Directory.Exists(Path.GetDirectoryName(filePath)))
					Directory.Delete(Path.GetDirectoryName(filePath), true);
			}
		}

		[Test]
		public void CanMakeAlbumImages()
		{
			Image image = new Image();
			image.Title = "Test Image";
			image.Height = 1;
			image.Width = 1;
			image.IsActive = true;
			image.LocalDirectoryPath = Path.GetFullPath(@"test") + @"\";
			image.FileName = "test.gif";
			
			//Write original image.
			try
			{
				Images.SaveImage(singlePixelBytes, image.OriginalFilePath);
				FileAssert.Exists(image.OriginalFilePath);

				Images.MakeAlbumImages(image);
				FileAssert.Exists(image.ResizedFilePath);
				FileAssert.Exists(image.ThumbNailFilePath);
			}
			finally
			{
				Directory.Delete(image.LocalDirectoryPath, true);
			}
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
				Images.DeleteImage(loadedImage);
				Assert.IsNull(Images.GetSingleImage(imageId, false));
				if(File.Exists(image.LocalDirectoryPath))
					File.Delete(image.LocalDirectoryPath);
			}
		}

		private static Image CreateImageInstance()
		{
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Test");
			Image image = new Image();
			image.Title = "Test Image";
			image.BlogId = Config.CurrentBlog.Id;
			image.CategoryID = categoryId;
			image.Height = 1;
			image.Width = 1;
			image.IsActive = true;
			image.LocalDirectoryPath = Path.GetFullPath(@"test") + @"\";
			image.FileName = "test.gif";
			return image;
		}

		[Test]
		public void CanCheckDirectory()
		{
			string dir = Path.GetFullPath("test-dir") + @"\";
			try
			{
				Images.CheckDirectory(dir);
				Assert.IsTrue(Directory.Exists(dir));
			}
			finally
			{
				Directory.Delete(dir);
			}
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
		[ExpectedArgumentException]
		public void CheckDirectoryThrowsArgumentException()
		{
			Images.CheckDirectory("");
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
	}
}

