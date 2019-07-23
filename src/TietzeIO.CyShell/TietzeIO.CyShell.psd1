#
# Module manifest for module 'cyapi-v2-powershell'
#
@{

# Script module or binary module file associated with this manifest.
RootModule = 'TietzeIO.CyShell.dll'

# Version number of this module.
ModuleVersion = '1.0.2'

# Supported PSEditions
# CompatiblePSEditions = @()

# ID used to uniquely identify this module
GUID = '{DD4EE15C-01A2-484B-84E9-7176933AF4C6}'

# Author of this module
Author = 'Jan Tietze'

# Company or vendor of this module
CompanyName = 'Jan Tietze'

# Copyright statement for this module
Copyright = '(c) 2019 Jan Tietze. All rights reserved.'

# Description of the functionality provided by this module
Description = @'
A module that provides Powershell commands to deal with CylancePROTECT and 
OPTICS detection and prevention events.

Check out the project page at https://github.com/jan-tee/TietzeIO.CyShell and the README and FAQ!.
'@

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '5.1'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# https://weblog.west-wind.com/posts/2019/Feb/19/Using-NET-Standard-with-Full-Framework-NET
DotNetFrameworkVersion = '4.7.2'

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# CLRVersion = '2.0'

# Processor architecture (None, X86, Amd64) required by this module
# (if this is set to anything but "none", Linux/macOS will fail with a "can't load libapi-ms-win-core-sysinfo-l1-1-0.dll" exception)
ProcessorArchitecture = 'None'

# Modules that must be imported into the global environment prior to importing this module
RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @(
	"CsvHelper.dll",
	"EPPlus.dll",
	"Flurl.dll",
	"Flurl.Http.dll",
	"Microsoft.CSharp.dll",
	"Microsoft.Data.Sqlite.dll",
	"Microsoft.DotNet.PlatformAbstractions.dll",
	"Microsoft.EntityFrameworkCore.Abstractions.dll",
	"Microsoft.EntityFrameworkCore.dll",
	"Microsoft.EntityFrameworkCore.Relational.dll",
	"Microsoft.EntityFrameworkCore.Sqlite.dll",
	"Microsoft.Extensions.Caching.Abstractions.dll",
	"Microsoft.Extensions.Caching.Memory.dll",
	"Microsoft.Extensions.Configuration.Abstractions.dll",
	"Microsoft.Extensions.Configuration.Binder.dll",
	"Microsoft.Extensions.Configuration.dll",
	"Microsoft.Extensions.Configuration.FileExtensions.dll",
	"Microsoft.Extensions.Configuration.Json.dll",
	"Microsoft.Extensions.DependencyInjection.Abstractions.dll",
	"Microsoft.Extensions.DependencyInjection.dll",
	"Microsoft.Extensions.DependencyModel.dll",
	"Microsoft.Extensions.FileProviders.Abstractions.dll",
	"Microsoft.Extensions.FileProviders.Physical.dll",
	"Microsoft.Extensions.FileSystemGlobbing.dll",
	"Microsoft.Extensions.Logging.Abstractions.dll",
	"Microsoft.Extensions.Logging.dll",
	"Microsoft.Extensions.Options.dll",
	"Microsoft.Extensions.Primitives.dll",
	"Newtonsoft.Json.dll",
	"Nito.AsyncEx.Tasks.dll",
	"Nito.Disposables.dll",
	"NLog.dll",
	"Polly.dll",
	"Remotion.Linq.dll",
	"SQLitePCLRaw.batteries_green.dll",
	"SQLitePCLRaw.batteries_v2.dll",
	"SQLitePCLRaw.core.dll",
	"SQLitePCLRaw.provider.e_sqlite3.dll",
	"System.AppContext.dll",
	"System.Buffers.dll",
	"System.Collections.Concurrent.dll",
	"System.Collections.Immutable.dll",
	"System.Collections.NonGeneric.dll",
	"System.Collections.Specialized.dll",
	"System.ComponentModel.Annotations.dll",
	"System.ComponentModel.dll",
	"System.ComponentModel.Primitives.dll",
	"System.ComponentModel.TypeConverter.dll",
	"System.Data.Common.dll",
	"System.Diagnostics.DiagnosticSource.dll",
	"System.Dynamic.Runtime.dll",
	"System.Interactive.Async.dll",
	"System.IO.FileSystem.Primitives.dll",
	"System.Linq.dll",
	"System.Linq.Expressions.dll",
	"System.Linq.Queryable.dll",
	"System.Management.Automation.dll",
	"System.Memory.dll",
	"System.Numerics.Vectors.dll",
	"System.ObjectModel.dll",
	"System.Reflection.Emit.dll",
	"System.Reflection.Emit.ILGeneration.dll",
	"System.Reflection.Emit.Lightweight.dll",
	"System.Reflection.TypeExtensions.dll",
	"System.Runtime.CompilerServices.Unsafe.dll",
	"System.Runtime.Numerics.dll",
	"System.Security.Claims.dll",
	"System.Security.Cryptography.Cng.dll",
	"System.Security.Cryptography.OpenSsl.dll",
	"System.Security.Cryptography.Pkcs.dll",
	"System.Security.Cryptography.Primitives.dll",
	"System.Security.Principal.dll",
	"System.Text.Encoding.CodePages.dll",
	"System.Text.RegularExpressions.dll",
	"System.Threading.dll",
	"System.Threading.Tasks.Extensions.dll",
	"System.Xml.ReaderWriter.dll",
	"System.Xml.XmlDocument.dll",
	"System.Xml.XPath.dll",
	"System.Xml.XPath.XmlDocument.dll",
	"TietzeIO.CyAPI.CAE.dll",
	"TietzeIO.CyAPI.Configuration.DPAPI.dll",
	"TietzeIO.CyAPI.Configuration.Plaintext.dll",
	"TietzeIO.CyAPI.dll",
	"TietzeIO.CyAPI.Polyfills.dll",
	"TietzeIO.CyShell.dll"
)

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
NestedModules = @(
	"TietzeIO.CyShell.psm1"
)

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @(
	"Backup-CylanceOpticsPackages",
	"Backup-CylanceTenant",
	"Invoke-CylanceIssuerSignatureMissingWizard"
)

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
# CmdletsToExport = @()

# Variables to export from this module
# VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
# AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
FileList = @(
	"CHANGELOG.md",
	"LICENSE.md",
	"License.txt",
	"README.md",
	"TietzeIO.CyShell.psd1",
	"TietzeIO.CyShell.psm1",
	"AnalyticsCmdlets.ps1",
	"TenantCmdlets.ps1",
	"CsvHelper.dll",
	"EPPlus.dll",
	"Flurl.dll",
	"Flurl.Http.dll",
	"Microsoft.CSharp.dll",
	"Microsoft.Data.Sqlite.dll",
	"Microsoft.DotNet.PlatformAbstractions.dll",
	"Microsoft.EntityFrameworkCore.Abstractions.dll",
	"Microsoft.EntityFrameworkCore.dll",
	"Microsoft.EntityFrameworkCore.Relational.dll",
	"Microsoft.EntityFrameworkCore.Sqlite.dll",
	"Microsoft.Extensions.Caching.Abstractions.dll",
	"Microsoft.Extensions.Caching.Memory.dll",
	"Microsoft.Extensions.Configuration.Abstractions.dll",
	"Microsoft.Extensions.Configuration.Binder.dll",
	"Microsoft.Extensions.Configuration.dll",
	"Microsoft.Extensions.Configuration.FileExtensions.dll",
	"Microsoft.Extensions.Configuration.Json.dll",
	"Microsoft.Extensions.DependencyInjection.Abstractions.dll",
	"Microsoft.Extensions.DependencyInjection.dll",
	"Microsoft.Extensions.DependencyModel.dll",
	"Microsoft.Extensions.FileProviders.Abstractions.dll",
	"Microsoft.Extensions.FileProviders.Physical.dll",
	"Microsoft.Extensions.FileSystemGlobbing.dll",
	"Microsoft.Extensions.Logging.Abstractions.dll",
	"Microsoft.Extensions.Logging.dll",
	"Microsoft.Extensions.Options.dll",
	"Microsoft.Extensions.Primitives.dll",
	"Newtonsoft.Json.dll",
	"Nito.AsyncEx.Tasks.dll",
	"Nito.Disposables.dll",
	"NLog.dll",
	"Polly.dll",
	"Remotion.Linq.dll",
	"SQLitePCLRaw.batteries_green.dll",
	"SQLitePCLRaw.batteries_v2.dll",
	"SQLitePCLRaw.core.dll",
	"SQLitePCLRaw.provider.e_sqlite3.dll",
	"System.AppContext.dll",
	"System.Buffers.dll",
	"System.Collections.Concurrent.dll",
	"System.Collections.Immutable.dll",
	"System.Collections.NonGeneric.dll",
	"System.Collections.Specialized.dll",
	"System.ComponentModel.Annotations.dll",
	"System.ComponentModel.dll",
	"System.ComponentModel.Primitives.dll",
	"System.ComponentModel.TypeConverter.dll",
	"System.Data.Common.dll",
	"System.Diagnostics.DiagnosticSource.dll",
	"System.Drawing.Common.dll",
	"System.Dynamic.Runtime.dll",
	"System.Interactive.Async.dll",
	"System.IO.FileSystem.Primitives.dll",
	"System.Linq.dll",
	"System.Linq.Expressions.dll",
	"System.Linq.Queryable.dll",
	"System.Management.Automation.dll",
	"System.Memory.dll",
	"System.Numerics.Vectors.dll",
	"System.ObjectModel.dll",
	"System.Reflection.Emit.dll",
	"System.Reflection.Emit.ILGeneration.dll",
	"System.Reflection.Emit.Lightweight.dll",
	"System.Reflection.TypeExtensions.dll",
	"System.Runtime.CompilerServices.Unsafe.dll",
	"System.Runtime.Numerics.dll",
	"System.Security.Claims.dll",
	"System.Security.Cryptography.Cng.dll",
	"System.Security.Cryptography.OpenSsl.dll",
	"System.Security.Cryptography.Pkcs.dll",
	"System.Security.Cryptography.Primitives.dll",
	"System.Security.Principal.dll",
	"System.Text.Encoding.CodePages.dll",
	"System.Text.RegularExpressions.dll",
	"System.Threading.dll",
	"System.Threading.Tasks.Extensions.dll",
	"System.Xml.ReaderWriter.dll",
	"System.Xml.XmlDocument.dll",
	"System.Xml.XPath.dll",
	"System.Xml.XPath.XmlDocument.dll",
	"TietzeIO.CyAPI.CAE.dll",
	"TietzeIO.CyAPI.Configuration.DPAPI.dll",
	"TietzeIO.CyAPI.Configuration.Plaintext.dll",
	"TietzeIO.CyAPI.dll",
	"TietzeIO.CyAPI.Polyfills.dll",
	"TietzeIO.CyShell.dll",
	"en-us\TietzeIO.CyShell.dll-Help.xml"
	"TietzeIO.CyShell.dll-Help.xml"
)

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        RequireLicenseAcceptance = $true

        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @('cylance', 'api', 'optics')

        # A URL to the license for this module.
        LicenseUri = 'https://github.com/jan-tee/TietzeIO.CyShell/blob/master/LICENSE.md'

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/jan-tee/TietzeIO.CyShell'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        ReleaseNotes = 'https://github.com/jan-tee/TietzeIO.CyShell/blob/master/CHANGELOG.md'

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''
}