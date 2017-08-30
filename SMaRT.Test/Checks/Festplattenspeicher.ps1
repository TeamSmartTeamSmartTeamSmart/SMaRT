$volume = Get-Volume -DriveLetter c
$freeSpace = $volume.SizeRemaining / $volume.Size
$healthstatus = $volume.HealthStatus
$totalSize = $volume.Size

if ($healthstatus -ne "Healthy") {
    Write-Output 3; # Error
} elseif ($freeSpace -gt 0.20) {
    Write-Output 1; # Success
} elseif ($freeSpace -gt 0.05) {
    Write-Output 2; # Warning
} else {
    Write-Output 3; # Error
}

"Freier Speicherplatz auf dem Laufwerk 'C': {0:P0} von " -f $freeSpace
"Laufwerkszustand: $healthstatus"