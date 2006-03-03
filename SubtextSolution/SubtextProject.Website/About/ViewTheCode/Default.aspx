<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="AboutLinks" Src="~/About/AboutLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - About - View The Code</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:AboutLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	<h2>View The Code</h2>
	<p>Well this is an open source project, right?  So where is the code?!</p>
	<p>
		Well if you&#8217;re not part of the Subtext team, but want to 
		play around with the source code anyways, it is pretty easy to 
		obtain the code.  Just follow the following steps:
	</p>
	
	<ul>
		<li>
			First, make sure you have <a href="http://subversion.tigris.org/servlets/ProjectDocumentList?folderID=91" rel="external" title="Subversion 1.3.0-setup.exe (3.18 mb)">Subversion Client</a> 
			installed.
		</li>
		<li>
			Download this <a href="/code/GetSubtext.zip" title="Useful batch utilities">zip archive</a> containing two useful batch files.
		</li>
		<li>
			Run the batch file &#8220;CreateLocalSubtextDatabase.bat&#8221; to create a local database 
			samed &#8220;SubtetxData&#8221;.  <strong>NOTE:</strong> if you already have a database 
			named SubtextData, this script will delete that one, so run at your own risk.
		</li>
		<li>
			The script should automatically map the ASPNET account as a user in the 
			database.  Confirm via Enterprise Manager.
		</li>
		<li>
			Now place GetSubtext.bat in your root &#8220;projects&#8221; folder or equivalent 
			(for me, I would place this file in my &#8220;c:\Projects&#8221; folder, though 
			some use c:\Dev or c:\Work).
		</li>
		<li>
			Now double click the batch file.  This will create a subfolder named SubtextSystem and check 
			out the trunk into that subfolder.
		</li>
	</ul>
	<p>
		If this step fails, make sure that the svn.exe is in your path.
	</p>
	<p>
		When you run the batch file, it downloads all the source code from the Subversion repository  
		and then runs a downloaded script named &#8220;CreateSubtextVdir.vbs&#8221; that 
		will set up a virtual directory in IIS pointing to the correct location.  
		All you have to do is compile and browse to &#8220;http://localhost/Subtext.Web/&#8221;.
	</p>
	<p>
		<strong>Other Notes:</strong> Make sure that you have the aspnet_client folder installed 
		under the root of your default website.
	</p>

</MP:MasterPage>
