
# trigger Rebuild Indexes Process
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$uri = 'https://func-uksouth-dev-eiir.azurewebsites.net/api/RebuildIndexes'
$key = ''

$headers = @{
'x-functions-key'= $key
}

$Params = @{
    Method = "Post"
    Uri = $uri
    Body = ''
    ContentType = 'application/json'
}

Write-Host "Http endpoint called with uri $($uri)"
$response = Invoke-WebRequest @Params -Headers $headers -UseBasicParsing

Write-Output $response