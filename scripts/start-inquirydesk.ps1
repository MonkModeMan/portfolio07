param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot '..\InquiryDesk.csproj')).Path,
    [string]$WorkingDirectory = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,
    [string]$Urls = 'https://localhost:7052;http://localhost:5052'
)

$ErrorActionPreference = 'Stop'

$urlList = $Urls -split ';' | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
$ports = $urlList | ForEach-Object { ([Uri]$_).Port }
$listeningPorts = @(
    $ports | Where-Object {
        Get-NetTCPConnection -LocalPort $_ -State Listen -ErrorAction SilentlyContinue
    }
)

if ($listeningPorts.Count -eq $ports.Count) {
    exit 0
}

if ($listeningPorts.Count -gt 0) {
    $joinedPorts = $listeningPorts -join ', '
    Write-Warning "InquiryDesk is already partially listening on port(s): $joinedPorts. Stop the existing process before starting with all URLs."
    exit 1
}

$logDirectory = Join-Path $WorkingDirectory 'logs'
New-Item -ItemType Directory -Path $logDirectory -Force | Out-Null

$timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$stdoutLog = Join-Path $logDirectory "inquirydesk-$timestamp.out.log"
$stderrLog = Join-Path $logDirectory "inquirydesk-$timestamp.err.log"

$arguments = @(
    'run',
    '--project',
    "`"$ProjectPath`"",
    '--urls',
    "`"$Urls`""
)

Start-Process `
    -FilePath 'dotnet' `
    -ArgumentList $arguments `
    -WorkingDirectory $WorkingDirectory `
    -WindowStyle Hidden `
    -RedirectStandardOutput $stdoutLog `
    -RedirectStandardError $stderrLog
