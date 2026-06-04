param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot '..\InquiryDesk.csproj')).Path,
    [string]$WorkingDirectory = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,
    [string]$Urls = 'http://localhost:5052'
)

$ErrorActionPreference = 'Stop'

$uri = [Uri]$Urls
$port = $uri.Port
$existingListener = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue

if ($existingListener) {
    exit 0
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
    $Urls
)

Start-Process `
    -FilePath 'dotnet' `
    -ArgumentList $arguments `
    -WorkingDirectory $WorkingDirectory `
    -WindowStyle Hidden `
    -RedirectStandardOutput $stdoutLog `
    -RedirectStandardError $stderrLog
