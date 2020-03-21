param (
    [Parameter(Mandatory = $true)][String[]]$microservices,
    [Parameter(Mandatory = $true)][String]$artifactPath
)

$jobs = @()
ForEach ($service in $microservices) {
    Write-Host "Start deploying microservice: " $service -ForegroundColor Green
    $jobs += Start-Job -ArgumentList $service -ScriptBlock {
        param($name, $path)
            $result = az webapp deployment source config-zip --resource-group kros-demo-rsg --name kros-demo-$name-api --src "$path/Kros.$name.Api.zip" 
            if (!$result){
                throw "Microservice ($name) failed. More information: $result"
            }
    }
}

Wait-Job -Job $jobs

$failed = $false

foreach ($job in $jobs) {
    if ($job.State -eq 'Failed') {
        Write-Host ($job.ChildJobs[0].Error) -ForegroundColor Red
        $failed = $true
    }
}

if ($failed -eq $true) {
   Write-Host 
   Write-Error "Microservices deploy failed."
}
