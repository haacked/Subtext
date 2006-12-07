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

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Scope = "assembly", Justification = "Assemblies are not currently being signed.")]
