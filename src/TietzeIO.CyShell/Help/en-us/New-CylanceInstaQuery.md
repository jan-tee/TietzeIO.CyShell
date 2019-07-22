---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# New-CylanceInstaQuery

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
New-CylanceInstaQuery -Name <String> [-Description <String>] -QueryType <String> [-CaseSensitive] [-ExactMatch]
 -Query <String[]> -Zones <CyZone[]> [-OS <String[]>] [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

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

### -CaseSensitive
{{ Fill CaseSensitive Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
{{ Fill Description Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExactMatch
{{ Fill ExactMatch Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
{{ Fill Name Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OS
{{ Fill OS Description }}

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Windows, MacOS

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Query
{{ Fill Query Description }}

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -QueryType
{{ Fill QueryType Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: File.Path, File.MD5, File.SHA256, File.Owner, File.CreationDateTime, Process.Name, Process.CommandLine, Process.PrimaryImagePath, Process.PrimaryImageMd5, Process.StartDateTime, NetworkConnection.DestAddr, NetworkConnection.DestPort, RegistryKey.ProcessName, RegistryKey.ProcessPrimaryImagePath, RegistryKey.ValueName, RegistryKey.FilePath, RegistryKey.FileMd5, RegistryKey.IsPersistencePoint

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Zones
{{ Fill Zones Description }}

```yaml
Type: CyZone[]
Parameter Sets: (All)
Aliases:

Required: True
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

### TietzeIO.CyAPI.Entities.Optics.CyInstaQuery

## NOTES

## RELATED LINKS
