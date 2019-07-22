#
# This build assumes the following directory structure
#
#  \Build          - This is where the project build code lives
#  \BuildArtifacts - This folder is created if it is missing and contains output of the build
#  \Code           - This folder contains the source code or solutions you want to build
#
Properties {
    $build_dir = Split-Path $psake.build_script_file
    $build_artifacts_dir = "$($build_dir)\Bin\Release\netstandard2.0"
    $code_dir = "$($build_dir)"
	$install_path = Get-VSSetupInstance | Select-Object -Property InstallationPath
	$msbuild = "$($install_path.InstallationPath)\MSBuild\Current\Bin\msbuild.exe"
	$OutDir = "$($PSScriptRoot)\bin\Release\netstandard2.0"
	$DefaultLocale = "en-us"
	$DocsRootDir = "Help"
	$WebDocsRootDir = "WebHelp"
	$ModuleName = "TietzeIO.CyShell"

    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
    $ModuleOutDir = "$OutDir"

    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
    $UpdatableHelpOutDir = "$OutDir\UpdatableHelp"

    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
    $SharedProperties = @{}

    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
    $LineSep = "-" * 78
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

# Task Default -Depends Build, BuildHelp, AfterBuildHelp
Task Default -Depends BuildHelp, AfterBuildHelp

Task Init -requiredVariables OutDir {
    if (!(Test-Path -LiteralPath $OutDir)) {
        New-Item $OutDir -ItemType Directory -Verbose:$VerbosePreference > $null
    }
    else {
        Write-Verbose "$($psake.context.currentTaskName) - directory already exists '$OutDir'."
    }
}

Task Build -depends Init, Clean {
    Write-Host "Building $($ModuleName.csproj)" -ForegroundColor Green
	Write-Host $code_dir -ForegroundColor Green

	Exec { & $msbuild "$build_dir\$($ModuleName).csproj" /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$build_artifacts_dir }
}

Task Clean {
    Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
    if (Test-Path $build_artifacts_dir)
    {
        rd $build_artifacts_dir -rec -force | out-null
    }

    mkdir $build_artifacts_dir | out-null

    Write-Host "Cleaning $($ModuleName).csproj" -ForegroundColor Green
    Exec {  & $msbuild "$build_dir\$($ModuleName).csproj" /t:Clean /p:Configuration=Release /v:quiet }
}

Task BuildHelp -depends GenerateMarkdown, GenerateHelpFiles, BuildUpdatableHelp {
}

Task GenerateMarkdown -requiredVariables DefaultLocale, DocsRootDir, ModuleName, ModuleOutDir {
    if (!(Get-Module platyPS -ListAvailable)) {
        "platyPS module is not installed. Skipping $($psake.context.currentTaskName) task."
        return
    }

	write-host "Importing $($ModuleOutDir)\$($ModuleName).psd1"
    $moduleInfo = Import-Module $ModuleOutDir\$ModuleName.psd1 -Global -Force -PassThru

    try {
        if ($moduleInfo.ExportedCommands.Count -eq 0) {
            "No commands have been exported. Skipping $($psake.context.currentTaskName) task."
            return
        }

        if (!(Test-Path -LiteralPath $DocsRootDir)) {
            New-Item $DocsRootDir -ItemType Directory > $null
        }

        if (!(Test-Path -LiteralPath $WebDocsRootDir)) {
            New-Item $WebDocsRootDir -ItemType Directory > $null
        }

		# Exclude some help files from updates to ensure data specific to the build system does not leak through auto-completing parameters
		$NoUpdateFiles = @( "Connect-Cylance.md", 
			"Remove-CylanceConsole.md", 
			"Get-CylanceConsole.md",
			"Set-CylanceConsole.md", 
			"Convert-CylanceTDRtoXLSX.md",
			"Get-CylanceTDR.md")

        if (Get-ChildItem -LiteralPath $DocsRootDir -Filter *.md -Recurse -File) {
			# if there are markdown files, process them
            Get-ChildItem -LiteralPath $DocsRootDir -Filter *.md -Recurse -File | ForEach-Object {
				if ($_.Name -iin $NoUpdateFiles) {
					Write-host "Skipping updates to $($_.Name) because it is on veto list of help files that should not be updated to avoid leaking build system configuration."
				} else {
					Update-MarkdownHelp -Path $_.FullName -Verbose:$VerbosePreference > $null
				}
            }
        }

        # ErrorAction set to SilentlyContinue so this command will not overwrite an existing MD file.
        New-MarkdownHelp -Module $ModuleName `
			-Locale $DefaultLocale `
			-WithModulePage -ModulePagePath $WebDocsRootDir\$DefaultLocale `
			-OutputFolder $DocsRootDir\$DefaultLocale `
			-ErrorAction SilentlyContinue `
			-Verbose:$VerbosePreference > $null
    }
    finally {
        Remove-Module $ModuleName
    }
}

Task GenerateHelpFiles -requiredVariables DocsRootDir, ModuleName, ModuleOutDir, OutDir {
    if (!(Get-Module platyPS -ListAvailable)) {
        "platyPS module is not installed. Skipping $($psake.context.currentTaskName) task."
        return
    }

    if (!(Get-ChildItem -LiteralPath $DocsRootDir -Filter *.md -Recurse -ErrorAction SilentlyContinue)) {
        "No markdown help files to process. Skipping $($psake.context.currentTaskName) task."
        return
    }

    $helpLocales = (Get-ChildItem -Path $DocsRootDir -Directory).Name

    # Generate the module's primary MAML help file.
    foreach ($locale in $helpLocales) {
        New-ExternalHelp -Path $DocsRootDir\$locale -OutputPath $ModuleOutDir\$locale -Force `
                         -ErrorAction SilentlyContinue -Verbose:$VerbosePreference > $null
    }
}

Task BuildUpdatableHelp -depends CoreBuildUpdatableHelp {
}

Task CoreBuildUpdatableHelp -requiredVariables DocsRootDir, ModuleName, UpdatableHelpOutDir {
    if (!(Get-Module platyPS -ListAvailable)) {
        "platyPS module is not installed. Skipping $($psake.context.currentTaskName) task."
        return
    }

    $helpLocales = (Get-ChildItem -Path $DocsRootDir -Directory).Name

    # Create updatable help output directory.
    if (!(Test-Path -LiteralPath $UpdatableHelpOutDir)) {
        New-Item $UpdatableHelpOutDir -ItemType Directory -Verbose:$VerbosePreference > $null
    }
    else {
        Write-Verbose "$($psake.context.currentTaskName) - directory already exists '$UpdatableHelpOutDir'."
        Get-ChildItem $UpdatableHelpOutDir | Remove-Item -Recurse -Force -Verbose:$VerbosePreference
    }

    # Generate updatable help files.  Note: this will currently update the version number in the module's MD
    # file in the metadata.
    foreach ($locale in $helpLocales) {
        New-ExternalHelpCab -CabFilesFolder $ModuleOutDir\$locale -LandingPagePath $WebDocsRootDir\$locale\$ModuleName.md `
                            -OutputFolder $UpdatableHelpOutDir -Verbose:$VerbosePreference > $null
    }
}

Task AfterBuildHelp {
    if (!(Get-Module platyPS -ListAvailable)) {
        "platyPS module is not installed. Skipping $($psake.context.currentTaskName) task."
        return
    }

	# copy default locale help files to module directory to make them discoverable when the module is installed
	Get-ChildItem -Path (Join-Path -Path $OutDir -ChildPath $DefaultLocale) -File -Filter "*.dll-Help.xml" |% {
		Copy-Item -Path $_.FullName -Destination $OutDir
	}
}