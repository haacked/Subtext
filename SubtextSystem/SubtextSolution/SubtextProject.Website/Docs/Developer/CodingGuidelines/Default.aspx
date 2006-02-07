<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>C# Coding Style Guide</h2>
	
	<h3>Preface</h3>
	<p>
	From developer to developer and project to project, coding styles vary 
	as much as personal fashion.  There is no &#8220;One True Way&#8221; when 
	it comes to a coding style.
	</p>
	<p>
	Unfortunately, while it would be wonderful to embrace all the unique styles 
	individuals bring to a project, it is just not feasible.  Some semblance of 
	conformity must be imposed for the sake of consistency and readability.
	</p>
	<p>
	Choosing a particular coding style is a delicate proposition as many religious 
	wars are fiercely battled over such trivial arguments such as whether to use 
	spaces or tabs for indentation.
	</p>
	<p>
	But choose we must, because should one developer use tabs, and another use 
	spaces, when they view each other&#8217;s code, it will be a mess.
	</p>
	<p>
	Hence these are the arbitrary guidelines we use on Subtext.  This is not a 
	statement on which style others should use in their own code, it is simply 
	the guidelines we choose to follow on Subtext in order to remain consistent.  
	Should a new developer ask why we chose this style, the answer will be 
	&#8220;It has always been this way. It is the way we do it.&#8221;
	</p>
	<h3>In a Nutshell</h3>
	<p>
	In order to avoid religious wars, I have tried to follow established guidelines 
	from two sources with a few exceptions.  These sources in particular focus on 
	public APIs.
	</p>
	<p>
	For public facing code, we will follow all the guidelines specified by:
	</p>
	<ul>
		<li><a href="http://www.gotdotnet.com/team/fxcop/" title="FxCop Website" rel="external">FxCop</a></li>
		<li><a href="http://www.amazon.com/gp/product/0321246756/002-3753862-9190411?v=glance&amp;n=283155" title="Book for sale on Amazon" rel="external">Framework Design Guidelines - Conventions, Idioms, and Patterns for Reusable .NET Libraries</a></li>
	</ul>
	<p>
	These sources do not cover internal style that is not public facing.  
	However, for the most part the same casing and naming conventions that 
	apply to public facing code will apply to internal code.
	</p>
	<p>
	However, we will follow the following conventions as well.
	</p>
	<ul>
		<li>Curly braces will be placed on their own line</li>
		<li>
		Do not prefix private member variables. Instead use the <code>this</code> keyword 
		when referencing a private member.
<pre>public class SomeClass
{
	private string <strong>someMember</strong>;
	
	public SomeClass(string someMember)
	{
		this.someMember = someMember;
	}
}</pre>
		</li>
	</ul>
</MP:MasterPage>