---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Get-CylanceArtifact

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
Get-CylanceArtifact -Detection <CyDetection> [-Source <String>] [-ArtifactType <String>]
 [-Api <ApiConnectionHandle>] [<CommonParameters>]
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

### -ArtifactType
{{ Fill ArtifactType Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: File, Process, User

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Detection
{{ Fill Detection Description }}

```yaml
Type: CyDetection
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Source
{{ Fill Source Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Instigating Process, Instigating Process Image File, Target Process, Target Process Image File, Instigating Process Owner, Target Process Owner, Target File

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### TietzeIO.CyAPI.Entities.Optics.CyDetection

## OUTPUTS

### TietzeIO.CyAPI.Entities.CyDeviceMetaData

## NOTES

## RELATED LINKS
