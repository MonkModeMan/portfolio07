param(
    [string]$TaskName = 'InquiryDesk Auto Start',
    [string]$ShortcutName = 'InquiryDesk Auto Start.lnk'
)

$ErrorActionPreference = 'Stop'

$removed = $false

try {
    $task = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue

    if ($task) {
        Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
        Write-Host "Unregistered startup task: $TaskName"
        $removed = $true
    }
}
catch {
    Write-Warning "Scheduled task removal failed. Continuing with Startup folder cleanup."
    Write-Warning $_.Exception.Message
}

$startupDirectory = [Environment]::GetFolderPath('Startup')
$shortcutPath = Join-Path $startupDirectory $ShortcutName

if (Test-Path -LiteralPath $shortcutPath) {
    Remove-Item -LiteralPath $shortcutPath -Force
    Write-Host "Removed startup shortcut: $shortcutPath"
    $removed = $true
}

if (-not $removed) {
    Write-Host "Startup registration was not found."
}
