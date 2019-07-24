---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Get-CylanceDevices
c
## SYNOPSIS
Returns a list of all registered devices.

## SYNTAX

```
Get-CylanceDevices [-Api <ApiConnectionHandle>] [-Zone <CyZone>] [<CommonParameters>]
```

## DESCRIPTION
Returns a list of all registered devices in the Cylance console. Example Properties are shown below

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-CylanceDevices
date_first_registered : 7/16/2019 4:36:03 PM
policy                : Sample Policy (123344558-7544-1337-ab013-3322432423437378)
state                 : Offline
agent_version         : 2.0.1534
ip_addresses          : {10.10.10.20, 192.168.1.100}
mac_addresses         : {00-0C-29-E2-F8-DD, 00-0C-29-E2-F8-E7}
device_id             : 234f3abf2-c5d4-3347-1337-b4931a7df7ec
device_name           : Malware-Laptop

```

A list of the above item will be returned.

### Example 2
Getting device details from a list of devices
```powershell
PS C:\> Get-CylanceDevices | where device_name -like "Malware*" | Get-CylanceDevice
host_name             : Malware-Laptop
os_version            : Microsoft Windows 7 Professional, Service Pack 1
last_logged_in_user   : Malware-Laptop\Demo
distinguished_name    :
update_type           :
update_available      : False
background_detection  : False
is_safe               : True
date_last_modified    :
date_offline          :
Zones                 :
date_first_registered : 11/26/2018 5:09:16 PM
policy                : Sample Policy (123344558-7544-1337-ab013-3322432423437378)
state                 : Online
agent_version         : 2.0.1534
ip_addresses          : {10.10.10.20, 192.168.1.100}
mac_addresses         : {00-0C-29-E2-F8-DD, 00-0C-29-E2-F8-E7}
device_id             : 234f3abf2-c5d4-3347-1337-b4931a7df7ec
device_name           : Malware-Laptop
```

## PARAMETERS

### -Api
An API session handle (global session handle will be used if none exists in the current session).

```yaml
Type: ApiConnectionHandle
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Zone
{{ Fill Zone Description }}

```yaml
Type: CyZone
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### TietzeIO.CyAPI.Entities.CyDeviceBase

## NOTES

## RELATED LINKS
