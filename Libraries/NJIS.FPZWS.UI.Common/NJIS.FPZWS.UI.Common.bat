MSBuild.SonarQube.Runner.exe begin /k:"NJIS.FPZWS.UI.Common" /n:"NJIS.FPZWS.UI.Common" /v:"1.0"
SonarScanner.MSBuild.exe /t:Rebuild
MSBuild.SonarQube.Runner.exe end