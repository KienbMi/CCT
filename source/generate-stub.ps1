$url = "http://zoniko.zapto.org:81/swagger/v1/swagger.json"
$output = "$PSScriptRoot\swagger.json"
$start_time = Get-Date

Invoke-WebRequest -Uri $url -OutFile $output
Write-Output "Time taken: $((Get-Date).Subtract($start_time).Seconds) second(s)"

$command = "Nswag run Stylist.nswag /runtime:NetCore30"
Invoke-Expression $command