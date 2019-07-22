---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Get-CylanceInstaQuery

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

### ById
```
Get-CylanceInstaQuery [-Id <String>] [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

### ByInstaQuery
```
Get-CylanceInstaQuery [-InstaQuery <CyInstaQuery>] [-Api <ApiConnectionHandle>] [<CommonParameters>]
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

### -Id
{{ Fill Id Description }}

```yaml
Type: String
Parameter Sets: ById
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InstaQuery
{{ Fill InstaQuery Description }}

```yaml
Type: CyInstaQuery
Parameter Sets: ByInstaQuery
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### TietzeIO.CyAPI.Entities.Optics.CyInstaQuery

## OUTPUTS

### TietzeIO.CyAPI.Entities.Optics.CyInstaQuery

## NOTES

## RELATED LINKS
