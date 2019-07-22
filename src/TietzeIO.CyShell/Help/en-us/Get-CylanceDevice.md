---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Get-CylanceDevice

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

### ByDevice
```
Get-CylanceDevice -Device <CyDeviceBase> [-WithZones] [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

### ByMAC
```
Get-CylanceDevice [-MAC <String>] [-WithZones] [-Api <ApiConnectionHandle>] [<CommonParameters>]
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

### -Device
{{ Fill Device Description }}

```yaml
Type: CyDeviceBase
Parameter Sets: ByDevice
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -MAC
{{ Fill MAC Description }}

```yaml
Type: String
Parameter Sets: ByMAC
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WithZones
{{ Fill WithZones Description }}

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### TietzeIO.CyAPI.Entities.CyDeviceBase

## OUTPUTS

### TietzeIO.CyAPI.Entities.CyDeviceMetaData

## NOTES

## RELATED LINKS
