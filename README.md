# TietzeIO.CyShell

[![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/TietzeIO.CyShell.svg)](https://www.powershellgallery.com/packages/TietzeIO.CyShell)
[![PowerShell Gallery Downloads](https://img.shields.io/powershellgallery/dt/TietzeIO.CyShell.svg)](https://www.powershellgallery.com/packages/TietzeIO.CyShell)

## What's this, and why should I use it

This is an open-source Powershell module and wrapper for the Cylance REST API.

It is similar to the
[CyCLI](https://github.com/jan-tee/cycli) module in that it provides Powershell support for these APIs, but it is implemented in C# and
offers many advantages:

 * **Strongly typed objects** - no more `PSCustomObject` return types make for easier coding
 * **Faster** - slow data conversion and single-threaded operation of the Powershell module made
   the Powershell module fairly slow.
 * More complete API coverage than CyCLI
 * Under active development

## How much faster is this?

|Scenario|CyCLI 0.9.5|TietzeIO.CyShell|
|---|---|---|
|Get 12943 threats using `Get-CylanceThreats` (TietzeIO.CyShell) vs. `Get-CyThreatList` (CyCLI)|33.85 seconds|6.08 seconds|
|Get 12942 devices using `Get-CylanceDevices` (TietzeIO.CyShell) vs. `Get-CyDeviceList` (CyCLI)|46.91 seconds|0.61 seconds|
|Get 170000 devices using `Get-CylanceDevices` (TietzeIO.CyShell) vs. `Get-CyDeviceList` (CyCLI)|1336.59|10.54 seconds|

## Installation

CyShell is released in the Powershell Gallery, and can be installed by:

```
install-module -name TietzeIO.CyShell
```

## Web Documentation

Documentation can be found [on jan-tee.github.io](https://jan-tee.github.io/TietzeIO.CyShell/TietzeIO.CyShell.html).

## Usage

Once TietzeIO.CyShell has been installed, you will need to setup a new connection to the Cylance API. You can do this by the following:

```powershell
New-CylanceConsole
Console: "Friendly Name"
APIId: <Your API ID from Cylance Console>
APISecret: <Your API Secret from Cylance Console>
APITenantId: <Your Tenant ID from Cylance Console>
Region: (apne1 | au | euc1 | sae1 | us-gov | us)
```
* Note: If you have previously setup CyCLI, you do not need to setup a new Console.

Next, you will need to connect to the console:

```powershell
Connect-Cylance "Friendly Name"
```

or

```powershell
Connect-Cylance "Friendly Name" -ProtectCache -OpticsCache
```

The '-ProtectCache' & '-OpticsCache' parameters will locally cache all data so you don't have to pull from the Cylance Tenant every
time you run a command. It also adds nice tab-completion for parameters.

Once you have connected to the Cylance Console, you'll have various commands available to you. You can get a list of commands by running:

```powershell
Get-Command -Module TietzeIO.CyShell
```
