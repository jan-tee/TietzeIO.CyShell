# Changelog

## v.14.0.0
 * Fixed a bug in Enable-CylanceApplicationTemplate that prevented saving the resultant policy
 * Enhanced parallel execution for more cmdlets (Remove-CylanceDetection, Update-CylanceDetection)

## v.13.0.0
 * Enhanced parallel execution of certain cmdlets, now with progress reporting and ETA

## v.8.0.0
* Added new cmdlets
* Added support for passing credentials in plain text as an argument BY POPULAR DEMAND... storage is still DPAPI protected
* Added Invoke-CylanceThreatAnalysis to group detections by artifact

## v.2.0.0
* Added new cmdlets
* Parallel execution added to: Get-CylanceDetection, Get-CylanceDevice
* 8 threads by default
* Added convenience cmdlet: Copy-CylancePolicy
* Added convenience modes when "-Cache" specified in "Connect-Cylance", or "Set-CylanceSession -Cache $true": Get-CylanceDevice -Name, Get-CylanceDetection -Device, -Rule, -PhoneticId..., Get-CylanceUser -Email with autocompletion in Cache modes!
* Still no solution for early termination of pipeline (Ctrl-C...)

## v.1.5.2
* Updated device metadata so that "last modified" can be null.

## v.1.5.1
* Added new base objet CyDeviceBase so that "Get Devices for zone" transactions does not appear to return empty fields
* Updated related Cmdlets (Get-CylanceDevice, Get-CylanceDevices) to accept/return this base type as appropriate

## v.0.0.5
* Added parallel request execution to all paged requests
* Session support now supports proper Verbose logging to Powershell across threads

## v.0.0.2
* Added basic cmdlets

## v.0.0.1
* Moved from Microsoft JWT library to 3rd party
