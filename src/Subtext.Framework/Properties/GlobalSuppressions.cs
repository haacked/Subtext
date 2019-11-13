// ---------------------------------------------------------------------------
// GlobalSuppressions.cs
//
// Provides assembly level (global) CodeAnalysis suppressions for FxCop.
//
// While static code analysis with FxCop is excellent for catching many common
// and not so common code errors, there are some things that it flags that
// do not always apply to the project at hand. For those cases, FxCop allows
// you to exclude the message (and optionally give a justification reason for
// excluding it). However, those exclusions are stored only in the FxCop
// project file. In the 2.0 version of the .NET framework, Microsoft introduced
// SuppressMessageAttribute, which is used primarily by the version of FxCop
// that is built in to Visual Studio. As this built-in functionality is not
// included in all versions of Visual Studio, we have opted to continue
// using the standalone version of FxCop. 
//
// In order for this version to recognize SupressMessageAttribute, the
// CODE_ANALYSIS symbol must be defined.
//
// ---------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Scope = "assembly",
        Justification = "Assemblies are not currently being signed.")]

// FxCop says that namespaces should generally have more than five types.
// Unfortunately, not all of these namespaces currently have more than five
// types but we still want the namespace so we can expand the library in the
// future without moving types around. 

#region CA1020:AvoidNamespacesWithFewTypes

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Web.Handlers",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.ImportExport.Conversion",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.ImportExport",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Providers",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Syndication.Compression",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Format",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Web",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Services",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Text",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Security",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Web.HttpModules",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Web.HttpModules",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.UI",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Net",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "Subtext.Framework.Search",
        Justification =
            "Ignoring this warning...we want this namespace, but don't have enough classes to go in it right now to satisfy the rule."
        )]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Subtext.Scripting.Exceptions")]
#endregion

// We could use a CustomDictionary.xml file to handle the spelling and case
// rules, but VS2005 Code Analysis doesn't support them and the FxCop add-ins
// and custom external tools don't rely on a project file, so we can't specify
// the location of the CustomDictionary.xml file. We don't want to modify the
// default file that ships with the FxCop distribution either. This does make
// more work for us, but it is the safest solution.

#region CA1704:IdentifiersShouldBeSpelledCorrectly

//[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Subtext.BlogML.Interfaces.IBlogMLProvider.CreatePostTrackback(BlogML.Xml.BlogMLTrackback,System.String):System.Void", MessageId = "0#trackback")]
//[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Subtext.BlogML.Interfaces.IBlogMLProvider.CreatePostTrackback(BlogML.Xml.BlogMLTrackback,System.String):System.Void", MessageId = "Trackback")]

#endregion