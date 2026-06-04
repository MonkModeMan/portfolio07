param(
    [string]$TaskName = 'InquiryDesk Auto Start'
)

$ErrorActionPreference = 'Stop'

$startScript = (Resolve-Path (Join-Path $PSScriptRoot 'start-inquirydesk.ps1')).Path
$actionArguments = "-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File `"$startScript`""

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
