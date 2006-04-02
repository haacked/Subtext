For more information on how to use Subversion with SourceForge projects, 
please read http://haacked.com/archive/2005/05/12/3178.aspx

Source Control Rules
=========================

The rules of engagement for the source control system are simple, 
but we will kick out anyone (independent of celebrity or other 
honorary status) who does not follow them. 

0.  If you are not sure whether you should/could/may add or change something, 
	ask a project administrator. Behave like a professional software developer, 
	please -- even if you aren't one. 

1.	Please be sure to understand the principles of Source Control 
	(http://software.ericsink.com/scm/source_control.html) and how CVS works.

2.  Every "official release" should be Tagged (Labelled) with the version number.

3.  If you want to make changes, UPDATE (Get Latest) the whole source tree first 
	and then start modifying select files. If you added new files, ensure to get 
	them ADDED and then COMMITED to CVS!
    After checking in your changes, wipe your working/merge directory, re-UPDATE 
    the tree and recompile locally. If that breaks, some files are missing, fix 
    the problem.
  3.1	If you have COMMIT privilages, please read Docs/subText_CVSGuidelines.txt 
		before you make any commits. This document explains how TAGs & BRANCHes 
		for the subText project will be handled/named.

4.	If you want to add a feature or fix a bug, we expect that you know what you 
	are doing. 

5.	Checked-In items must compile, work as expected and add features, never remove 
	features (unless removing the feature is an assigned task).

6.	Any new feature that you add should be configurable.  Ideally via the web admin 
	tool.

7.	If you have GPL code in your pocket coming in, leave it at the door, please. 

8.	This list may grow.

9.	Make sure to CVS Ignore "bin" and "obj" directories as well as any *.user 
	and *.suo files.
	
10. Please provide descriptive comments of EVERY check-in.