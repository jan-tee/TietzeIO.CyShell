---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Set-CylancePolicyForDevice

## SYNOPSIS
Assigns policy to a device.

## SYNTAX

```
Set-CylancePolicyForDevice -Device <CyDeviceBase> [-Policy <CyPolicyMinimalMetaData>]
 [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

## DESCRIPTION
Will apply an existing policy to a device.

## EXAMPLES

### Example 1
```powershell
PS C:\> $device = (Get-CylanceDevices)[0]
PS C:\> $policy = (Get-CylancePolicies)[1]
PS C:\> $device | Set-CylancePolicyForDevice -Policy $policy
```

Takes the first device in a tenant, and applies the second policy in the list to it (basically a randomly chosen policy, because order is not guaranteed).

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
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Policy
{{ Fill Policy Description }}

```yaml
Type: CyPolicyMinimalMetaData
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
