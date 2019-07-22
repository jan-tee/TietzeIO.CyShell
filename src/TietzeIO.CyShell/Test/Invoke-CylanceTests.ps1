<#

#>

$timestamp = get-date -Format "yyyy.MM.dd_HH.mm.ss"
Connect-Cylance CyAPI-Test -Verbose
$detections = Get-CylanceDetections
$devices = Get-CylanceDevices
$devices_details = Get-CylanceDevices | Get-CylanceDeviceParallel
$policies = Get-CylancePolicies
$new_zone = New-CylanceZone -Name "TestZone $($timestamp)" -Policy $policies[1]
$devices | add-cylancedevicetozone -Zone $new_zone
$devices | where device_name -like "*op*" | remove-cylancedevicefromzone -Zone $new_zone
$policy = new-cylancepolicy -Name "TestPolicy $($timestamp)"
$policy_details = $policy | get-cylancepolicy
$policy | remove-cylancepolicy