Although SubtextBuildSetup is a C# class library, it's intended to house the WiX 
setup project for Subtext as well as any Nant files.

Unfortunately Votive (VS add-in and templates for WiX) isn't mature enough yet, 
which is why we rely on this Haack.  The SubtextSolution is intended to be complete.

We'll use a Post Build step to run the WiX commands necessary 
to build the various files.