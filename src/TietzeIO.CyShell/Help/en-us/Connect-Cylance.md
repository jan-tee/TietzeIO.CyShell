---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# Connect-Cylance

## SYNOPSIS
Connects to the specified console

## SYNTAX

```
Connect-Cylance [-Scope <String>] [-ProtectCache] [-OpticsCache] [-ProxyServer <String>] [[-Console] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Connects to the Cylance console specified and establish an authenticated session using the specified console configuration profile.

Console configuration profiles hold the API credentials. They can be created and removed using `New-CylanceConsole` and `Remove-CylanceConsole`.

It can also initialize caches as you connect.

## EXAMPLES

### Example 1
```powershell
PS C:\> Connect-Cylance MyTenant -ProtectCache -OpticsCache
```

This will connect to the Console named "MyTenant" in your consoles file (typically consoles.json, but different implementations of configuration storage are available;
you can create consoles using the `New-CylanceConsole` command), and populate the Protect (e.g. device, policy, zone data) and Optics (detections, detection rules) caches 
that can be used later to e.g. refer to policies by name, and use tab auto-completion for certain fields when used interactively.

## PARAMETERS

### -Console
The console record in the console definition file.

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

### -OpticsCache
Populates the OPTICS cache.

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

### -ProtectCache
Populates the PROTECT cache.

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

### -ProxyServer
Proxy server to use, if any

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
By default, connections are global. This is best for interactive sessions. If you wish to connect to multiple consoles in parallel, you should use `-Scope None` and then supply the API handle returned to every cmdlet explicitly.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Session, None

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

### TietzeIO.CyShell.Session.ApiConnectionHandle

## NOTES

## RELATED LINKS
