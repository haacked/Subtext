using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Moq;

namespace UnitTests.Subtext.Framework.Skinning
{
    public static class SkinTestExtensions
    {
        public static void SetupSkin(this Mock<VirtualPathProvider> vppMock, IList<VirtualDirectory> directories,
                                     string skinName, string skinConfigContents)
        {
            string skinConfigPath = string.Format(CultureInfo.InvariantCulture, "~/skins/{0}/skin.config", skinName);
            var virtualFile = new Mock<VirtualFile>(skinConfigPath);
            Stream stream = skinConfigContents.ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);
            vppMock.Setup(v => v.FileExists(skinConfigPath)).Returns(true);
            vppMock.Setup(v => v.GetFile(skinConfigPath)).Returns(virtualFile.Object);
            var skinDir =
                new Mock<VirtualDirectory>(string.Format(CultureInfo.InvariantCulture, "~/skins/{0}", skinName));
            skinDir.Setup(d => d.Name).Returns(skinName);
            directories.Add(skinDir.Object);
        }

        public static VirtualPathProvider SetupSkins(this Mock<VirtualPathProvider> vppMock)
        {
            string piyoConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Piyo"" TemplateFolder=""Piyo"" StyleMergeMode=""MergedAfter"" ScriptMergeMode=""Merge"">
	    <Scripts>
		    <Script Src=""Scripts/piyo.js"" />
            <Script Src=""~/scripts/prototype.js"" />
            <Script Src=""~/scripts/scriptaculous.js?load=effects"" />
            <Script Src=""~/scripts/lightbox.js"" />
	    </Scripts>
	    <Styles>
		    <Style href=""~/skins/_System/csharp.css"" />
		    <Style href=""~/skins/_System/commonstyle.css"" />
		    <Style href=""~/skins/_System/commonlayout.css"" />
            <Style href=""~/css/lightbox.css"" media=""screen"" />
		    <Style title=""fixed"" href=""piyo-fixed.css"" media=""screen""/>
            <Style title=""elastic"" href=""piyo-elastic.css"" media=""screen""/>
		    <Style href=""print.css"" media=""print"" />
	    </Styles>
    </SkinTemplate>
</SkinTemplates>";

            string semagogyConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Semagogy"" TemplateFolder=""Semagogy"" ScriptMergeMode=""DontMerge"" StyleMergeMode=""MergedFirst"" >
        <Scripts>
            <Script Src=""Scripts/DarkHorseLayoutEngine.js"" />
        </Scripts>
        <Styles>
            <Style href=""~/skins/_System/csharp.css"" />
            <Style href=""~/skins/_System/commonstyle.css"" />
            <Style href=""~/skins/_System/commonlayout.css"" />
            <Style href=""print.css"" media=""print"" />
        </Styles>
    </SkinTemplate>
</SkinTemplates>";

            string redbookConfig =
                @"<SkinTemplates>
	<SkinTemplate Name=""RedBook"" TemplateFolder=""RedBook"" StyleSheet=""Red.css"" ScriptMergeMode=""Merge"" StyleMergeMode=""None"">
		<Scripts>
			<Script Src=""~/scripts/niceforms.js"" />
            <Script Src=""http://www.google.com/adsense.js"" />
		</Scripts>
		<Styles>
			<Style href=""~/skins/_System/csharp.css"" />
			<Style href=""~/skins/_System/commonstyle.css"" />
			<Style href=""~/skins/_System/commonlayout.css"" />
			<Style href=""niceforms-default.css"" />
			<Style href=""print.css"" media=""print"" />
		</Styles>
	</SkinTemplate>

    <SkinTemplate Name=""BlueBook"" TemplateFolder=""RedBook"" StyleSheet=""Blue.css"" ExcludeDefaultStyle=""true"" ScriptMergeMode=""Merge"">
	    <Scripts>
		    <Script Src=""~/scripts/niceforms.js"" />
	    </Scripts>
	    <Styles>
		    <Style href=""~/skins/_System/csharp.css"" />
		    <Style href=""~/skins/_System/commonstyle.css"" />
		    <Style href=""~/skins/_System/commonlayout.css"" />
		    <Style href=""niceforms-default.css"" />
		    <Style href=""IEHacks.css"" conditional=""if IE"" />
		    <Style href=""print.css"" media=""print"" />
	    </Styles>
    </SkinTemplate>

    <SkinTemplate Name=""GreenBook"" TemplateFolder=""RedBook"" StyleSheet=""Green.css"" StyleMergeMode=""MergedAfter"">
	    <Scripts>
		    <Script Src=""~/scripts/niceforms.js"" />
		    <Script Src=""blah.js"" />
	    </Scripts>
	    <Styles>
		    <Style href=""~/skins/_System/csharp.css"" />
		    <Style href=""~/skins/_System/commonstyle.css"" />
		    <Style href=""~/skins/_System/commonlayout.css"" />
		    <Style href=""niceforms-default.css"" />
            <Style href=""IEHacks.css"" conditional=""if IE"" />
		    <Style href=""print.css"" media=""print"" />
	    </Styles>
    </SkinTemplate>
</SkinTemplates>";

            string natureConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Leafy"" TemplateFolder=""Nature"" StyleSheet=""leafy.css"" StyleMergeMode=""MergedAfter"">
        <Styles>
            <Style href=""~/css/lightbox.css"" media=""screen"" />
            <Style href=""~/scripts/XFNHighlighter.css"" />
            <Style href=""~/skins/_System/csharp.css"" />
            <Style href=""~/skins/_System/commonstyle.css"" />
            <Style href=""~/skins/_System/commonlayout.css"" />
            <Style href=""print.css"" media=""print"" />
        </Styles>
        <Scripts>
            <Script Src=""~/scripts/XFNHighlighter.js"" />
            <Script Src=""~/scripts/lightbox.js"" />
        </Scripts>
    </SkinTemplate>
</SkinTemplates>";

            string gradientConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Gradient"" TemplateFolder=""Gradient"" ExcludeDefaultStyle=""false"" StyleMergeMode=""MergedAfter"" ScriptMergeMode=""Merge"">
		<Styles>
			<Style href=""~/skins/_System/csharp.css"" />
			<Style href=""~/skins/_System/commonstyle.css"" />
			<Style href=""~/skins/_System/commonlayout.css"" />
			<Style href=""IEPatches.css"" conditional=""if IE"" media=""screen"" />
			<Style href=""print.css"" media=""print"" />
		</Styles>
	</SkinTemplate>
</SkinTemplates>";

            string wpConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""WPSkin"" TemplateFolder=""WPSkin"" StyleMergeMode=""None"" ExcludeDefaultStyle=""true"" >
		<Styles>
			<Style href=""~/skins/_System/csharp.css"" />
			<Style href=""~/skins/_System/commonstyle.css"" />
			<Style href=""~/skins/_System/commonlayout.css"" />
			<Style href=""print.css"" media=""print"" />
		</Styles>
	</SkinTemplate>
</SkinTemplates>";

            string submarineConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Submarine"" TemplateFolder=""Submarine"" StyleMergeMode=""MergedAfter"" ScriptMergeMode=""Merge"">
		<Scripts>
			<Script Src=""~/scripts/niceforms.js"" />
			<Script Src=""~/scripts/lightbox.js"" />
			<Script Src=""~/scripts/XFNHighlighter.js"" />
			<Script Src=""~/scripts/ExternalLinks.js"" />
			<Script Src=""~/scripts/LiveCommentPreview.js"" />
			<Script Src=""~/scripts/AmazonTooltips.js"" />
		</Scripts>
		<Styles>
			<Style href=""~/skins/_System/csharp.css"" />
			<Style href=""~/skins/_System/commonstyle.css"" />
			<Style href=""~/skins/_System/commonlayout.css"" />
			<Style href=""~/scripts/XFNHighlighter.css"" />
			<Style href=""~/css/lightbox.css"" />
			<Style href=""niceforms-default.css"" media=""all"" conditional=""if IE""/>
            <Style href=""IEHacks.css"" conditional=""if IE"" />
			<Style href=""print.css"" media=""print"" />
		</Styles>
	</SkinTemplate>
</SkinTemplates>";

            string origamiConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Origami"" TemplateFolder=""Origami"" StyleMergeMode=""MergedAfter"" ScriptMergeMode=""Merge"">
        <Styles>
            <Style href=""~/skins/_System/csharp.css"" />
            <Style href=""~/skins/_System/commonstyle.css"" />
            <Style href=""~/skins/_System/commonlayout.css"" />
            <Style href=""Styles/user-styles.css"" media=""all"" />
            <Style href=""Styles/print.css"" media=""print"" />
            <Style href=""Styles/core.css"" media=""screen"" />
            <Style href=""Styles/tables.css"" media=""screen"" />
        </Styles>
        <Scripts>
            <Script Src=""Scripts/cookies.js"" />
            <Script Src=""Scripts/prototype.js"" Defer=""true""/>
            <Script Src=""Scripts/effects.js"" />
            <Script Src=""Scripts/init.js"" />
            <Script Src=""Scripts/behaviour.js"" />
            <Script Src=""Scripts/coreFunctions.js"" />
            <Script Src=""Scripts/styleSwitcher.js"" />
        </Scripts>
    </SkinTemplate>
</SkinTemplates>";

            string anotherEonConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""AnotherEon001"" TemplateFolder=""AnotherEon001"" StyleMergeMode=""MergedAfter"">
	    <Styles>
		    <Style href=""~/skins/_System/csharp.css"" />
		    <Style href=""http://haacked.com/skins/_System/commonstyle.css"" />
		    <Style href=""~/skins/_System/commonlayout.css"" />
		    <Style href=""print.css"" media=""print"" />
	    </Styles>
    </SkinTemplate>
</SkinTemplates>";

            string keyWestConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""KeyWest"" TemplateFolder=""KeyWest"" StyleMergeMode=""MergedFirst"">
        <Styles>
            <Style href=""~/skins/_System/csharp.css"" />
            <Style href=""~/skins/_System/commonstyle.css"" />
            <Style href=""~/skins/_System/commonlayout.css"" />
            <Style href=""print.css"" media=""print"" />
        </Styles>
    </SkinTemplate>
</SkinTemplates>";

            string lightZConfig =
                @"<SkinTemplates>
    <SkinTemplate Name=""Lightz"" TemplateFolder=""Lightz"" StyleSheet=""light.css"" StyleMergeMode=""None"">
		<Styles>
			<Style href=""~/skins/_System/csharp.css"" />
			<Style href=""~/skins/_System/commonstyle.css"" />
			<Style href=""~/skins/_System/commonlayout.css"" />
			<Style href=""print.css"" media=""print"" />
		</Styles>
	</SkinTemplate>
</SkinTemplates>";

            var directories = new List<VirtualDirectory>();
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            vppMock.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);

            vppMock.SetupSkin(directories, "Piyo", piyoConfig);
            vppMock.SetupSkin(directories, "Semagogy", semagogyConfig);
            vppMock.SetupSkin(directories, "RedBook", redbookConfig);
            vppMock.SetupSkin(directories, "Nature", natureConfig);
            vppMock.SetupSkin(directories, "Gradient", gradientConfig);
            vppMock.SetupSkin(directories, "WPSkin", wpConfig);
            vppMock.SetupSkin(directories, "Submarine", submarineConfig);
            vppMock.SetupSkin(directories, "Origami", origamiConfig);
            vppMock.SetupSkin(directories, "AnotherEon001", anotherEonConfig);
            vppMock.SetupSkin(directories, "KeyWest", keyWestConfig);
            vppMock.SetupSkin(directories, "lightz", lightZConfig);
            return vppMock.Object;
        }
    }
}