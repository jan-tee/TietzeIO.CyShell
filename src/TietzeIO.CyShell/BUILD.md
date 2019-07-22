# TietzeIO.CyShell

## Build dependencies

To build the online help/documentation, you will need to have these **Powershell modules** installed (installable with the below commands)

* Install-Module PSake
* Install-Module PlatyPS
* Install-Module VSSetup

## Manual documentation build instructions

In the project directory, run `Invoke-Psake` to build and update the help files.

Markdown files will be created in the `Help\en-us\` directory. PlatyPS will then convert these files into an XML document
that is to be shared alongside the PS modules. The file is automatically placed in the output directory, and the
manual installer `Invoke-InstallModule.ps1` as well as the module `.psd1` file refer to the help files.

## Updating documentation

All documentation updates should be made in the `Help/en-us/` in the specific Markdown file (`<cmdlet name>.md`). Once this has been updated, rerun
the `Invoke-Psake` command to update the docs or run the build task.

If you run it manually and you have already run Invoke-Psake once in the current Powershell session, you will have to close
the session and re-open to rebuild.

Rebuild documentation command: `invoke-psake BuildHelp`
