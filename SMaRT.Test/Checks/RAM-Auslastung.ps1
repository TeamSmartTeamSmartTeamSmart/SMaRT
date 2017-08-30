$os = WmiObject -Class win32_operatingsystem
$freeRamRelative = $os.FreePhysicalMemory / $os.TotalVisibleMemorySize
$freeRamAbsolute = $os.FreePhysicalMemory / (1024*1024)
$totalRam = $os.TotalVisibleMemorySize / (1024*1024)

if ($freeRamRelative -gt 0.20) {
    Write-Output 1 # Success
} elseif ($freeRamRelative -gt 0.05) {
    Write-Output 2 # Warning
} else {
    Write-Output 3 # Error
}

"Freier Arbeitsspeicher: {0:P0} ({1:N2}/{2:N2} Gb)" -f $freeRamRelative, $freeRamAbsolute, $totalRam