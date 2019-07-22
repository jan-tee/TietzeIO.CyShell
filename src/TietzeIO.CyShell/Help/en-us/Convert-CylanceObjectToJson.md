---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Convert-CylanceObjectToJson

## SYNOPSIS
Dumps an object to the same JSON form that would be sent to the console by the REST transaction.

## SYNTAX

```
Convert-CylanceObjectToJson -Object <ICyEntity> [<CommonParameters>]
```

## DESCRIPTION
This cmdlet is used to serialize an object to JSON using the same serialization options used by the CyShell module; this is typically what would be sent over the wire if you were to PUT or POST such an object.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-CylancePolicies | Get-CylancePolicy | Convert-CylanceObjectToJson
```

Returns the exact JSON representation for all policies in the tenant.

## PARAMETERS

### -Object
The object to dump to a JSON string.

```yaml
Type: ICyEntity
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### TietzeIO.CyAPI.Entities.ICyEntity

## OUTPUTS

### TietzeIO.CyShell.Session.ApiConnectionHandle

## NOTES

## RELATED LINKS
