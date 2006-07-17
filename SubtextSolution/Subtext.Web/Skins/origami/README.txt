Origami Skin for SubText 1.5.1.0
Original Design by www.leevigraham.com for Typo and WordPress
Adapted and configured for SubText by timheuer.com/smilinggoat.net

Installing the Origami Skin
as of 28 JUN 2006
=============================

This package contains several files:

1)	origami.zip
	This is the actual skin files
2)	skinsConfig.xml
	This is the <SkinTemplate> node you'll need
3)	This readme :-)
	This readme contains several additional steps you'll need to do.
	

NOTE: 	%SUBTEXT% in this document refers to *your* installation location of 
		the SubText application files

Step 1: Unzip the origami.zip files into your 
			%SUBTEXT%\Skins directory

Step 2: In your %SUBTEXT%\Admin\Skins.config you'll want to add the node
		identified in the skinsConfig.xml file included here -- this is required
		
Step 3: In the PageTemplate.ascx file you'll notice some if IE statements to add IE hacked 
        stylesheets.  Until SubText is updated to include conditional stylesheets, you'll want to 
        hard-code the path to these two style sheets for them to be used.
		
Step 5:	Go to your admin and choose the style!

// TODO:
1) Conditional stylesheets
2) Put CAPTCHA on comments page