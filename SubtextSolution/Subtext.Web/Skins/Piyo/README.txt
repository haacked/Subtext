This skin has been developed by Simone Chiaretta, inspired by Asual theme (default Blojsom theme).
To install it just unzip the archive into your skin folder and edit the Skins.config file adding the following entry:

	<SkinTemplate SkinID="Piyo"  Skin="Piyo">
		<Scripts>
			<Script Src="Scripts/piyo.js" />
		</Scripts>
		<Styles>
			<Style title="fixed" href="piyo-fixed.css" />
			<Style title="elastic" href="piyo-elastic.css" />
		</Styles>
	</SkinTemplate>

NOTE: to change the default stylesheet that a user will get the first time to the page,
 or after their cookie expires, just change the order in the above config settings.
 i.e.- If piyo-fixed.css is on top, it will be default.

Please send comments to simone@piyosailing.com