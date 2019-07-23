function Backup-CylanceOpticsPackages() {
	 Get-CylancePackages | Get-CylancePackage | ForEach-Object {
	 	 $name = $_.packageDescriptor.name
		 $file = $_.PackageFile
		 $version = $_.packageDescriptor.version

		 if (![string]::isnullorempty($file)) {
			$outfile = "$($name)_v$($version).zip"
			Write-Host "Writing package: $($name) to $($outfile)"
			Move-Item -Path $file -Destination $outfile
		 }
	 }
}

function Backup-CylanceTenant()
{
	if ((Get-CylanceSession) -eq $null) {
		throw "Not connected."
	}

	$Timestamp = get-date -Format "yyyyMMdd-HHmmss"
	$File = "$($Timestamp)_$($TenantID)_Backup.json"
	$TenantID = (Get-CylanceSession).Session.APITenantId

	Write-Host "Starting backup process for tenant $($TenantID)"

	Write-Host -NoNewline "Retrieving devices... "
	$Devices = Get-CylanceDevices
	Write-Host -ForegroundColor Green "Done"

	Write-Host -NoNewline "Retrieving policies... "
	$Policies = Get-CylancePolicies | Get-CylancePolicy
	Write-Host -ForegroundColor Green "Done"

	Write-Host -NoNewline "Retrieving zones... "
	$Zones = Get-CylanceZones | Get-CylanceZone
	Write-Host -ForegroundColor Green "Done"

	Write-Host -NoNewline "Retrieving users... "
	$Users = Get-CylanceUsers | Get-CylanceUser
	Write-Host -ForegroundColor Green "Done"

	Write-Host -NoNewline "Retrieving detection rulesets..."
	$DetectionRulesets = Get-CylanceDetectionRulesets | Get-CylanceDetectionRuleSet
	Write-Host -ForegroundColor Green "Done"

	$Backup = @{
		Timestamp = "$($Timestamp)"
		TenantConsoleID = $TenantID
		TenantID = $TenantID
		Devices = $Devices
		Policies = $Policies
		DetectionRulesets = $DetectionRulesets
		Zones = $Zones
		Users = $Users
	}

	ConvertTo-Json $Backup -Depth 100 | Out-File $File
}