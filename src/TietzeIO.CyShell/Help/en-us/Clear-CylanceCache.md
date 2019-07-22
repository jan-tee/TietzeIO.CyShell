---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Clear-CylanceCache

## SYNOPSIS
Clears the Cylance session caches.

## SYNTAX

```
Clear-CylanceCache [-Api <ApiConnectionHandle>] [<CommonParameters>]
```

## DESCRIPTION
During the course of an API session, caches may be populated that enable auto-completion, other lookups, and analysis functions. You can clear these caches with this cmdlet.

## EXAMPLES

### Example 1
```powershell
PS C:\> Clear-CylanceCache
```

Clears the session caches.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
