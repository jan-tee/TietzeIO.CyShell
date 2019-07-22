<#
.SYNOPSIS
	This cmdlet checks if the tenant has signs of having a network configuration problem preventing
    the proper local validation of publisher certificates.
    
    It will offer to automatically set affected detections to "Reviewed" status, and add a helpful comment.
#>
function Invoke-CylanceIssuerSignatureMissingWizard()
{
	$rulesInstigating = @(
		"Unsigned Application Network Beaconing",
		"Oversized Image File",
		"Cylance Security Masquerader",
		"Internet Browser With Suspicious Parent"
	)
	$rulesTarget = @(
		"Cylance Security Masquerader",
		"Process Without Common Executable Extension",
		"Intentional File Name Confusion",
		"Internet Browser Launched Unsigned Process",
		"Office Launched Unsigned Process",
		"Suspicious OS Process Owner",
		"Suspicious OS Process Image Path"
	)

	$allrules = $rulesTarget + $rulesInstigating

	$a = (Get-CylanceCachedDetections -Detailed |
		Select-CylanceDetection -Rule $rulesInstigating |
		Where-Object { ($_ | Get-CylanceArtifact -Source "Instigating Process Image File").FileSignature.SignatureStatus -eq "IssuerSignatureMissing" })

	$b = (Get-CylanceCachedDetections -Detailed |
		Select-CylanceDetection -Rule $rulesTarget |
		Where-Object { ($_ | Get-CylanceArtifact -Source "Target Process Image File").FileSignature.SignatureStatus -eq "IssuerSignatureMissing" })

	Write-Host "Detections with problem in instigating process: $($a.count)"
	Write-Host "Detections with problem in target process:      $($b.count)"
	$c = Get-CylanceCachedDetections | Select-CylanceDetection -Rule $allrules
	Write-Host "Total detections examined:                      $($c.Count)"
	$t = Get-CylanceCachedDetections
	Write-Host "Total detections in tenant:                     $($t.Count)"
	$percentage = ($a.Count + $b.Count) / $t.Count
	Write-Host ("Percentage affected:                            {0:p}" -f $percentage)

	Set-Variable -Name DetectionsTypeA -Scope Global -Value $a
	Set-Variable -Name DetectionsTypeB -Scope Global -Value $b

	if ($percentage -gt 5) {
		Write-Host "More than 5% affected devices - this typically means the tenant is affected."
	} else {
		Write-Host "Less than 5% affected devices - this can mean the tenant is partially affected, e.g. only for certain sites or clients, or that agents are roaming a lot to networks where they do have the necessary access to cache the required certificates."
	}

	Write-Host "Based on these results, do you want to automatically mark affected detections as 'Reviewed' (Y/N)?"

	$answer = $host.UI.ReadLine()

	if ($answer -like "Y*") {

		if ($DetectionsTypeA -ne $null) {
			Write-Host "Fixing type 'Instigating Process Image File' type detections."
			$DetectionsTypeA | Update-CylanceDetection -Status Reviewed -Comment "This is a detection likely to be caused by missing WinHTTP network configuration when WinHTTP/CryptoAPI is unable to validate a publisher certificate. The publisher certificate may be valid, but the local system could not validate the signature's authenticity as a result."
		}
		if ($DetectionsTypeB -ne $null) {
			Write-Host "Fixing type 'Target Process Image File' type detections."
			$DetectionsTypeB | Update-CylanceDetection -Status Reviewed -Comment "This is a detection likely to be caused by missing WinHTTP network configuration when WinHTTP/CryptoAPI is unable to validate a publisher certificate. The publisher certificate may be valid, but the local system could not validate the signature's authenticity as a result."
		}
	}
}