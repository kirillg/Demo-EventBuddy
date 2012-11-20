Param([string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."
# Get settings from user configuration file
if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "Config.Local.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile

# Import required settings from config.local.xml if neccessary #
[string] $CSharpSnippets = Resolve-Path $xmlDemoSettings.configuration.codeSnippets.cSharp
[string] $IESnippets = Resolve-Path $xmlDemoSettings.configuration.favorites.iESnippets

[string] $beginSolutionDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.beginSolutionDir
[string] $assetsDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.assetsDir
[string] $vsSuoDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.vsSuoDir

[string] $win8SolutionFile = $xmlDemoSettings.configuration.localPaths.win8SolutionFile
[string] $phone8SolutionFile = $xmlDemoSettings.configuration.localPaths.phone8SolutionFile
[string] $workingDir = $xmlDemoSettings.configuration.localPaths.workingDir
[string] $solutionWorkingDir = $xmlDemoSettings.configuration.localPaths.solutionWorkingDir
[string] $serverSnippetsDirInAssets = $xmlDemoSettings.configuration.localPaths.serverSnippetsDirInAssets

[string] $serverSnippetsList = $xmlDemoSettings.configuration.serverSnippetsList

[string] $usePhoneEmulator = $xmlDemoSettings.configuration.settings.usePhoneEmulator

#[string] $vsSettingsFile = Resolve-Path $xmlDemoSettings.configuration.localPaths.vsSettingsFile

[string] $appName  = $xmlDemoSettings.configuration.appSetup.packagename;
[string] $appxPath =  $xmlDemoSettings.configuration.appSetup.appxPath;

popd
# "========= Main Script =========" #

New-Item "$workingDir" -type directory | Out-Null

write-host
write-host
write-host "========= Copying Begin solution to working directory... ========="
if (!(Test-Path "$solutionWorkingDir"))
{
	New-Item "$solutionWorkingDir" -type directory | Out-Null
}
Copy-Item "$beginSolutionDir\*" "$solutionWorkingDir" -Recurse -Force

write-host "Copying Begin solution to working directory done!"

write-host
write-host
write-host "========= Copying assets code to working directory... ========="
New-Item "$workingDir\Assets" -type directory | Out-Null
Copy-Item "$assetsDir\*" "$workingDir\Assets\" -Recurse -Force
write-host "Copying Assets code to working directory done!"

[string] $solution = Resolve-Path (Join-Path $solutionWorkingDir $win8SolutionFile)
[string] $phoneSolution = Resolve-Path (Join-Path $solutionWorkingDir $phone8SolutionFile)
write-host
write-host
write-host "========= Uninstall > Build > Install Event Buddy app ... ========="

write-host "Removing application..."
Get-AppxPackage | Where {$_.Name –match "$appName"} | Remove-AppxPackage

write-host
write-host
write-host "Building Windows 8 application..."
&"C:\Windows\Microsoft.Net\Framework64\v4.0.30319\MSBUILD.exe" ($solution,"/p:configuration=Release", "/target:Clean,Rebuild,Publish")

write-host
write-host
write-host "Installing Windows 8 application"
[string] $appxPS = Resolve-Path ((Join-Path $solutionWorkingDir $appxPath ) + "\Add-AppDevPackage.ps1")
Start-Process powershell ("-command", "$appxPS")

write-host
write-host
write-host "========= Installing Code Snippets for the Windows 8 application... ========="
[string] $documentsFolder = [Environment]::GetFolderPath("MyDocuments")
if (-NOT (test-path "$documentsFolder"))
{
    $documentsFolder = "$env:UserProfile\Documents";
}

[string] $myCSharpSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets"

Copy-Item "$CSharpSnippets\*.snippet" "$myCSharpSnippetsLocation" -force

write-host "Installing Code Snippets done!"

[string] $myFavorites  = [Environment]::GetFolderPath("Favorites") 
[string] $myIESnippetTargetLocation = "$myFavorites\Links";
write-host
write-host
write-host "========= Installing IE Snippets ... ========="
Copy-Item "$IESnippets\*.*" "$myIESnippetTargetLocation " -force

write-host "Installing IE Snippets done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Disabling IE Download Notify... ========="
# Set-IEDownloadNotify -Disable
# write-host "Disabling IE Download Notify done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Set IE AutoComplete Settings ========="
# Set-SetIEAutoCompleteSettings -AutoCompleteForms  $false -AutoCompleteUsernamesAndPasswords $false -AskBeforeSavingPasswords $false
# write-host "Set IE AutoComplete Settings Done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Setting IE Home Page to blank... ========="
# Set-IEDefaultHomePage -Blank
# write-host "Setting IE Home Page to blank done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Importing VS settings... ========="
# Import-VSSettings $vsSettingsFile
# Start-Sleep -s 10
# Close-VS -Force
# Start-Sleep -s 2
# write-host "Importing VS settings done!"

write-host
write-host
write-host "========= Copying Visual Studio .suo to Begin Solution... ========="
Copy-Item "$vsSuoDir\*.suo" "$solutionWorkingDir" -Recurse -Force
write-host "Copying Visual Studio .suo to begin solution Done!!"

write-host
write-host
write-host "========= Starting Visual Studio... ========="
if (test-path "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe")
{
	Open-VS -SolutionFile $solution
	Start-Sleep -s 2
	write-host "Starting Visual Studio (Windows 8) Done!"
	
	if ($usePhoneEmulator -ieq "true")
	{
		Open-VS -SolutionFile $phoneSolution
		Start-Sleep -s 2
		write-host "Starting Visual Studio (Phone 8) Done!"
	}
}
else
{
	Write-Warning "This task was not executed because the Visual Studio path ${Env:ProgramFiles(x86)}\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe was not found"
}

# Step commented out to avoid making intrusive changes to the environment
# Open-Magnifier -Minimized

Open-IE "https://manage.windowsazure.com/?lc=1033#Workspaces/MobileServicesExtension/apps"