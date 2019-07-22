---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Add-ToCylanceGlobalSafeList

## SYNOPSIS
Adds an entry to global safelist. 

## SYNTAX

```
Add-ToCylanceGlobalSafeList -Category <String> -Threat <PSObject> [-Api <ApiConnectionHandle>] -Reason <String>
 [<CommonParameters>]
```

## DESCRIPTION
Adds a hash entry to the Global Safelist.

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

### -Category
{{ Fill Category Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Admin Tool, Drivers, Internal Application, Operating System, Security Software, None

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Reason
{{ Fill Reason Description }}

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

### -Threat
{{ Fill Threat Description }}

```yaml
Type: PSObject
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

### System.Management.Automation.PSObject

## OUTPUTS

### TietzeIO.CyAPI.Rest.CyRestAddToGlobalList

## NOTES

## RELATED LINKS
