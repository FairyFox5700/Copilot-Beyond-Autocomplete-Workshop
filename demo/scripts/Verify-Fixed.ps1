$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
Push-Location $root
try {
    & dotnet test demo/tests/Store.Checkout.Tests/Store.Checkout.Tests.csproj
    if ($LASTEXITCODE -ne 0) { throw 'Focused tests failed.' }

    & dotnet test CopilotWorkshop.slnx
    if ($LASTEXITCODE -ne 0) { throw 'Full solution tests failed.' }

    & dotnet build CopilotWorkshop.slnx --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'Solution build failed.' }

    $demo = & dotnet run --project demo/src/Store.Checkout.Demo/Store.Checkout.Demo.csproj --no-build | Out-String
    if (-not $demo.Contains('Discount:   -US$10.00') -or -not $demo.Contains('Total:       US$100.00')) {
        throw "Visible checkout result is wrong.`n$demo"
    }

    Write-Host 'FIX VERIFIED: focused tests, full tests, build, and visible result are green.' -ForegroundColor Green
}
finally {
    Pop-Location
}
