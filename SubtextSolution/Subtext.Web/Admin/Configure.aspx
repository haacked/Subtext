<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Configure" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Configure.aspx.cs" Inherits="Subtext.Web.Admin.Pages.Configure" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    Actions
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>
    
<asp:Content ID="configurationOptions" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<%  /*
	        Removing the AdvancedPanel b/c it is incompatible with asp.net AJAX UpdatePanels (due 
	        to the Collapsible control's OnInit method moving its child controls around. In a 
	        future version of Subtext we will replace all AdvancedPanel controls with an Extender
	        from the AJAX Control Toolkit.
        */
	%>
	<div class="CollapsibleHeader">
	    <span>Configure</span>
	</div>
	<div class="Edit">
		<asp:Panel runat="server" GroupingText="Main Settings" CssClass="options" >
			
				<label accesskey="t" for="Edit_txbTitle"><u>T</u>itle</label>
				<asp:TextBox id="txbTitle" runat="server" CssClass="textbox"></asp:TextBox>
			
				<label accesskey="s" for="Edit_txbSubtitle"><u>S</u>ubtitle</label>
				<asp:TextBox id="txbSubtitle" runat="server" CssClass="textbox"></asp:TextBox>
			
				<label accesskey="u" for="Edit_txbUser"><u>O</u>wner</label>
				<asp:TextBox id="txbUser" runat="server" CssClass="textbox" Enabled="false" />
			
				<label accesskey="e" for="Edit_txbAuthorEmail">Owner's <u>E</u>mail</label>
				<asp:TextBox id="txbAuthorEmail" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
			
				<label accesskey="s" for="Edit_ddlSkin">Display <u>S</u>kin</label>
				<asp:DropDownList id="ddlSkin" runat="server"></asp:DropDownList>
				
				<div>
				    <asp:CheckBox id="ckbAllowServiceAccess" runat="server" CssClass="checkbox" AccessKey="w" Text="Allow <u>W</u>eb Service Access" />
				</div>
		
		</asp:Panel>
		<asp:Panel runat="server" GroupingText="Location Settings" CssClass="options wide">
			<asp:UpdatePanel ID="ajaxTimezone" runat="server">
			    <ContentTemplate>
			        <p>
				        <label class="Block" accesskey="z" for="Edit_ddlTimezone">
					        Your Time<u>z</u>one
					        <st:HelpToolTip id="hlpTimeZone" runat="server" HelpText="Select your timezone, which may differ from the timezone where your blog server is located." ImageUrl="~/images/icons/help-small.png" ImageWidth="16" ImageHeight="16" />
				        </label>
				        <asp:DropDownList id="ddlTimezone" runat="server" OnSelectedIndexChanged="ddlTimezone_SelectedIndexChanged" AutoPostBack="true">
        					
				        </asp:DropDownList>
			        </p>
			        <p>
				        <em>Time at selected timezone is: <strong><asp:Label ID="lblCurrentTime" runat="server" /></strong></em><br />
				        <em>Time at server is: <strong><asp:Label ID="lblServerTime" runat="server" /></strong></em><br />
				        <em>Server timezone is <asp:Label ID="lblServerTimeZone" runat="server" /></em><br />
				        <em><acronym title="Coordinated Universal Time">UTC</acronym> time is <asp:Label ID="lblUtcTime" runat="server" /></em>
			        </p>
			    </ContentTemplate>
			</asp:UpdatePanel>

			
			<label accesskey="l" for="Edit_ddlLangLocale"><u>L</u>anguage/Locale</label>
			
			<asp:DropDownList id="ddlLangLocale" runat="server">
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
		<asp:Panel runat="server" GroupingText="Count Settings" CssClass="options">
			
			<label accesskey="d" for="Edit_ddlItemCount"><u>D</u>efault Number of Feed/Homepage Items</label>
			<asp:DropDownList id="ddlItemCount" runat="server" />
		
			<label accesskey="p" for="Edit_ddlCategoryListPostCount">Number of <u>P</u>osts in Category Lists</label>
			<asp:DropDownList id="ddlCategoryListPostCount" runat="server" />
		
		</asp:Panel>
		<div class="clear">
			<div class="options">
				<label accesskey="c" for="Edit_txbSecondaryCss">
					<st:HelpToolTip id="HelpToolTip1" runat="server" HelpText="You can enter custom CSS within this block.  Be careful as the tool will not validate the CSS.  This CSS will be included (as a proper link) within every page of your blog."><u>C</u>ustom CSS</st:HelpToolTip>
				</label>
				<asp:TextBox id="txbSecondaryCss" runat="server" CssClass="textarea" TextMode="MultiLine"></asp:TextBox>
			</div>
			<div class="options">
				<label accesskey="a" for="Edit_txbNews">Static News/<u>A</u>nnouncement</label>
				<asp:TextBox id="txbNews" runat="server" CssClass="textarea" TextMode="MultiLine"></asp:TextBox>
			</div>
		</div>
		
		<!--Mail to Weblog UI-->
		<asp:Panel ID="Panel1" runat="server" GroupingText="Mail To Weblog Settings" CssClass="options">
		    <asp:UpdatePanel ID="pop3Settings" runat="Server">
				<ContentTemplate>
					<div>
						<asp:CheckBox id="ckbPop3MailToWeblog" runat="server" CssClass="checkbox" AccessKey="m" 
							Text="Enable <u>M</u>ailToWeblog" AutoPostBack="true" />
					</div>
	    			
					<asp:Panel ID="pnlMailToWeblogConfigWrapper" runat="server" Visible="false" >
						<label accesskey="p" for="Edit_txbPop3Server"><u>P</u>op3 Server</label>
						<asp:TextBox id="txbPop3Server" runat="server" CssClass="textbox">pop3.yourdomain.com</asp:TextBox>
	        		
						<label accesskey="r" for="Edit_txbPop3User">Pop3 Use<u>r</u> Name</label>
						<asp:TextBox id="txbPop3User" runat="server" CssClass="textbox"></asp:TextBox>
	        			
						<label accesskey="w" for="Edit_txbPop3Password">Pop3 Pass<u>w</u>ord</label>
						<asp:TextBox id="txbPop3Password" runat="server" CssClass="textbox" TextMode="Password" />
	        		
						<label accesskey="f" for="Edit_txbPop3Subject">Pop3 Subject Pre<u>f</u>ix</label>
						<asp:TextBox id="txbPop3Subject" runat="server" CssClass="textbox">BLOGIT</asp:TextBox>
	        		
						<label accesskey="g" for="Edit_txbPop3StartTag">Pop3 Start Ta<u>g</u></label>
						<asp:TextBox id="txbPop3StartTag" runat="server" CssClass="textbox">startTag</asp:TextBox>&nbsp;
	        			
						<label accesskey="x" for="Edit_txbPop3EndTag">Pop3 End Tag<u>X</u></label>
						<asp:TextBox id="txbPop3EndTag" runat="server" CssClass="textbox">endTag</asp:TextBox>
	                
						<div>
							<asp:CheckBox id="ckbPop3InlineAttachedPict" runat="server" CssClass="checkbox" AccessKey="3" Text="Pop<u>3</u> Inline Attached Pictures" />
						</div>
	                
						<label accesskey="h" for="Edit_txbPop3ThumbHeight">Pop3 Inline Attached Pictures Thumb <u>H</u>eight</label>
						<asp:TextBox id="txbPop3ThumbHeight" runat="server" CssClass="textbox number">100</asp:TextBox>
	                
						<div>
							<asp:CheckBox id="ckbPop3DeleteProcessedEmail" runat="server" CssClass="checkbox" AccessKey="o" Text="Delete Pr<u>o</u>cessed Emails" />
						</div>
					</asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
		</asp:Panel>
		<!--End of Mail to Weblog UI-->
		
		<div class="clear">
			<div class="options">
				<p>
					<label class="Block" accesskey="m" for="Edit_txtMetaTags">Additional <u>M</u>eta Tags</label>
					<asp:TextBox id="txbMetaTags" runat="server" CssClass="textarea" TextMode="MultiLine"></asp:TextBox>
				</p>
			</div>
			<div class="options">
				<p>
					<label class="Block" accesskey="t" for="Edit_txtGenericTrackingCode">Sitewide <u>T</u>racking code</label>
					<asp:TextBox id="txbGenericTrackingCode" runat="server" CssClass="textarea" TextMode="MultiLine"></asp:TextBox>
				</p>
			</div>
		</div>
		<div class="clear">
			<asp:Button id="btnPost" runat="server" CssClass="buttonSubmit" Text="Save" />
		</div>
	</div>
</asp:Content>
