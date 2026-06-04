param(
    [string]$TaskName = 'InquiryDesk Auto Start',
    [string]$ShortcutName = 'InquiryDesk Auto Start.lnk'
)

$ErrorActionPreference = 'Stop'

$startScript = (Resolve-Path (Join-Path $PSScriptRoot 'start-inquirydesk.ps1')).Path
$actionArguments = "-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File `"$startScript`""

try {
    $action = New-ScheduledTaskAction `
        -Execute 'powershell.exe' `
        -Argument $actionArguments

    $trigger = New-ScheduledTaskTrigger -AtLogOn
    $settings = New-ScheduledTaskSettingsSet `
        -AllowStartIfOnBatteries `
        -DontStopIfGoingOnBatteries `
        -StartWhenAvailable

    Register-ScheduledTask `
        -TaskName $TaskName `
        -Action $action `
        -Trigger $trigger `
        -Settings $settings `
        -Description 'Start InquiryDesk ASP.NET Core app at user logon.' `
        -Force | Out-Null

    Write-Host "Registered startup task: $TaskName"
    exit 0
}
catch {
    Write-Warning "Scheduled task registration failed. Falling back to Startup folder shortcut."
    Write-Warning $_.Exception.Message
}

$startupDirectory = [Environment]::GetFolderPath('Startup')
$shortcutPath = Join-Path $startupDirectory $ShortcutName
$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($shortcutPath)
$shortcut.TargetPath = 'powershell.exe'
$shortcut.Arguments = $actionArguments
$shortcut.WorkingDirectory = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$shortcut.WindowStyle = 7
$shortcut.Description = 'Start InquiryDesk ASP.NET Core app at user logon.'
$shortcut.Save()

Write-Host "Registered startup shortcut: $shortcutPath"
