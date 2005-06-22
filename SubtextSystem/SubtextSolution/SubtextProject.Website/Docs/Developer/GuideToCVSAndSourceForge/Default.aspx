<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Developer Documentation</h2>
	
	
		<h2><span class="title">Quickstart Guide To Open Source Development With CVS and SourceForge</span></h2>
		<h2>Introduction</h2>

<p>
This post is dedicated to the .NET head who&#8217;s grown up on Visual Source Safe and suddenly finds himself (or herself) in the midst of an open source project hosted by SourceForge.  CVS is very different from the check-out, check-in pessimistic locking approach taken by VSS.  I hope to demistify it just a bit so you can start hacking away at the numerous .NET based open source projects hosted on SourceForge.
</p>
<h2>Disclaimer</h2>
<p>
Keep in mind that I&#8217;m basing this on my experience.  Although there are multiple Windows CVS clients, I&#8217;ve only used <a href="http://www.tortoisecvs.org/" target="_blank">TortoiseCVS</a>.  However, I&#8217;m sure these experiences apply to <a href="http://www.wincvs.org/" target="_blank">WinCVS</a> as well.
</p>

<h2>Software</h2>
<p>
Before we begin, please download the following tools.
</p>
<ul>
<li><a href="http://www.tortoisecvs.org/" target="_blank">TortoiseCVS</a> - a Windows CVS client.</li>
<li><a href="http://the.earth.li/~sgtatham/putty/latest/x86/puttygen.exe" target="_blank">PuTTYGen</a> - Used to generate your SSH keys.</li>
<li><a href="http://the.earth.li/~sgtatham/putty/latest/x86/pageant.exe" target="_blank">Pageant</a></li>
<li><a href="http://the.earth.li/~sgtatham/putty/latest/x86/putty.exe" target="_blank">PuTTY</a></li>

</ul>
<h2>Generate SSH Keys</h2>
<p>
The next step is to run PuTTYGen to generate your SSH keys.</p>
<ol>
<li>In the Parameters section at the bottom, make sure to select &#8220;SSH2 DSA&#8221;. <br/><img src="http://haacked.com/images/PuttyGenScreenShot.gif" width="483" height="471" alt="PuTTYGen Screenshot" border="0" /></li>
<li>Click the &#8220;Generate&#8221; button.</li>
<li>Follow the on-screen instructions (&#8220;Please generate some randomness by moving the mouse over the blank area&#8221;). Key generation will be performed immediately afterward.</li>

<li>Upon completion of key generation, enter username@shell.sof.net in the &#8220;Key comment&#8221; field, replacing &#8217;username&#8217; with your SourceForge.net user name.  This comment will help you identify the purpose of this key.</li>
<li>Enter a passphrase and confirm it.<br /><img src="http://haacked.com/images/PuttyGenPassphrase.gif" width="483" height="471" alt="" border="0" /></li>
<li>Click on the &#8220;Save Private Key&#8221; button and save your private key (using the .ppk extension) somewhere you&#8217;ll be able to find it again.</li>
<li>Good, now you can post your keys on SourceForge.  Keep PuTTYGen open to where it is because you&#8217;ll need it later.</li>

</ol>
<h2>Posting Your SSH Keys</h2>
<p>
The point of this process is so that you don&#8217;t have to enter your password for every single CVS file operation.  In order to do that, CVS needs a copy of your public SSH key.  To do that, make sure you are logged in and...
</p>
<ol>
<li>Go to your <a href="http://sourceforge.net/account/" target="_blank">account page</a>.</li>
<li>Scroll down to the &#8220;Host Access Information&#8221; section.</li>

<li>You should see a section about the Project Shell Server. Click on the &#8220;Edit SSH Keys for Shell/CVS&#8221; link.<br /><img src="http://haacked.com/images/SourceForgeScreenshot.gif" width="504" height="265" alt="" border="0" /></li>
<li>
This will provide a form in which you can post your public key.  The text to post in here is displayed at the top of PuTTYGen in a text box with the label &#8220;Public key for pasting into OpenSSH authorized_keys file:&#8221;<br /><img src="http://haacked.com/images/PuttyGenPublicKeyForPasting.gif" width="472" height="212" alt="" border="0" />
</li>
<li>Make sure to follow the instructions on the page.  Multiple keys can be posted, as long as there is one per line.</li>
</ol>
<p>There is a delay before your keys are fully posted, so be patient.</p>
<h2>Getting Pageant Involved</h2>

<p>
Now is where Pageant gets involved.  Pageant is a little service that runs in your system tray.  It&#8217;s primary purpose is to provide authentication into SSH.  It holds your private keys in memory, already decoded, so you can use them without having to enter your passphrase all the time.  Instead, you enter your passphrase once when you start pageant.
</p>
<ol>
<li>After installing and running Pageant, you can double click on its icon at any time. It looks like a computer with a hat on it.</li>
<li>Simply click on the &#8220;Add Key&#8221; button and find the private (*.ppk) file you created earlier.  That&#8217;s it!</li>
</ol>
<h2>Checking Out A Module</h2>
<p>

At this point, you are all set to get going.  
</p>
<ol>
<li>Make sure you&#8217;ve been added as a developer to the project you&#8217;re going to work on. A project administrator would have to do this.</li>
<li>In Windows Explorer go to the folder you wish to check the code out into.</li>
<li>Right click and select the &#8220;CVS Checkout&#8221; command.<br /><img src="http://haacked.com/images/CvsCheckout.gif" width="287" height="288" alt="CVS Checkout Command" border="0" /></li>
<li>You will need your username on SourceForge and the project UNIX name.  For example, if your username was &#8220;haacked&#8221; (it isn&#8217;t, because that&#8217;s mine) and the project you were working on is &#8220;subtext&#8221;, you&#8217;d enter the following information

<ul>
<li>Protocol: Secure Shell (:ext)</li>
<li>Server: cvs.sourceforge.net</li>
<li>Directory: /cvsroot/subtext</li>
<li>Username: haacked</li>
</ul>
<br/><img src="http://haacked.com/images/CvsCheckoutModuleDialog.gif" width="482" height="494" alt="CVS Checkout Module Dialog" border="0" />
</li>
<li>Wait patiently as the project is created on your local machine.</li>
</ol>
<h2>Now Write Some Code</h2>

<p>
Note that you only have to checkout a module once.  Afterwards you can run the update command to get changes committed by other developers.  It&#8217;s a good idea to do this before and after you make any changes.
</p>
<p align="center">
<img src="http://haacked.com/images/CVSUpdateCommand.gif" width="329" height="443" alt="CVS Update" border="0" />
</p>
<h2>Commiting Changes</h2>
<p>
After you&#8217;ve changed some files, their icons be marked with an orange arrow.  To commit your changes, right click and select the Commit command.  Please make sure to enter an informative comment.
</p>
<p align="center"><img src="http://haacked.com/images/CVSCommitCommand.gif" width="376" height="297" alt="" border="0" /></p>
<p>
To commit multiple changes, right click on the root folder and select Commit.  You&#8217;llget a list of all changed files.  You can check the ones you wish to commit and commit them in bulk.

</p>
<h2>Adding Files</h2>
<p>
If you add a new file to the project, you&#8217;ll need to add it to CVS and THEN commit it.  To add a file, simply right click on it and select &#8220;CVS Add&#8221;.
</p>
<h2>Know when to ignore</h2>
<p>
TortoiseCVS is not integrated with Visual Studio.NET.  Thus it doesn&#8217;t know that there are some files you do not want to add to CVS such as *.suo, * and maybe the &#8220;bin&#8221; and &#8220;obj&#8221; folders.  To ignore folders, simply right click on them and select &#8220;CVS Ignore&#8221;.  This will create a .cvsignore file in the directory.  It&#8217;s probably not a bad idea to add this to the repository so that others don&#8217;t accidentally add &#8220;ignored&#8221; files.

</p>
<p>
You can also set ignored files using file patterns within TortoiseCVS&#8217;s preferences dialog.  Right click on any file and select &#8220;CVS&#8221; -> &#8220;Preferences&#8221;.  Under the &#8220;Ignored Files&#8221; tab, enter file patters such as *.user.
</p>
<h2>Submitting Patches as a Non-Developer</h2>
<p>
If you do not have developer access, you can still submit patches to a project.  In most SourceForge project sites, there is a &#8220;Patch&#8221; section where patches can be submitted.  In order to learn how to submit and apply patches, read the following article &#8220;<a href="http://www.hanselman.com/blog/PermaLink,guid,b6603ac5-3464-490f-a557-62f56b7f5668.aspx">Using a Windows version of GNU Patch.exe with CVS and Diff Files</a>&#8221;.

</p>
<h2>For More Information</h2>
<ul>
<li>Be sure to read Eric Sink's <a href="http://software.ericsink.com/scm/source_control.html" target="_blank">Source Control HOWTO</a>.</li>
<li>Also check out the SourceForge <a href="http://sourceforge.net/docman/?group_id=1" target="_blank">site docs</a>. Including...</li>
<li><ul>
<li><a href="http://sourceforge.net/docman/display_doc.php?docid=761&amp;group_id=1" target="_blank">Guide to Generation and Posting of SSH Keys</a></li>
<li><a href="http://sourceforge.net/docman/display_doc.php?docid=14033&amp;group_id=1#top" target="_blank">Basic Introduction to CVS and SourceForge.net (SF.net) Project CVS Services</a></li>

<li><a href="http://sourceforge.net/docman/display_doc.php?docid=768&amp;group_id=1" target="_blank">Introduction to SourceForge.net Project CVS Services for Developers</a></li>
<li><a href="http://sourceforge.net/docman/display_doc.php?docid=766&amp;group_id=1" target="_blank">WinCvs CVS Client Installation Instructions</a></li>
</ul></li>
</ul>
<h2>Conclusion</h2>
<p>
I hope this gets you on your feet when joining an open source project in SourceForge.  If you find any errors, omissions, and such, please let me know so I can correct it.
</p>
	
	
</MP:MasterPage>