$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Copy-Item "$root/fixtures/CheckoutCalculator.Fixed.cs" "$root/src/Store.Checkout/CheckoutCalculator.cs" -Force
(Get-Item "$root/src/Store.Checkout/CheckoutCalculator.cs").LastWriteTime = Get-Date
Write-Host 'Applied known-good checkout fix.'
