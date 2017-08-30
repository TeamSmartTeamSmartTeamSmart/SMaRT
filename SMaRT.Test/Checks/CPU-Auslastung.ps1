$processorInfo = Get-WmiObject win32_processor
$avgUtilization = ($processorInfo | Measure-Object -property LoadPercentage -Average | Select Average).Average
$procCount = @(Get-Process).Count

if ($avgUtilization -lt 80) {
    Write-Output 1 # Success
} elseif($avgUtilization -lt 95) {
    Write-Output 2 # Warning
} else {
    Write-Output 3 # Error
}

"CPU Auslastung: {0:P0}" -f ($avgUtilization / 100)
"Laufende Prozesse: $procCount"