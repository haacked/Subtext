<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Configure"
    CodeBehind="Configure.aspx.cs" Inherits="Subtext.Web.Admin.Pages.Configure" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <script type="text/javascript">
        $(function () {
            $('select.timezone').change(function () {
                var timeZone = $(this).val();
                ajaxServices.getTimeZoneInfo(timeZone, function (response) {
                    if (response.error)
                    //TODO: Handle this more gracefully.
                        alert(response.error);
                    else {
                        $("#timeZoneInfo").effect('highlight', {}, 1000);
                        $("#serverTimeZone").html(response.result.serverTimeZone);
                        $("#serverTime").html(response.result.serverTime);
                        $("#serverUtcTime").html(response.result.serverUtcTime);
                        $("#currentTime").html(response.result.currentTime);
                    }
                });

            });
        });
    </script>

    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel ID="Messages" runat="server" />
    <h2>Configure</h2>
    <div class="Edit" id="configure-form">
        <asp:Panel runat="server" GroupingText="Main Settings" CssClass="options">
            <label accesskey="t" for="Edit_txbTitle">
                <u>T</u>itle</label>
            <asp:TextBox ID="txbTitle" runat="server" CssClass="textbox" />
            <label accesskey="s" for="Edit_txbSubtitle">
                <u>S</u>ubtitle</label>
            <asp:TextBox ID="txbSubtitle" runat="server" CssClass="textbox" />
            <label accesskey="u" for="Edit_txbUser">
                <u>U</u>sername</label>
            <asp:TextBox ID="txbUser" runat="server" CssClass="textbox" />
            <label accesskey="n" for="Edit_txbAuthor">
                Owner's Display <u>N</u>ame</label>
            <asp:TextBox ID="txbAuthor" runat="server" CssClass="textbox" />
            <label accesskey="e" for="Edit_txbAuthorEmail">
                Owner's <u>E</u>mail</label>
            <asp:TextBox ID="txbAuthorEmail" runat="server" CssClass="textbox" />            
            <div>
                <asp:CheckBox ID="ckbShowEmailonRssFeed" runat="server" TextAlign="Right" CssClass="checkbox" AccessKey="r" Text="Show email address on <u>R</u>SS Feed" />
            </div>
            <div>
                <asp:CheckBox ID="ckbAllowServiceAccess" runat="server" TextAlign="Right" CssClass="checkbox" AccessKey="w"
                    Text="Allow <u>W</u>eb Service Access" />
            </div>
            <div>
	            <asp:CheckBox id="chkAutoGenerate" runat="server" TextAlign="Right" Text="Auto-Generate Friendly Url" CssClass="checkbox" AccessKey="g" />
		        <st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, blog posts and articles will have friendly Urls auto-generated based on the title. For example, the title 'My Blog Post' will become 'my-blog-post.aspx'.">
			            <img src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information" />
		        </st:HelpToolTip>
	        </div>

        </asp:Panel>
        <asp:Panel runat="server" GroupingText="Location Settings" CssClass="options wide">
            <p>
                <label accesskey="z" for="Edit_ddlTimezone">
                    Your Time<u>z</u>one
                    <st:HelpToolTip ID="hlpTimeZone" runat="server" HelpText="Select your timezone, which may differ from the timezone where your blog server is located."
                        ImageUrl="~/images/icons/help-small.png" ImageWidth="16" ImageHeight="16" />
                </label>
                <asp:DropDownList ID="ddlTimezone" runat="server" CssClass="wide-dropdown timezone" />
            </p>
            <div id="timeZoneInfo">
                <em>Time at selected timezone is: 
                    <strong id="currentTime"><asp:Literal ID="lblCurrentTime" runat="server" /></strong></em><br />
                <div>
                    <em>Time at server is: 
                    <strong><span id="serverTime"><asp:Literal ID="lblServerTime" runat="server" /></span></strong></em>
                    </div>
                <div>
                    <em>Server timezone is 
                        <span id="serverTimeZone"><asp:Literal ID="lblServerTimeZone" runat="server" /></span></em>
                </div>
                <div>
                    <em>
                    <acronym title="Coordinated Universal Time">UTC</acronym> time is
                    <span id="serverUtcTime"><asp:Literal ID="lblUtcTime" runat="server" /></span>
                    </em>
                </div>
            </div>
            <label accesskey="l" for="Edit_ddlLangLocale">
                <u>L</u>anguage/Locale</label>
            <asp:DropDownList ID="ddlLangLocale" runat="server">
                <asp:ListItem Text="Afrikaans" Value="af" />
                <asp:ListItem Text="Afrikaans - South Africa" Value="af-ZA" />
                <asp:ListItem Text="Albanian" Value="sq" />
                <asp:ListItem Text="Albanian - Albania" Value="sq-AL" />
                <asp:ListItem Text="Arabic" Value="ar" />
                <asp:ListItem Text="Arabic - Algeria" Value="ar-DZ" />
                <asp:ListItem Text="Arabic - Bahrain" Value="ar-BH" />
                <asp:ListItem Text="Arabic - Egypt" Value="ar-EG" />
                <asp:ListItem Text="Arabic - Iraq" Value="ar-IQ" />
                <asp:ListItem Text="Arabic - Jordan" Value="ar-JO" />
                <asp:ListItem Text="Arabic - Kuwait" Value="ar-KW" />
                <asp:ListItem Text="Arabic - Lebanon" Value="ar-LB" />
                <asp:ListItem Text="Arabic - Libya" Value="ar-LY" />
                <asp:ListItem Text="Arabic - Morocco" Value="ar-MA" />
                <asp:ListItem Text="Arabic - Oman" Value="ar-OM" />
                <asp:ListItem Text="Arabic - Qatar" Value="ar-QA" />
                <asp:ListItem Text="Arabic - Saudi Arabia" Value="ar-SA" />
                <asp:ListItem Text="Arabic - Syria" Value="ar-SY" />
                <asp:ListItem Text="Arabic - Tunisia" Value="ar-TN" />
                <asp:ListItem Text="Arabic - United Arab Emirates" Value="ar-AE" />
                <asp:ListItem Text="Arabic - Yemen" Value="ar-YE" />
                <asp:ListItem Text="Armenian" Value="hy" />
                <asp:ListItem Text="Armenian - Armenia" Value="hy-AM" />
                <asp:ListItem Text="Azeri" Value="az" />
                <asp:ListItem Text="Azeri (Cyrillic) - Azerbaijan" Value="Cy-az-AZ" />
                <asp:ListItem Text="Azeri (Latin) - Azerbaijan" Value="Lt-az-AZ" />
                <asp:ListItem Text="Basque" Value="eu" />
                <asp:ListItem Text="Basque - Basque" Value="eu-ES" />
                <asp:ListItem Text="Belarusian" Value="be" />
                <asp:ListItem Text="Belarusian - Belarus" Value="be-BY" />
                <asp:ListItem Text="Bulgarian" Value="bg" />
                <asp:ListItem Text="Bulgarian - Bulgaria" Value="bg-BG" />
                <asp:ListItem Text="Catalan" Value="ca" />
                <asp:ListItem Text="Catalan - Catalan" Value="ca-ES" />
                <asp:ListItem Text="Chinese - Hong Kong SAR" Value="zh-HK" />
                <asp:ListItem Text="Chinese - Macau SAR" Value="zh-MO" />
                <asp:ListItem Text="Chinese - China" Value="zh-CN" />
                <asp:ListItem Text="Chinese (Simplified)" Value="zh-CHS" />
                <asp:ListItem Text="Chinese - Singapore" Value="zh-SG" />
                <asp:ListItem Text="Chinese - Taiwan" Value="zh-TW" />
                <asp:ListItem Text="Chinese (Traditional)" Value="zh-CHT" />
                <asp:ListItem Text="Croatian" Value="hr" />
                <asp:ListItem Text="Croatian - Croatia" Value="hr-HR" />
                <asp:ListItem Text="Czech" Value="cs" />
                <asp:ListItem Text="Czech - Czech Republic" Value="cs-CZ" />
                <asp:ListItem Text="Danish" Value="da" />
                <asp:ListItem Text="Danish - Denmark" Value="da-DK" />
                <asp:ListItem Text="Dhivehi" Value="div" />
                <asp:ListItem Text="Dhivehi - Maldives" Value="div-MV" />
                <asp:ListItem Text="Dutch" Value="nl" />
                <asp:ListItem Text="Dutch - Belgium" Value="nl-BE" />
                <asp:ListItem Text="Dutch - The Netherlands" Value="nl-NL" />
                <asp:ListItem Text="English" Value="en" />
                <asp:ListItem Text="English - Australia" Value="en-AU" />
                <asp:ListItem Text="English - Belize" Value="en-BZ" />
                <asp:ListItem Text="English - Canada" Value="en-CA" />
                <asp:ListItem Text="English - Caribbean" Value="en-CB" />
                <asp:ListItem Text="English - Ireland" Value="en-IE" />
                <asp:ListItem Text="English - Jamaica" Value="en-JM" />
                <asp:ListItem Text="English - New Zealand" Value="en-NZ" />
                <asp:ListItem Text="English - Philippines" Value="en-PH" />
                <asp:ListItem Text="English - South Africa" Value="en-ZA" />
                <asp:ListItem Text="English - Trinidad and Tobago" Value="en-TT" />
                <asp:ListItem Text="English - United Kingdom" Value="en-GB" />
                <asp:ListItem Text="English - United States" Value="en-US" />
                <asp:ListItem Text="English - Zimbabwe" Value="en-ZW" />
                <asp:ListItem Text="Estonian" Value="et" />
                <asp:ListItem Text="Estonian - Estonia" Value="et-EE" />
                <asp:ListItem Text="Faroese" Value="fo" />
                <asp:ListItem Text="Faroese - Faroe Islands" Value="fo-FO" />
                <asp:ListItem Text="Farsi" Value="fa" />
                <asp:ListItem Text="Farsi - Iran" Value="fa-IR" />
                <asp:ListItem Text="Finnish" Value="fi" />
                <asp:ListItem Text="Finnish - Finland" Value="fi-FI" />
                <asp:ListItem Text="French" Value="fr" />
                <asp:ListItem Text="French - Belgium" Value="fr-BE" />
                <asp:ListItem Text="French - Canada" Value="fr-CA" />
                <asp:ListItem Text="French - France" Value="fr-FR" />
                <asp:ListItem Text="French - Luxembourg" Value="fr-LU" />
                <asp:ListItem Text="French - Monaco" Value="fr-MC" />
                <asp:ListItem Text="French - Switzerland" Value="fr-CH" />
                <asp:ListItem Text="Galician" Value="gl" />
                <asp:ListItem Text="Galician - Galician" Value="gl-ES" />
                <asp:ListItem Text="Georgian" Value="ka" />
                <asp:ListItem Text="Georgian - Georgia" Value="ka-GE" />
                <asp:ListItem Text="German" Value="de" />
                <asp:ListItem Text="German - Austria" Value="de-AT" />
                <asp:ListItem Text="German - Germany" Value="de-DE" />
                <asp:ListItem Text="German - Liechtenstein" Value="de-LI" />
                <asp:ListItem Text="German - Luxembourg" Value="de-LU" />
                <asp:ListItem Text="German - Switzerland" Value="de-CH" />
                <asp:ListItem Text="Greek" Value="el" />
                <asp:ListItem Text="Greek - Greece" Value="el-GR" />
                <asp:ListItem Text="Gujarati" Value="gu" />
                <asp:ListItem Text="Gujarati - India" Value="gu-IN" />
                <asp:ListItem Text="Hebrew" Value="he" />
                <asp:ListItem Text="Hebrew - Israel" Value="he-IL" />
                <asp:ListItem Text="Hindi" Value="hi" />
                <asp:ListItem Text="Hindi - India" Value="hi-IN" />
                <asp:ListItem Text="Hungarian" Value="hu" />
                <asp:ListItem Text="Hungarian - Hungary" Value="hu-HU" />
                <asp:ListItem Text="Icelandic" Value="is" />
                <asp:ListItem Text="Icelandic - Iceland" Value="is-IS" />
                <asp:ListItem Text="Indonesian" Value="id" />
                <asp:ListItem Text="Indonesian - Indonesia" Value="id-ID" />
                <asp:ListItem Text="Italian" Value="it" />
                <asp:ListItem Text="Italian - Italy" Value="it-IT" />
                <asp:ListItem Text="Italian - Switzerland" Value="it-CH" />
                <asp:ListItem Text="Japanese" Value="ja" />
                <asp:ListItem Text="Japanese - Japan" Value="ja-JP" />
                <asp:ListItem Text="Kannada" Value="kn" />
                <asp:ListItem Text="Kannada - India" Value="kn-IN" />
                <asp:ListItem Text="Kazakh" Value="kk" />
                <asp:ListItem Text="Kazakh - Kazakhstan" Value="kk-KZ" />
                <asp:ListItem Text="Konkani" Value="kok" />
                <asp:ListItem Text="Konkani - India" Value="kok-IN" />
                <asp:ListItem Text="Korean" Value="ko" />
                <asp:ListItem Text="Korean - Korea" Value="ko-KR" />
                <asp:ListItem Text="Kyrgyz" Value="ky" />
                <asp:ListItem Text="Kyrgyz - Kazakhstan" Value="ky-KZ" />
                <asp:ListItem Text="Latvian" Value="lv" />
                <asp:ListItem Text="Latvian - Latvia" Value="lv-LV" />
                <asp:ListItem Text="Lithuanian" Value="lt" />
                <asp:ListItem Text="Lithuanian - Lithuania" Value="lt-LT" />
                <asp:ListItem Text="Macedonian" Value="mk" />
                <asp:ListItem Text="Macedonian - FYROM" Value="mk-MK" />
                <asp:ListItem Text="Malay" Value="ms" />
                <asp:ListItem Text="Malay - Brunei" Value="ms-BN" />
                <asp:ListItem Text="Malay - Malaysia" Value="ms-MY" />
                <asp:ListItem Text="Marathi" Value="mr" />
                <asp:ListItem Text="Marathi - India" Value="mr-IN" />
                <asp:ListItem Text="Mongolian" Value="mn" />
                <asp:ListItem Text="Mongolian - Mongolia" Value="mn-MN" />
                <asp:ListItem Text="Norwegian" Value="no" />
                <asp:ListItem Text="Norwegian (Bokmål) - Norway" Value="nb-NO" />
                <asp:ListItem Text="Norwegian (Nynorsk) - Norway" Value="nn-NO" />
                <asp:ListItem Text="Polish" Value="pl" />
                <asp:ListItem Text="Polish - Poland" Value="pl-PL" />
                <asp:ListItem Text="Portuguese" Value="pt" />
                <asp:ListItem Text="Portuguese - Brazil" Value="pt-BR" />
                <asp:ListItem Text="Portuguese - Portugal" Value="pt-PT" />
                <asp:ListItem Text="Punjabi" Value="pa" />
                <asp:ListItem Text="Punjabi - India" Value="pa-IN" />
                <asp:ListItem Text="Romanian" Value="ro" />
                <asp:ListItem Text="Romanian - Romania" Value="ro-RO" />
                <asp:ListItem Text="Russian" Value="ru" />
                <asp:ListItem Text="Russian - Russia" Value="ru-RU" />
                <asp:ListItem Text="Sanskrit" Value="sa" />
                <asp:ListItem Text="Sanskrit - India" Value="sa-IN" />
                <asp:ListItem Text="Serbian (Cyrillic) - Serbia" Value="Cy-sr-SP" />
                <asp:ListItem Text="Serbian (Latin) - Serbia" Value="Lt-sr-SP" />
                <asp:ListItem Text="Slovak" Value="sk" />
                <asp:ListItem Text="Slovak - Slovakia" Value="sk-SK" />
                <asp:ListItem Text="Slovenian" Value="sl" />
                <asp:ListItem Text="Slovenian - Slovenia" Value="sl-SI" />
                <asp:ListItem Text="Spanish" Value="es" />
                <asp:ListItem Text="Spanish - Argentina" Value="es-AR" />
                <asp:ListItem Text="Spanish - Bolivia" Value="es-BO" />
                <asp:ListItem Text="Spanish - Chile" Value="es-CL" />
                <asp:ListItem Text="Spanish - Colombia" Value="es-CO" />
                <asp:ListItem Text="Spanish - Costa Rica" Value="es-CR" />
                <asp:ListItem Text="Spanish - Dominican Republic" Value="es-DO" />
                <asp:ListItem Text="Spanish - Ecuador" Value="es-EC" />
                <asp:ListItem Text="Spanish - El Salvador" Value="es-SV" />
                <asp:ListItem Text="Spanish - Guatemala" Value="es-GT" />
                <asp:ListItem Text="Spanish - Honduras" Value="es-HN" />
                <asp:ListItem Text="Spanish - Mexico" Value="es-MX" />
                <asp:ListItem Text="Spanish - Nicaragua" Value="es-NI" />
                <asp:ListItem Text="Spanish - Panama" Value="es-PA" />
                <asp:ListItem Text="Spanish - Paraguay" Value="es-PY" />
                <asp:ListItem Text="Spanish - Peru" Value="es-PE" />
                <asp:ListItem Text="Spanish - Puerto Rico" Value="es-PR" />
                <asp:ListItem Text="Spanish - Spain" Value="es-ES" />
                <asp:ListItem Text="Spanish - Uruguay" Value="es-UY" />
                <asp:ListItem Text="Spanish - Venezuela" Value="es-VE" />
                <asp:ListItem Text="Swahili" Value="sw" />
                <asp:ListItem Text="Swahili - Kenya" Value="sw-KE" />
                <asp:ListItem Text="Swedish" Value="sv" />
                <asp:ListItem Text="Swedish - Finland" Value="sv-FI" />
                <asp:ListItem Text="Swedish - Sweden" Value="sv-SE" />
                <asp:ListItem Text="Syriac" Value="syr" />
                <asp:ListItem Text="Syriac - Syria" Value="syr-SY" />
                <asp:ListItem Text="Tamil" Value="ta" />
                <asp:ListItem Text="Tamil - India" Value="ta-IN" />
                <asp:ListItem Text="Tatar" Value="tt" />
                <asp:ListItem Text="Tatar - Russia" Value="tt-RU" />
                <asp:ListItem Text="Telugu" Value="te" />
                <asp:ListItem Text="Telugu - India" Value="te-IN" />
                <asp:ListItem Text="Thai" Value="th" />
                <asp:ListItem Text="Thai - Thailand" Value="th-TH" />
                <asp:ListItem Text="Turkish" Value="tr" />
                <asp:ListItem Text="Turkish - Turkey" Value="tr-TR" />
                <asp:ListItem Text="Ukrainian" Value="uk" />
                <asp:ListItem Text="Ukrainian - Ukraine" Value="uk-UA" />
                <asp:ListItem Text="Urdu" Value="ur" />
                <asp:ListItem Text="Urdu - Pakistan" Value="ur-PK" />
                <asp:ListItem Text="Uzbek" Value="uz" />
                <asp:ListItem Text="Uzbek (Cyrillic) - Uzbekistan" Value="Cy-uz-UZ" />
                <asp:ListItem Text="Uzbek (Latin) - Uzbekistan" Value="Lt-uz-UZ" />
                <asp:ListItem Text="Vietnamese" Value="vi" />
                <asp:ListItem Text="Vietnamese - Vietnam" Value="vi-VN" />
            </asp:DropDownList>
        </asp:Panel>
        <div class="clear">
            <asp:Panel runat="server" GroupingText="Count Settings" CssClass="options">
                <label accesskey="d" for="Edit_ddlItemCount">
                    <u>D</u>efault Number of Feed/Homepage Items</label>
                <asp:DropDownList ID="ddlItemCount" CssClass="number" runat="server" />
                <label accesskey="p" for="Edit_ddlCategoryListPostCount">
                    Number of <u>P</u>osts in Category Lists</label>
                <asp:DropDownList ID="ddlCategoryListPostCount" CssClass="number" runat="server" />
            </asp:Panel>
        </div>
        <div class="clear">
            <div class="options">
                <label accesskey="c" for="Edit_txbSecondaryCss">
                    <u>C</u>ustom CSS
                    <st:helptooltip ID="HelpToolTip1" runat="server" HelpText="You can enter custom CSS within this block.  Be careful as the tool will not validate the CSS.  This CSS will be included (as a proper link) within every page of your blog."
                        ImageUrl="~/images/icons/help-small.png" ImageWidth="16" 
                    ImageHeight="16" />
                </label>
                <asp:TextBox ID="txbSecondaryCss" runat="server" CssClass="textarea" TextMode="MultiLine" />
                <label accesskey="a" for="Edit_txbNews">
                    Static News/<u>A</u>nnouncement</label>
                <asp:TextBox ID="txbNews" runat="server" CssClass="textarea" TextMode="MultiLine" />
                <label accesskey="t" for="Edit_txtGenericTrackingCode">
                    Sitewide <u>T</u>racking code</label>
                <asp:TextBox ID="txbGenericTrackingCode" runat="server" CssClass="textarea" TextMode="MultiLine" />
            </div>
        </div>
        <div class="clear">
            <asp:Button ID="btnPost" runat="server" CssClass="buttonSubmit" Text="Save" />
        </div>
    </div>
</asp:Content>
