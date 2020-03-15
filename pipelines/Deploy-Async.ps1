param (
    [Parameter(Mandatory = $true)][String[]]$microservices,
    [Parameter(Mandatory = $true)][String]$artifactPath
)

#$jobs = @()
ForEach ($service in $microservices) {
    Write-Host "Start deploying microservice: " $service -ForegroundColor Green
   # $jobs += Start-Job -ArgumentList $service -ScriptBlock {
   #     param($name)
   $name = $service
            $result = az webapp deployment source config-zip --resource-group kros-demo-rsg --name kros-demo-$name-api --src "$artifactPath/Kros.$name.Api.zip" 
            if ($result){
                Write-Host "Deployed microservice: " $name -ForegroundColor Green
            }
   # }
}

#Wait-Job -Job $jobs

#$failed = $false

#foreach ($job in $jobs) {
#    Receive-Job $job -Keep
#    if ($job.ChildJobs[0].Error -ne '') {
#        $failed = $true
#    }
#}

#if ($failed -eq $true) {
#    Write-Host 
#    Write-Error "Microservices deploy failed."
#}
