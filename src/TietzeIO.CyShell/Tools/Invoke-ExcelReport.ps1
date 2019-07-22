$Timestamp = get-date -Format "yyyyMMdd-hhmmss"
$File = "$($Timestamp)_$($TenantID)_Report.xlsx"

$a = Invoke-CylanceThreatAnalysis
$a | %{
    [pscustomobject]@{
        Count = $_.Count
        Classification = $_.Classification
        Hashes = [String]::Join(", ", ($_.Threats.sha256 | Select -Unique) )
    }
} | export-excel -Path $File -WorksheetName "PROTECT Overview" -AutoFilter -AutoSize -AutoNameRange -Title "PROTECT Overview"

$a = Invoke-CylanceThreatPathAnalysis
$a | %{
        [pscustomobject]@{
            Count = $_.Count
            Path = $_.Path
            Devices = [String]::Join(", ", ($_.ThreatDevices.device_name | Select -Unique) )
            PUP = $_.PUP
            Trusted = $_.Trusted
            Malware = $_.Malware
            DualUse = $_.DualUse
            Unclassified = $_.Unclassified
        }
} | export-excel -Path $File -WorksheetName "Threats by Path" -AutoFilter -AutoSize -AutoNameRange -Title "Threats by Path"

$a = Invoke-CylanceArtifactAnalysis -PerVersionStats
$a | export-excel -Path $File -WorksheetName "OPTICS Overview" -AutoFilter -AutoSize -AutoNameRange -Title "OPTICS Overview"
$a | %{ 
    $name =$_.RuleName
    $version =$_.RuleVersion
    $_.buckets | %{
        [pscustomobject]@{
            Type = $_.Type
            Name = $_.Name
            Identifier = $_.Identifier
            Hits = $_.Hits
            PhoneticIds = [String]::Join( ", ", $_.Members.Detection.PhoneticId )
        }
    } | export-excel -Path $File -WorksheetName "$($name) ($($version))" -AutoFilter -AutoSize -AutoNameRange -Title "OPTICS Rule: $($name) ($($version))"
}
