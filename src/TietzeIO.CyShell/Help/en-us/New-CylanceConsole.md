---
external help file: TietzeIO.CyShell.dll-Help.xml
Module Name: TietzeIO.CyShell
online version:
schema: 2.0.0
---

# New-CylanceConsole

## SYNOPSIS
Add a new Cylance Console to your CyShell local Console.json file

## SYNTAX

```
New-CylanceConsole -Console <String> -APIId <String> -APISecret <PSObject> -APITenantId <String>
 [-TDRToken <String>] -Region <String> [-LoginUser <String>] [-LoginPassword <PSObject>]
 [-ProxyServer <String>] [-NoVerify] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> New-CylanceConsole

cmdlet New-CylanceConsole at command pipeline position 1
Supply values for the following parameters:
Console: SETenant
APIId: Your-API-ID
APISecret: Your-API-Secret
APITenantId: Your-API-Tenant-ID
Region: (apne1|au|euc1|sae1|us-gov|us)
```

Use 'us' for US consoles, 'euc1' for Europe, and for any other console, use the appropriate suffix from your console login page URL.

## PARAMETERS

### -APIId
API ID found in the Settings->Integrations tab of the Cylance Console

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

### -APISecret
API Secret found in the Settings->Integrations tab of Cylance Console

```yaml
Type: PSObject
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -APITenantId
API Tenant ID found in the Settings->Integrations tab of Cylance Console

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

### -Console
{{ Fill Console Description }}

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

### -LoginPassword
{{ Fill LoginPassword Description }}

```yaml
Type: PSObject
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LoginUser
{{ Fill LoginUser Description }}

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

### -NoVerify
{{ Fill NoVerify Description }}

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
{{ Fill ProxyServer Description }}

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

### -Region
{{ Fill Region Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: apne1, au, euc1, sae1, us-gov, us

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TDRToken
{{ Fill TDRToken Description }}

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
