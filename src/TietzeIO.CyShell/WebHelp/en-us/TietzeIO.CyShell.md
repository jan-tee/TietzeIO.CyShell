---
Module Name: TietzeIO.CyShell
Module Guid: dd4ee15c-01a2-484b-84e9-7176933af4c6
Download Help Link: none
Help Version: 28.0.0
Locale: en-us
---

# TietzeIO.CyShell Module
## Description
A PowerShell module for fast, easy, and reliable interaction with the Cylance REST API services.

## TietzeIO.CyShell Cmdlets
### [Backup-CylanceTenant](Backup-CylanceTenant.md)
Creates a back up of settings in a tenant

### [Invoke-CylanceIssuerSignatureMissingWizard](Invoke-CylanceIssuerSignatureMissingWizard.md)
Tests for OPTICS detection instances that are likely caused by a local configuration issue in the tenant environment (outside the Cylance configuration) preventing Windows from performing proper certificate validation checks

### [Add-CylanceDeviceToZone](Add-CylanceDeviceToZone.md)
Adds a device to a zone

### [Add-ToCylanceGlobalQuarantineList](Add-ToCylanceGlobalQuarantineList.md)
Adds a file hash to the Global Quarantine list

### [Add-ToCylanceGlobalSafeList](Add-ToCylanceGlobalSafeList.md)
Adds a file hash to the Global Safelist

### [Block-CylanceDeviceThreat](Block-CylanceDeviceThreat.md)
Blocks (quarantines) a file-based threat on a device

### [Clear-CylanceCache](Clear-CylanceCache.md)
Clears the session cache for the current session

### [Connect-Cylance](Connect-Cylance.md)
Connects to the Cylance API for a console

### [Convert-CylanceObjectToJson](Convert-CylanceObjectToJson.md)
Converts an object used by the module into the JSON representation typically sent to the REST backend (used mostly for troubleshooting purposes by module contributors)

### [Convert-CylanceTDRtoXLSX](Convert-CylanceTDRtoXLSX.md)
Converts a Cylance Threat Data Report to XLSX

### [Copy-CylancePolicy](Copy-CylancePolicy.md)
Creates a duplicate (clone) of a Cylance device policy

### [Disconnect-Cylance](Disconnect-Cylance.md)
Disconnects an API session

### [Enable-CylanceApplicationTemplate](Enable-CylanceApplicationTemplate.md)
Adds and enables an application template to a device policy

### [Get-CylanceArtifact](Get-CylanceArtifact.md)
Retrieves an artifact from a detection event

### [Get-CylanceCachedDetections](Get-CylanceCachedDetections.md)
Dumps cached detections from the current session

### [Get-CylanceCachedThreatDevices](Get-CylanceCachedThreatDevices.md)
Retrieves cached threat instances

### [Get-CylanceCachedThreats](Get-CylanceCachedThreats.md)
Retrieves cached threats

### [Get-CylanceConsole](Get-CylanceConsole.md)
Retrieves API/console configuration entries

### [Get-CylanceDetection](Get-CylanceDetection.md)
Retrieves a detection with details

### [Get-CylanceDetectionException](Get-CylanceDetectionException.md)
Retrieves a detection exception definition

### [Get-CylanceDetectionExceptions](Get-CylanceDetectionExceptions.md)
Retrieves metadata for all detection exceptions

### [Get-CylanceDetectionRule](Get-CylanceDetectionRule.md)
Retrieves a detection rule with details

### [Get-CylanceDetectionRules](Get-CylanceDetectionRules.md)
Retrieves metadata for all detection rules

### [Get-CylanceDetectionRuleSet](Get-CylanceDetectionRuleSet.md)
Retrieves a detection rule set with details

### [Get-CylanceDetectionRuleSets](Get-CylanceDetectionRuleSets.md)
Retrieves metadata for all detection rulesets

### [Get-CylanceDetections](Get-CylanceDetections.md)
Retrieves metadata for the 10000 most recent detections

### [Get-CylanceDevice](Get-CylanceDevice.md)
Retrieves details for a device

### [Get-CylanceDevices](Get-CylanceDevices.md)
Retrieves metadata for all devices

### [Get-CylanceDeviceThreats](Get-CylanceDeviceThreats.md)
Retrieves all threat instances for a device

### [Get-CylanceGlobalQuarantineList](Get-CylanceGlobalQuarantineList.md)
Retrieves the Global Quarantine list

### [Get-CylanceGlobalSafeList](Get-CylanceGlobalSafeList.md)
Retrieves the Global Safelist

### [Get-CylanceInstaQueries](Get-CylanceInstaQueries.md)
Retrieves metadata for all InstaQueries

### [Get-CylanceInstaQuery](Get-CylanceInstaQuery.md)
Retrieves all data for a particular InstaQuery

### [Get-CylanceLockdownHistory](Get-CylanceLockdownHistory.md)
Retrieves the lockdown history and certain metadata items for a device

### [Get-CylancePolicies](Get-CylancePolicies.md)
Retrievs metadata for all device policies

### [Get-CylancePolicy](Get-CylancePolicy.md)
Retrieves a device policy

### [Get-CylanceSession](Get-CylanceSession.md)
Returns the current API session

### [Get-CylanceTDR](Get-CylanceTDR.md)
Downloads a Threat Data Report

### [Get-CylanceThreat](Get-CylanceThreat.md)
Retrieves threat details for a file-based threat

### [Get-CylanceThreatDevices](Get-CylanceThreatDevices.md)
Retrieves device instance details for a particular threat

### [Get-CylanceThreats](Get-CylanceThreats.md)
Retrieves metadata for all file-based threats

### [Get-CylanceUser](Get-CylanceUser.md)
Retrieves a user object

### [Get-CylanceUsers](Get-CylanceUsers.md)
Retrieves metadata for all user objects

### [Get-CylanceVersion](Get-CylanceVersion.md)
Returns version information for this module

### [Get-CylanceZone](Get-CylanceZone.md)
Retrieves details for a zone

### [Get-CylanceZones](Get-CylanceZones.md)
Retrieves metadata for all zones

### [Hide-CylanceInstaQuery](Hide-CylanceInstaQuery.md)
Removes an InstaQuery from view in the console

### [Invoke-CylanceArtifactAnalysis](Invoke-CylanceArtifactAnalysis.md)
Performs an analysis of cached detection details and mines for commonality in artifacts, allowing easy identification of candidates for the creation of detection exclusions

### [Invoke-CylanceDetectionRuleAnalysis](Invoke-CylanceDetectionRuleAnalysis.md)
TODO OUTDATED Performs frequency analysis of detections by detection rule.

### [Invoke-CylanceDetectionStats](Invoke-CylanceDetectionStats.md)
Performs frequency analysis of detections by detection rule.

### [Invoke-CylanceThreatAnalysis](Invoke-CylanceThreatAnalysis.md)
Performs frequency analysis of cached threats by classification. Output can be used to quarantine/safelist

### [Invoke-CylanceThreatPathAnalysis](Invoke-CylanceThreatPathAnalysis.md)
Performs frequency analysis of cached threats by path. Output can be used to quarantine/safelist

### [New-CylanceConsole](New-CylanceConsole.md)
Creates a new console configuration entry in the configuration store

### [New-CylanceDetectionException](New-CylanceDetectionException.md)
Creates a new detection exception rule

### [New-CylanceDetectionRuleSet](New-CylanceDetectionRuleSet.md)
Creates a new detection rule set

### [New-CylanceInstaQuery](New-CylanceInstaQuery.md)
Starts a new InstaQuery

### [New-CylancePolicy](New-CylancePolicy.md)
Creates a new device policy

### [New-CylanceUser](New-CylanceUser.md)
Creates a new console user

### [New-CylanceZone](New-CylanceZone.md)
Creates a new zone for devices

### [Open-CylanceDetection](Open-CylanceDetection.md)
Opens a browser with the details view for a particular detection

### [Remove-CylanceConsole](Remove-CylanceConsole.md)
Removes a console configuration entry from the configuration store

### [Remove-CylanceDetection](Remove-CylanceDetection.md)
Removes a detection

### [Remove-CylanceDevice](Remove-CylanceDevice.md)
Removes a device

### [Remove-CylanceDeviceFromZone](Remove-CylanceDeviceFromZone.md)
Removes a device from a zone

### [Remove-CylancePolicy](Remove-CylancePolicy.md)
Removes a policy

### [Remove-CylanceZone](Remove-CylanceZone.md)
Removes a zone

### [Remove-FromCylanceGlobalQuarantineList](Remove-FromCylanceGlobalQuarantineList.md)
Removes a file hash from the Global Quarantine List

### [Remove-FromCylanceGlobalSafeList](Remove-FromCylanceGlobalSafeList.md)
Removes a file hash from the Global Safelist

### [Select-CylanceArtifact](Select-CylanceArtifact.md)
Fill in the Description

### [Select-CylanceDetection](Select-CylanceDetection.md)
Fill in the Description

### [Select-CylanceThreat](Select-CylanceThreat.md)
Fill in the Description

### [Select-CylanceThreatDevices](Select-CylanceThreatDevices.md)
Fill in the Description

### [Set-CylanceConsole](Set-CylanceConsole.md)
Fill in the Description

### [Set-CylancePolicyForDevice](Set-CylancePolicyForDevice.md)
Fill in the Description

### [Set-CylanceSession](Set-CylanceSession.md)
Fill in the Description

### [Sync-CylanceCache](Sync-CylanceCache.md)
Fill in the Description

### [Unblock-CylanceDeviceThreat](Unblock-CylanceDeviceThreat.md)
Fill in the Description

### [Update-CylanceDetection](Update-CylanceDetection.md)
Fill in the Description

### [Update-CylanceDetectionException](Update-CylanceDetectionException.md)
Fill in the Description

### [Update-CylancePolicy](Update-CylancePolicy.md)
Fill in the Description

### [Update-CylanceZone](Update-CylanceZone.md)
Fill in the Description

