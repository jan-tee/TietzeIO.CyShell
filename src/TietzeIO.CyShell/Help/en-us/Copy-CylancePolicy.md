---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Copy-CylancePolicy

## SYNOPSIS
Creates a clone of a Cylance policy

## SYNTAX

```
Copy-CylancePolicy -Source <CyPolicy> -TargetName <String> [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

## DESCRIPTION
Clones an existing policy.

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

### -Source
Source policy object.

```yaml
Type: CyPolicy
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetName
Target policy name.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### TietzeIO.CyAPI.Entities.Policy.CyPolicy

## NOTES

## RELATED LINKS
