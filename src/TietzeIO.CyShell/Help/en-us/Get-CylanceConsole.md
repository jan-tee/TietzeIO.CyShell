---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Get-CylanceConsole

## SYNOPSIS
Returns a Cylance console configuration profile

## SYNTAX

```
Get-CylanceConsole [[-Console] <String>] [<CommonParameters>]
```

## DESCRIPTION
Returns the Cylance console configuration profile for the specified console name.

Console configuration profiles hold the API credentials. They can be created and removed using `New-CylanceConsole` and `Remove-CylanceConsole`.

Supports auto-completion for the `-Console` argument.

## EXAMPLES

### Example 1
Connects to console `QA`

```powershell
PS C:\> Connect-Cylance -Console QA
```

## PARAMETERS

### -Console

The console configuration (API account definition) to use.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: <console profile name, use tab key for autocompletion>

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
