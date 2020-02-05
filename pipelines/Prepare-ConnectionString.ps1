param (
    [Parameter(Mandatory = $true)][string]$microserviceName
)

$template = $env:CONNECTIONSTRING_TEMPLATE
$dbname = "kros-demo-$microserviceName-db"
$connectionString = $template -Replace '{dbname}', $dbname

Write-Host "##vso[task.setvariable variable=ConnectionStrings.DefaultConnection]$connectionString"