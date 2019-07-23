---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Enable-CylanceApplicationTemplate

## SYNOPSIS
Modifies a policy object to enable exclusions for certain known applications, based on module-supplied templates.

## SYNTAX

```
Enable-CylanceApplicationTemplate [-Policy] <CyPolicy> [-Template] <String[]> [<CommonParameters>]
```

## DESCRIPTION
{{Applies script control, memory protection, and file scan exclusions from an application template.

Application templates are shipped with the module.}}

## EXAMPLES

### Example 1

{{To fetch the policy "SomePolicy", modify the policy to enable the VirtualBox_Windows application, and update the policy in the console, use this:}}

```powershell
{{PS C:\> Get-CylancePolicy -Name 'SomePolicy' | Enable-CylanceApplicationTemplate -Template VirtualBox_Windows | Update-CylancePolicy}}
```

## PARAMETERS

### -Policy
Policy object

```yaml
Type: CyPolicy
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Template
Application template name

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Defender, FireEye_HX_Windows, Kaspersky_Windows, McAfee_Linux, McAfee_Windows, SCCM, SophosAV_Windows, Symantec_Windows, Tanium_Linux, Tanium_macOS, Tanium_Windows, TrendMicro_Windows, VirtualBox_Windows, Windows

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### TietzeIO.CyAPI.Entities.Policy.CyPolicy

## OUTPUTS

### TietzeIO.CyAPI.Entities.Policy.CyPolicy

## NOTES

## RELATED LINKS
