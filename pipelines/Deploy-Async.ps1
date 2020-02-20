param (
    [Parameter(Mandatory = $true)][String[]]$microservices,
    [Parameter(Mandatory = $true)][String]$artifactPath
)

$jobs = @()
ForEach ($service in $microservices) {
    $jobs += Start-Job -ArgumentList $service -ScriptBlock {
        param($name)
            Write-Host "Deploying microservice: " $name
            az webapp deployment source config-zip --resource-group kros-demo-rsg --name kros-demo-$name-api --src "$artifactPath/Kros.$name.Api.zip"
            Write-Host "Deployed microservice: " $name
    }
}

Wait-Job -Job $jobs | Out-Null

$failed = $false

foreach ($job in $jobs) {
    if ($job.State -eq 'Failed') {
        $failed = true
        Write-Host ($job.ChildJobs[0].JobStateInfo.Reason.Message) -ForegroundColor Red
    } else {
        Write-Host (Receive-Job $job) -ForegroundColor Green
    }
}

if ($failed -eq $true)
{
    Write-Error "Microservices deploy failed."
}
