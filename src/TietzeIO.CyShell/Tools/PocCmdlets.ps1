<#
.SYNOPSIS
    This cmdlet will autocreate the zones / policies used in most POCs.

    Device Policies Created:
        1. Crawl - Alert Only
        2. AQT + Optics - Alert: SC, MP
        3. AQT + SC + MP + Optics

#>
function Invoke-CylancePOCSetup()
{
    $pname = "1. Crawl - Alert Only"
    Write-Host "Creating Policy: $pname"
    $p = New-CylancePolicy -name "$pname"

    # Enable Script Control
    $pname = "2. Walk - AQT + Optics - Alert: SC, MP, DC"
    Write-Host "Creating: $pname"
    $p = New-CylancePolicy -name "$pname"

    #Tried setting Enum val directly, but that didn't work
    $p.filetype_actions.SetSuspiciousFileTypeAction(3,"executable")
    $p.filetype_actions.SetThreatFileTypeAction(3, "executable")
    # $p.filetype_actions.suspicious_files.actions.value__ = 3
    # $p.filetype_actions.threat_files.actions.value__ = 3

    $p.policy | where name -eq "script_control" |foreach { $_.value = 1 }
    $p.policy | where name -eq "memory_exploit_detection" |foreach { $_.value = 1 }
    $p.policy | where name -eq "optics" |foreach { $_.value = 1 }
    $p.policy | where name -eq "optics_application_control_auto_upload" |foreach { $_.value = 1 }
    $p.policy | where name -eq "optics_malware_auto_upload" |foreach { $_.value = 1 }
    $p.policy | where name -eq "optics_memory_defense_auto_upload" |foreach { $_.value = 1 }
    $p.policy | where name -eq "optics_script_control_auto_upload" |foreach { $_.value = 1 }

    # write-host $p
    # write-host $p.policy
    $p | update-cylancepolicy

    $pname = "3. Run - AQT, SC, MP + Optics - Alert: DC"
    Write-Host "Creating $pname"
    $p = Get-CylancePolicies | where name -like "2. Walk*" | Get-CylancePolicy
    $p = Copy-CylancePolicy -Source $p -TargetName "$pname"

    $p.memoryviolation_actions.memory_violations | foreach { $_.action = "Block"}
    $p.memoryviolation_actions.memory_violations_ext | foreach { $_.action = "Block"}

    $p | update-cylancepolicy
}
