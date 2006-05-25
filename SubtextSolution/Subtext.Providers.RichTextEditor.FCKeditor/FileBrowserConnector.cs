#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
///Part of this code derives from the original FileBrowserConnector shipped with 
///the original FredCK.FCKeditorV2, which is redistributed with the following license

#region Original License
/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2005 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * "Support Open Source software. What about a donation today?"
 * 
 * 
 * File Authors:
 * 		Frederico Caldeira Knabben (fredck@fckeditor.net)
 */
#endregion
#endregion

using System;
using System.Globalization ;
using System.Xml ;
using System.Web ;
using System.Text.RegularExpressions;
using Subtext.Framework;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using FredCK.FCKeditorV2;

namespace Subtext.Providers.RichTextEditor.FCKeditor
{
	/// <summary>
	/// Summary description for FileBrowserConnector.
	/// </summary>
	public class FileBrowserConnector: System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			// Get the main request informaiton.
			string sCommand = Request.QueryString["Command"] ;
			if ( sCommand == null ) return ;

			string sResourceType = Request.QueryString["Type"] ;
			if ( sResourceType == null ) return ;

			string sCurrentFolder = Request.QueryString["CurrentFolder"] ;
			if ( sCurrentFolder == null ) return ;

			// Check the current folder syntax (must begin and start with a slash).
			if ( ! sCurrentFolder.EndsWith( "/" ) )
				sCurrentFolder += "/" ;
			if ( ! sCurrentFolder.StartsWith( "/" ) )
				sCurrentFolder = "/" + sCurrentFolder ;

			// File Upload doesn't have to return XML, so it must be intercepted before anything.
			if ( sCommand == "FileUpload" )
			{
				this.FileUpload( sResourceType, sCurrentFolder ) ;
				return ;
			}

			// Cleans the response buffer.
			Response.ClearHeaders() ;
			Response.Clear() ;

			// Prevent the browser from caching the result.
			Response.CacheControl = "no-cache" ;

			// Set the response format.
			Response.ContentEncoding	= System.Text.UTF8Encoding.UTF8 ;
			Response.ContentType		= "text/xml" ;

			XmlDocument oXML = new XmlDocument() ;
			XmlNode oConnectorNode = CreateBaseXml( oXML, sCommand, sResourceType, sCurrentFolder ) ;

			if(sResourceType.Equals("Image"))
			{
				// Execute the required command.
				switch( sCommand )
				{
					case "GetFolders" :
						this.GetFolders( oConnectorNode, sCurrentFolder ) ;
						break;
					case "GetFoldersAndFiles" :
						this.GetFolders( oConnectorNode, sCurrentFolder ) ;
						this.GetFiles( oConnectorNode, sResourceType,sCurrentFolder ) ;
						break;
					case "CreateFolder" :
						this.CreateFolder( oConnectorNode, sResourceType, sCurrentFolder ) ;
						break;
				}
			}
			else if(sResourceType.Equals("Posts"))
			{
				// Execute the required command.
				switch( sCommand )
				{
					case "GetFolders" :
						GetCategories( oConnectorNode, sCurrentFolder) ;
						break;
					case "GetFoldersAndFiles" :
						GetCategories( oConnectorNode, sCurrentFolder ) ;
						GetPosts( oConnectorNode, sCurrentFolder ) ;
						break;
					case "CreateFolder" :
						this.CreateFolder( oConnectorNode, sResourceType, sCurrentFolder ) ;
						break;
				}
			}
			else if(sResourceType.Equals("File"))
			{
				// Execute the required command.
				switch( sCommand )
				{
					case "GetFolders" :
						this.GetFolders( oConnectorNode, sCurrentFolder ) ;
						break;
					case "GetFoldersAndFiles" :
						this.GetFolders( oConnectorNode, sCurrentFolder ) ;
						this.GetFiles( oConnectorNode, sResourceType, sCurrentFolder ) ;
						break;
					case "CreateFolder" :
						this.CreateFolder( oConnectorNode, sResourceType, sCurrentFolder ) ;
						break;
				}
			}

			// Output the resulting XML.
			Response.Write( oXML.OuterXml ) ;

			Response.End() ;

			base.OnLoad (e);
		}
 
		#region Command Handlers

		private void GetFolders( XmlNode connectorNode,  string currentFolder )
		{
			// Map the virtual path to the local server path.
			string sServerDir = this.ServerMapFolder( currentFolder ) ;

			// Create the "Folders" node.
			XmlNode oFoldersNode = XmlUtil.AppendElement( connectorNode, "Folders" ) ;

			System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo( sServerDir ) ;
			System.IO.DirectoryInfo[] aSubDirs = oDir.GetDirectories() ;

			for ( int i = 0 ; i < aSubDirs.Length ; i++ )
			{
				// Create the "Folders" node.
				XmlNode oFolderNode = XmlUtil.AppendElement( oFoldersNode, "Folder" ) ;
				XmlUtil.SetAttribute( oFolderNode, "name", aSubDirs[i].Name ) ;
			}
		}

		private void GetFiles( XmlNode connectorNode, string resourceType,  string currentFolder )
		{
			// Map the virtual path to the local server path.
			string sServerDir = this.ServerMapFolder( currentFolder ) ;

			// Create the "Files" node.
			XmlNode oFilesNode = XmlUtil.AppendElement( connectorNode, "Files" ) ;

			System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo( sServerDir ) ;
			System.IO.FileInfo[] aFiles = oDir.GetFiles();

			for ( int i = 0 ; i < aFiles.Length ; i++ )
			{
				if(Regex.IsMatch(aFiles[i].Extension,GetAllowedExtension(resourceType),RegexOptions.IgnoreCase))
				{

					Decimal iFileSize = Math.Round( (Decimal)aFiles[i].Length / 1024 ) ;
					if ( iFileSize < 1 && aFiles[i].Length != 0 ) iFileSize = 1 ;

					// Create the "File" node.
					XmlNode oFileNode = XmlUtil.AppendElement( oFilesNode, "File" ) ;
					XmlUtil.SetAttribute( oFileNode, "name", aFiles[i].Name ) ;
					XmlUtil.SetAttribute( oFileNode, "size", iFileSize.ToString( CultureInfo.InvariantCulture ) ) ;
				}
			}
		}

		private void CreateFolder( XmlNode connectorNode, string resourceType, string currentFolder )
		{
			string sErrorNumber = "0" ;

			if(resourceType.Equals("Posts"))
			{
				sErrorNumber = "103" ;
			}
			else {
			string sNewFolderName = Request.QueryString["NewFolderName"] ;

				if ( sNewFolderName == null || sNewFolderName.Length == 0 )
					sErrorNumber = "102" ;
				else
				{
					// Map the virtual path to the local server path of the current folder.
					string sServerDir = this.ServerMapFolder( currentFolder ) ;

					try
					{
						Util.CreateDirectory( System.IO.Path.Combine( sServerDir, sNewFolderName )) ;
					}
					catch ( ArgumentException )
					{
						sErrorNumber = "102" ;
					}
					catch ( System.IO.PathTooLongException )
					{
						sErrorNumber = "102" ;
					}
					catch ( System.IO.IOException )
					{
						sErrorNumber = "101" ;
					}
					catch ( System.Security.SecurityException )
					{
						sErrorNumber = "103" ;
					}
					catch ( Exception )
					{
						sErrorNumber = "110" ;
					}
				}
			}

			// Create the "Error" node.
			XmlNode oErrorNode = XmlUtil.AppendElement( connectorNode, "Error" ) ;
			XmlUtil.SetAttribute( oErrorNode, "number", sErrorNumber ) ;
		}

		private void FileUpload( string resourceType, string currentFolder )
		{
			string sErrorNumber = "0" ;
			string sFileName = "" ;

			if(!resourceType.Equals("Posts"))
			{
				HttpPostedFile oFile = Request.Files["NewFile"] ;

				if ( oFile != null )
				{
					// Map the virtual path to the local server path.
					string sServerDir = this.ServerMapFolder( currentFolder ) ;

					// Get the uploaded file name.
					sFileName = System.IO.Path.GetFileName( oFile.FileName ) ;

					int iCounter = 0 ;

					while ( true )
					{
						string sFilePath = System.IO.Path.Combine( sServerDir, sFileName ) ;

						if ( System.IO.File.Exists( sFilePath ) )
						{
							iCounter++ ;
							sFileName = 
								System.IO.Path.GetFileNameWithoutExtension( oFile.FileName ) +
								"(" + iCounter + ")" +
								System.IO.Path.GetExtension( oFile.FileName ) ;

							sErrorNumber = "201" ;
						}
						else
						{
							oFile.SaveAs( sFilePath ) ;
							break ;
						}
					}
				}
				else
					sErrorNumber = "202" ;
			}
			else 
			{
				sErrorNumber = "203" ;
			}

			Response.Clear() ;

			Response.Write( "<script type=\"text/javascript\">" ) ;
			Response.Write( "window.parent.frames['frmUpload'].OnUploadCompleted(" + sErrorNumber + ",'" + sFileName.Replace( "'", "\\'" ) + "') ;" ) ;
			Response.Write( "</script>" ) ;

			Response.End() ;
		}

		#endregion

		#region Post Type Handler

		private static void GetCategories( XmlNode connectorNode, string currentFolder )
		{
			if(currentFolder.Equals("/") )
			{
				LinkCategoryCollection catList= Links.GetCategories(CategoryType.PostCollection,false);

				// Create the "Folders" node.
				XmlNode oFoldersNode = XmlUtil.AppendElement( connectorNode, "Folders" ) ;

				for ( int i = 0 ; i < catList.Count ; i++ )
				{
					// Create the "Folders" node.
					XmlNode oFolderNode = XmlUtil.AppendElement( oFoldersNode, "Folder" ) ;
					XmlUtil.SetAttribute( oFolderNode, "name", catList[i].Title ) ;
				}
			}
		}

		private static void GetPosts( XmlNode connectorNode,  string currentFolder )
		{
			PagedEntryCollection posts;
			if(currentFolder.Equals("/"))
			{
				posts= Entries.GetPagedEntries(PostType.BlogPost, -1,1, 1000,true);
			}
			else
			{
				string categoryName=currentFolder.Substring(1,currentFolder.Length-2);
				LinkCategory cat = Links.GetLinkCategory(categoryName,false);
				posts= Entries.GetPagedEntries(PostType.BlogPost, cat.CategoryID,1, 1000,true);
			}

			// Create the "Files" node.
			XmlNode oFilesNode = XmlUtil.AppendElement( connectorNode, "Files" ) ;
			for ( int i = 0 ; i < posts.Count ; i++ )
			{
				// Create the "File" node.
				if(posts[i].IsActive) 
				{
					XmlNode oFileNode = XmlUtil.AppendElement( oFilesNode, "File" ) ;
					XmlUtil.SetAttribute( oFileNode, "name", posts[i].Title+"|"+posts[i].Link ) ;
					XmlUtil.SetAttribute( oFileNode, "size", posts[i].DateUpdated.ToShortDateString() ) ;
				}
			}
		}

		#endregion

		#region Base XML Creation

		private static XmlNode CreateBaseXml( XmlDocument xml, string command, string resourceType, string currentFolder )
		{
			// Create the XML document header.
			xml.AppendChild( xml.CreateXmlDeclaration( "1.0", "utf-8", null ) ) ;

			// Create the main "Connector" node.
			XmlNode oConnectorNode = XmlUtil.AppendElement( xml, "Connector" ) ;
			XmlUtil.SetAttribute( oConnectorNode, "command", command ) ;
			XmlUtil.SetAttribute( oConnectorNode, "resourceType", resourceType ) ;

			// Add the current folder node.
			if(!resourceType.Equals("Posts")) 
			{
				XmlNode oCurrentNode = XmlUtil.AppendElement( oConnectorNode, "CurrentFolder" ) ;
				XmlUtil.SetAttribute( oCurrentNode, "path", currentFolder ) ;
				XmlUtil.SetAttribute( oCurrentNode, "url", GetUrlFromPath(currentFolder) ) ;
			}
			else 
			{
				XmlNode oCurrentNode = XmlUtil.AppendElement( oConnectorNode, "CurrentFolder" ) ;
				XmlUtil.SetAttribute( oCurrentNode, "path", currentFolder ) ;
				XmlUtil.SetAttribute( oCurrentNode, "url", "") ;
			}

			return oConnectorNode ;
		}

		#endregion

		#region Directory Mapping

		private string ServerMapFolder( string folderPath )
		{
			// Get the resource type directory.
			string sResourceTypePath = Server.MapPath(GetImageRootPath());

			// Return the resource type directory combined with the required path.
			return System.IO.Path.Combine( sResourceTypePath, folderPath.TrimStart('/') ) ;
		}

		private static string GetUrlFromPath( string folderPath )
		{
				return GetImageRootPath() + folderPath.Substring(1);
		}

		private static string GetImageRootPath() 
		{
			return Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);
		}
		#endregion

		private static string GetAllowedExtension(string resourceType)
		{
			string extStr="";
			if(resourceType.Equals("File")) 
			{
				extStr=FCKeditorRichTextEditorProvider.FileAllowedExtensions;
			}
			else if(resourceType.Equals("Image")) 
			{
				extStr=FCKeditorRichTextEditorProvider.ImageAllowedExtensions;
			}
			return extStr;
		}

	}
}
