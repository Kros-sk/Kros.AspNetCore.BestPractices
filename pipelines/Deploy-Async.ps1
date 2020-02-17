param (
    [Parameter(Mandatory = $true)][String[]]$microservices
)

$jobs = @()
ForEach ($service in $microservices) {
    $jobs += Start-Job -ArgumentList $service -ScriptBlock {
        param($name)
            az webapp deployment source config-zip --resource-group kros-dev-meetups-rsg --name demo-dev-$name-api --src "$(Pipeline.Workspace)/ToDosDemoServices/drop/Kros.$name.Api.zip"
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