<#
    .SYNOPSIS
        Install the module in the PowerShell module folder.
    .DESCRIPTION
        Install the module in the PowerShell module folder by copying all the files.
#>

$ModuleName = 'TietzeIO.CyShell'

if ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)) 
{
	# Windows
	$ModulePath = 'C:\Program Files\WindowsPowerShell\Modules'
} else {
	# Mac/Linux be like...
	$ModulePath = "$([Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile))/.local/share/powershell/Modules"
}

Write-Host "$($ModuleName) module installation started"

$SourcePath = $PSScriptRoot
$ModuleDir = Join-Path -Path $ModulePath -ChildPath $ModuleName

$ModuleData = Import-PowerShellDataFile -Path (Join-Path $SourcePath -ChildPath "$($ModuleName).psd1")
$FileList = $ModuleData.FileList |% {
	# cross-platform... but the .psd1 seems to use a static platform separator.

	$_ -replace "\\",[IO.Path]::DirectorySeparatorChar
	}

# $AllFiles = Get-ChildItem -Path $SourcePath -File -Recurse

# delete old directories
$FileList | 
    ForEach-Object { 
        $FileName = $_
		$DestinationFile = Join-Path $ModuleDir -ChildPath $FileName
		$TargetDir = Split-Path $DestinationFile

        if (Test-Path $TargetDir -PathType Container) {
		    Get-ChildItem -Path $TargetDir -Recurse | Remove-Item -Recurse
        }
	}

# copy
$FileList | 
    ForEach-Object { 
        $FileName = $_
		$SourceFile = Join-Path -Path $SourcePath -ChildPath $FileName
		$DestinationFile = Join-Path $ModuleDir -ChildPath $FileName
		$TargetDir = Split-Path $DestinationFile

		if (-not (Test-Path $SourceFile)) {
			Write-Error "Source file $($SourceFile) does not exist."
			Exit
		}

		if (-not (Test-Path $TargetDir)) {
			New-Item -Path $TargetDir -ItemType Directory -EA Stop | Out-Null
			Write-Host "$ModuleName created module folder '$($TargetDir)'"
		}

		Copy-Item -Path $SourceFile -Destination $DestinationFile

		Write-Host "$($ModuleName) installed module file '$($FileName)' to '$($DestinationFile)'"
	}

Write-Host "$ModuleName module installation successful"
