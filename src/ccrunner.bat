..\BuildTools\Nant\NAnt -buildfile:SubText-CCNET.build -D:CCNetProject=Subtext -D:CCNetBuildDate=2007/08/11 -D:CCNetBuildTime=13:00:00 -D:CCNetArtifactDirectory="artifacts\output" -D:CCNetWorkingDirectory="." -D:CCNetLabel=2.0.0.0 -D:fxcop.exe="..\BuildTools\FxCop\FxCopCmd.exe" testAndCover reporting release dist.source
pause