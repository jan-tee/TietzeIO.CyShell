# TietzeIO.CyShell

## What's this, and why should I use it

This is an open-source Powershell module and wrapper for the Cylance REST API.

It is similar to the
[CyCLI](https://github.com/jan-tee/cycli) module in that it provides Powershell support for these APIs, but it is implemented in C# and
offers many advantages:

 * **Strongly typed objects** - no more `PSCustomObject` return types make for easier coding
 * **Faster** - slow data conversion and single-threaded operation of the Powershell module made
   the Powershell module fairly slow.
 * More complete API coverage than CyCLI
 * Under active development
 
## How much faster is this?

|Scenario|CyCLI 0.9.5|cyapi-v2-powershell|
|---|---|---|
|Get 12943 threats using `Get-CylanceThreats` (TietzeIO.CyShell) vs. `Get-CyThreatList` (CyCLI)|33.85 seconds|6.08 seconds|
|Get 12942 devices using `Get-CylanceDevices` (TietzeIO.CyShell) vs. `Get-CyDeviceList` (CyCLI)|46.91 seconds|0.61 seconds|
|Get 170000 devices using `Get-CylanceDevices` (TietzeIO.CyShell) vs. `Get-CyDeviceList` (CyCLI)|1336.59|10.54 seconds|

