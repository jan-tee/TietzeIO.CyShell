---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Remove-CylanceConsole

## SYNOPSIS
Removes a Cylance console configuration profile

## SYNTAX

```
Remove-CylanceConsole [-Console] <String> [<CommonParameters>]
```

## DESCRIPTION
Removes a Cylance console configuration profile from storage.

Console configuration profiles hold the API credentials. They can be created and removed using `New-CylanceConsole` and `Remove-CylanceConsole`.

## EXAMPLES

### Example 1
Removes console `OldConsole`

```powershell
PS C:\> Remove-CylanceConsole -Console OldConsole
```

## PARAMETERS

### -Console
{{ Fill Console Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: <console profile name, use tab key for autocompletion>

Required: True
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
