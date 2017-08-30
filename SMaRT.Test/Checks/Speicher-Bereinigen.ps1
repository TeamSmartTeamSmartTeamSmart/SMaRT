$volume = Get-Volume -DriveLetter c
$freeSpace = $volume.SizeRemaining / $volume.Size


if ($freeSpace -lt 0.05) {
    Get-ChildItem $env:TEMP -Recurse | Remove-Item -Force -Recurse
    Clear-RecycleBin

    $freeSpace = $volume.SizeRemaining / $volume.Size
    if ($freeSpace -lt 0.05) {
        Write-Output 3; # Error
    } else {
        Write-Output 2; # Warning
    }
} else {
    Write-Output 1; # Success
}