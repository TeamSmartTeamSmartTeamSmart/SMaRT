param (
    [string]$processName,
    [string]$maxRam
)

$processes = Get-Process $processName
$count =  ($processes).Count
$ramUsed = $processes | Measure PagedMemorySize64 -Sum -Average
$threadInfo = $processes | Select -ExpandProperty Threads | Measure Id -Sum

if (($ramUsed.Sum / (1024*1024)) -gt $maxRam) {
    Write-Output 3; # Error
} elseif ($count -eq 0) {
    Write-Output 3; # Error
} else {
    Write-Output 1; # Success
}

"$count Prozesse mit dem Namen '$processName'"
"Gesamter RAM-Verbrauch: {0:N2}(durchschnittlich {1:N2}) Mb" -f ($ramUsed.Sum / (1024 * 1024)), ($ramUsed.Average / (1024 * 1024))
"Gesamtzahl Threads: {0}" -f $threadInfo.Count