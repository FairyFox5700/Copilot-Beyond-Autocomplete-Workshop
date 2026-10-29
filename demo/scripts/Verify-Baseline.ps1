$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
Push-Location $root
try {
    $previous = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    $output = & dotnet test demo/tests/Store.Checkout.Tests/Store.Checkout.Tests.csproj --logger "console;verbosity=detailed" 2>&1 | Out-String
    $exitCode = $LASTEXITCODE
    $ErrorActionPreference = $previous

    $required = @(
        'LoyaltyDiscount_AppliesToMerchandiseButNotShipping [FAIL]',
        'Passed Store.Checkout.Tests.CheckoutCalculatorTests.NoDiscount_ReturnsSubtotalPlusShipping',
        'Passed Store.Checkout.Tests.CheckoutCalculatorTests.Discount_RoundsAwayFromZeroToCurrencyPrecision',
        'Total tests: 3',
        'Passed: 2',
        'Failed: 1'
    )

    if ($exitCode -eq 0) { throw 'Baseline unexpectedly passed.' }
    foreach ($text in $required) {
        if (-not $output.Contains($text)) { throw "Baseline manifest mismatch. Missing: $text`n$output" }
    }
    if ($output -match 'Skipped:\s+[1-9]') { throw "Unexpected skipped tests.`n$output" }

    Write-Host 'BASELINE VERIFIED: exactly 2 passed, 1 intended failure, 0 skipped.' -ForegroundColor Yellow
}
finally {
    Pop-Location
}
