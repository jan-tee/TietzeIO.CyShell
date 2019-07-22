---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Invoke-CylanceThreatPathAnalysis

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
Invoke-CylanceThreatPathAnalysis [-OrderBy <String>] [-FileStatus <String>] [-IgnoreRecycleBin]
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

### -FileStatus
{{ Fill FileStatus Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Unsafe, Abnormal, Waived, Default

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IgnoreRecycleBin
{{ Fill IgnoreRecycleBin Description }}

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

### -OrderBy
{{ Fill OrderBy Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Path, Count

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

### TietzeIO.CyAPI.Entities.CyDeviceMetaData

## NOTES

## RELATED LINKS
