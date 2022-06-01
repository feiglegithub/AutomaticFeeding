SonarScanner.MSBuild.exe begin /k:"NJIS.FPZWS.Common" /n:"NJIS.FPZWS.Common" /v:"1.0"
MSBuild.exe /t:Rebuild
SonarScanner.MSBuild.exe end