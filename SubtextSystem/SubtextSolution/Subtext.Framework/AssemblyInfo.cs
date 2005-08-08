using System;
using System.Reflection;
using System.Runtime.InteropServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("Subtext.Framework")]
[assembly: AssemblyDescription("Contains the core business logic for Subtext.")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]

[assembly: log4net.Config.XmlConfigurator( ConfigFile="Log4Net.config",Watch=true )]
