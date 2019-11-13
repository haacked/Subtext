using System;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Microsoft.Design", "CA1050:DeclareTypesInNamespaces")]
[AttributeUsage(AttributeTargets.All)]
public sealed class CoverageExcludeAttribute : Attribute
{
}