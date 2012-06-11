/* */

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Images' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'Url'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images]
		ADD [Url] [nvarchar] (512) NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Host' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'Email'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
		ADD [Email] [nvarchar] (256) NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'TimeZoneId'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [TimeZoneId] [nvarchar] (128) NULL
END
GO

-- 32 bit
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Morocco Standard Time' WHERE TimeZone = -1661197935
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'UTC' WHERE TimeZone = -884914970
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'GMT Standard Time' WHERE TimeZone = 276583904
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Greenwich Standard Time' WHERE TimeZone = 1781660264
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Europe Standard Time' WHERE TimeZone = -798214753
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Europe Standard Time' WHERE TimeZone = 1437650955
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Romance Standard Time' WHERE TimeZone = 1638717133
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central European Standard Time' WHERE TimeZone = 57756332
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Central Africa Standard Time' WHERE TimeZone = 673793782
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Jordan Standard Time' WHERE TimeZone = 504144858
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'GTB Standard Time' WHERE TimeZone = -1218342521
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Middle East Standard Time' WHERE TimeZone = -1378520932
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Egypt Standard Time' WHERE TimeZone = -196964124
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'South Africa Standard Time' WHERE TimeZone = 975644206
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'FLE Standard Time' WHERE TimeZone = -612696498
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Israel Standard Time' WHERE TimeZone = -970708731
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Europe Standard Time' WHERE TimeZone = -412154371
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Namibia Standard Time' WHERE TimeZone = 86633817
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arabic Standard Time' WHERE TimeZone = 2023587485
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arab Standard Time' WHERE TimeZone = -167174882
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Russian Standard Time' WHERE TimeZone = -402707908
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Africa Standard Time' WHERE TimeZone = 137438298
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Georgian Standard Time' WHERE TimeZone = -190664863
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Iran Standard Time' WHERE TimeZone = 563311159
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arabian Standard Time' WHERE TimeZone = -2085021355
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Azerbaijan Standard Time' WHERE TimeZone = 2052522788
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mauritius Standard Time' WHERE TimeZone = 664263238
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Caucasus Standard Time' WHERE TimeZone = 1342357774
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Afghanistan Standard Time' WHERE TimeZone = 195872777
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Ekaterinburg Standard Time' WHERE TimeZone = -272795071
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pakistan Standard Time' WHERE TimeZone = -1769678438
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'West Asia Standard Time' WHERE TimeZone = -1229352363
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'India Standard Time' WHERE TimeZone = 658848125
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Sri Lanka Standard Time' WHERE TimeZone = -1596721493
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Nepal Standard Time' WHERE TimeZone = -1249671084
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'N. Central Asia Standard Time' WHERE TimeZone = -175654566
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Asia Standard Time' WHERE TimeZone = 43840148
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Myanmar Standard Time' WHERE TimeZone = 1081970031
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SE Asia Standard Time' WHERE TimeZone = -640089798
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'North Asia Standard Time' WHERE TimeZone = -1622414852
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'China Standard Time' WHERE TimeZone = -1544826827
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'North Asia East Standard Time' WHERE TimeZone = 1919547074
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Singapore Standard Time' WHERE TimeZone = -633666887
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Australia Standard Time' WHERE TimeZone = 97075335
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Taipei Standard Time' WHERE TimeZone = 1612399360
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tokyo Standard Time' WHERE TimeZone = 1235385739
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Korea Standard Time' WHERE TimeZone = -1556155466
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Yakutsk Standard Time' WHERE TimeZone = 1347173492
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Cen. Australia Standard Time' WHERE TimeZone = -159819906
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'AUS Central Standard Time' WHERE TimeZone = 838505781
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Australia Standard Time' WHERE TimeZone = -2019245524
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'AUS Eastern Standard Time' WHERE TimeZone = -2010970479
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'West Pacific Standard Time' WHERE TimeZone = -1972898444
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tasmania Standard Time' WHERE TimeZone = -147965313
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Vladivostok Standard Time' WHERE TimeZone = 958916168
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Pacific Standard Time' WHERE TimeZone = 619026968
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'New Zealand Standard Time' WHERE TimeZone = -695818228
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Fiji Standard Time' WHERE TimeZone = 829705268
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Kamchatka Standard Time' WHERE TimeZone = 1057208658
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tonga Standard Time' WHERE TimeZone = -165907155
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Azores Standard Time' WHERE TimeZone = 1455188603
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Cape Verde Standard Time' WHERE TimeZone = 1792605581
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mid-Atlantic Standard Time' WHERE TimeZone = -1639373905
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. South America Standard Time' WHERE TimeZone = 352495817
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Argentina Standard Time' WHERE TimeZone = 1452104566
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Eastern Standard Time' WHERE TimeZone = -1904441504
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Greenland Standard Time' WHERE TimeZone = 2069054746
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Montevideo Standard Time' WHERE TimeZone = -698758876
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Newfoundland Standard Time' WHERE TimeZone = -1602501789
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Paraguay Standard Time' WHERE TimeZone = 1442930628
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Atlantic Standard Time' WHERE TimeZone = 558059746
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Western Standard Time' WHERE TimeZone = -1819533385
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Brazilian Standard Time' WHERE TimeZone = -765050843
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific SA Standard Time' WHERE TimeZone = 429541042
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Venezuela Standard Time' WHERE TimeZone = 985181202
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Pacific Standard Time' WHERE TimeZone = 1013972657
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Eastern Standard Time' WHERE TimeZone = -1188006249
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'US Eastern Standard Time' WHERE TimeZone = 1685750035
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central America Standard Time' WHERE TimeZone = 1437958604
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Standard Time' WHERE TimeZone = -590151426
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Standard Time (Mexico)' WHERE TimeZone = -262115582
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Canada Central Standard Time' WHERE TimeZone = 1827704133
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'US Mountain Standard Time' WHERE TimeZone = -1319529113
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mountain Standard Time (Mexico)' WHERE TimeZone = -1681944020
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mountain Standard Time' WHERE TimeZone = 2123325864
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific Standard Time' WHERE TimeZone = -2037797565
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific Standard Time (Mexico)' WHERE TimeZone = -1766322352
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Alaskan Standard Time' WHERE TimeZone = 997436142
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Hawaiian Standard Time' WHERE TimeZone = 1106595067
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Samoa Standard Time' WHERE TimeZone = 733176969
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Dateline Standard Time' WHERE TimeZone = -1069290744
GO
-- 64 bit
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Morocco Standard Time' WHERE TimeZone = -1674444738
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'UTC' WHERE TimeZone = -1844635960
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'GMT Standard Time' WHERE TimeZone = -1625750204
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Greenwich Standard Time' WHERE TimeZone = -685615200
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Europe Standard Time' WHERE TimeZone = 1859473815
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Europe Standard Time' WHERE TimeZone = -460310079
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Romance Standard Time' WHERE TimeZone = -704851233
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central European Standard Time' WHERE TimeZone = -560844558
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Central Africa Standard Time' WHERE TimeZone = -726664342
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Jordan Standard Time' WHERE TimeZone = -1277044542
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'GTB Standard Time' WHERE TimeZone = -1842006117
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Middle East Standard Time' WHERE TimeZone = -291626098
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Egypt Standard Time' WHERE TimeZone = -490860919
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'South Africa Standard Time' WHERE TimeZone = 860862901
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'FLE Standard Time' WHERE TimeZone = -383128615
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Israel Standard Time' WHERE TimeZone = 658548668
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Europe Standard Time' WHERE TimeZone = -1183008407
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Namibia Standard Time' WHERE TimeZone = 1586716951
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arabic Standard Time' WHERE TimeZone = 680499712
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arab Standard Time' WHERE TimeZone = -839495020
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Russian Standard Time' WHERE TimeZone = 1919281727
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Africa Standard Time' WHERE TimeZone = 745651479
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Georgian Standard Time' WHERE TimeZone = 348559166
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Iran Standard Time' WHERE TimeZone = -1723939488
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Arabian Standard Time' WHERE TimeZone = 612516132
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Azerbaijan Standard Time' WHERE TimeZone = 248568749
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mauritius Standard Time' WHERE TimeZone = 854731451
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Caucasus Standard Time' WHERE TimeZone = -1443998676
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Afghanistan Standard Time' WHERE TimeZone = 179237422
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Ekaterinburg Standard Time' WHERE TimeZone = 1629619399
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pakistan Standard Time' WHERE TimeZone = -206580251
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'West Asia Standard Time' WHERE TimeZone = -1342646435
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'India Standard Time' WHERE TimeZone = -373611211
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Sri Lanka Standard Time' WHERE TimeZone = 1270428731
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Nepal Standard Time' WHERE TimeZone = -383210494
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'N. Central Asia Standard Time' WHERE TimeZone = -1048615953
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Asia Standard Time' WHERE TimeZone = 1286541411
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Myanmar Standard Time' WHERE TimeZone = 1044583239
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SE Asia Standard Time' WHERE TimeZone = 146933106
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'North Asia Standard Time' WHERE TimeZone = -755135757
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'China Standard Time' WHERE TimeZone = -2089982209
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'North Asia East Standard Time' WHERE TimeZone = -475511664
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Singapore Standard Time' WHERE TimeZone = 1132222406
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'W. Australia Standard Time' WHERE TimeZone = -399707485
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Taipei Standard Time' WHERE TimeZone = -1123372412
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tokyo Standard Time' WHERE TimeZone = 824682022
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Korea Standard Time' WHERE TimeZone = -98687038
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Yakutsk Standard Time' WHERE TimeZone = -1859252204
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Cen. Australia Standard Time' WHERE TimeZone = 1294115614
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'AUS Central Standard Time' WHERE TimeZone = 1440654346
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. Australia Standard Time' WHERE TimeZone = -1174139371
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'AUS Eastern Standard Time' WHERE TimeZone = 1647873175
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'West Pacific Standard Time' WHERE TimeZone = -877351560
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tasmania Standard Time' WHERE TimeZone = -665026478
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Vladivostok Standard Time' WHERE TimeZone = -1294423576
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Pacific Standard Time' WHERE TimeZone = -223963774
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'New Zealand Standard Time' WHERE TimeZone = 742740495
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Fiji Standard Time' WHERE TimeZone = 344091856
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Kamchatka Standard Time' WHERE TimeZone = -524620051
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Tonga Standard Time' WHERE TimeZone = 384975597
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Azores Standard Time' WHERE TimeZone = -1860139178
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Cape Verde Standard Time' WHERE TimeZone = -399497991
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mid-Atlantic Standard Time' WHERE TimeZone = 2065415991
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'E. South America Standard Time' WHERE TimeZone = 1496017926
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Argentina Standard Time' WHERE TimeZone = -1391526861
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Eastern Standard Time' WHERE TimeZone = -116074804
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Greenland Standard Time' WHERE TimeZone = -2037421198
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Montevideo Standard Time' WHERE TimeZone = -741348800
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Newfoundland Standard Time' WHERE TimeZone = -44316549
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Paraguay Standard Time' WHERE TimeZone = 500578636
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Atlantic Standard Time' WHERE TimeZone = 1740635284
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Western Standard Time' WHERE TimeZone = 389455682
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Brazilian Standard Time' WHERE TimeZone = -1002842239
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific SA Standard Time' WHERE TimeZone = 246913255
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Venezuela Standard Time' WHERE TimeZone = 2106691661
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'SA Pacific Standard Time' WHERE TimeZone = 121378899
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Eastern Standard Time' WHERE TimeZone = 493190372
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'US Eastern Standard Time' WHERE TimeZone = -129629376
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central America Standard Time' WHERE TimeZone = -1160296163
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Standard Time' WHERE TimeZone = -235799587
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Central Standard Time (Mexico)' WHERE TimeZone = -2044313967
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Canada Central Standard Time' WHERE TimeZone = 780421781
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'US Mountain Standard Time' WHERE TimeZone = -322453441
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mountain Standard Time (Mexico)' WHERE TimeZone = 1032262625
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Mountain Standard Time' WHERE TimeZone = -497815947
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific Standard Time' WHERE TimeZone = -667686747
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Pacific Standard Time (Mexico)' WHERE TimeZone = 1336905121
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Alaskan Standard Time' WHERE TimeZone = -2023655303
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Hawaiian Standard Time' WHERE TimeZone = 2091460006
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Samoa Standard Time' WHERE TimeZone = 145434963
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = 'Dateline Standard Time' WHERE TimeZone = -1114305718
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'TimeZone'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP CONSTRAINT [DF_subtext_Config_TimeZone]

	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN [TimeZone]
END