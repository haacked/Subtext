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
			First, make sure you have <a href="http://www.tortoisecvs.org/" target="_blank">TortoiseCVS</a> 
			installed.
		</li>

		<li>
			Create a database in SQL Server named “SubtextData”.
		</li>
		<li>
			Confirm that the built-in ASPNET account exists as a SQL Server login.  
			<mp:HelpToolTip id="hlpAspnetLogin" runat="server" HelpText="Confirm this using Enterprise Manager by going to the Security folder and clicking on &#8220;Logins&#8221;.  There should be a login with the name <strong>{ComputerName}\\ASPNET</strong>.  If not, create a new login here using Windows authentication and selecting the local ASPNET account.">How?</mp:HelpToolTip>
		</li>
		<li>
			Now create a user in the database using the ASPNET Login. 
			<mp:HelpToolTip id="Helptooltip1" runat="server" HelpText="In Enterprise Manager, expand the SubtextData database and click on the <em>Users</em> node. Right click here and select <em>New Database User</em>.  In the <em>Login name</em> drop down, select the ASPNET login.">How?</mp:HelpToolTip>
		</li>
		<li>
			<a href="/code/GetSubtext.zip">Download and unzip</a> the batch file 
			to your projects folder (for me, I would place this file in my 
			&#8220;c:\Projects&#8221; folder).
		</li>
		<li>Now double click the batch file.</li>
	</ul>
	<p>
		If you install TortoiseCVS in a location other than your program files, 
		you will need to modify the batch file to point to the correct location.
	</p>
	<p>
		When you run the batch file, it downloads all the source code from CVS 
		and then runs a downloaded script named &#8220;CreateSubtextVdir.vbs&#8221; that 
		will set up a virtual directory in IIS pointing to the correct location.  
		All you have to do is compile and browse to &#8220;http://localhost/Subtext.Web/&#8221;.
	</p>
	<p>
		<strong>Other Notes:</strong> Make sure that you have the aspnet_client folder installed 
		under the root of your default website.
	</p>

</MP:MasterPage>
