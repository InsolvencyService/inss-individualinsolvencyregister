param ( $templateId, $recipients, $subject, $body )
Write-Host "EiirHttpRequest-Test.ps1 has been called with: [templateId $($templateId), recipients $($recipients), subject $($subject), body $($body) ]"

$uri = 'https://func-uksouth-dev-eiir.azurewebsites.net/api/notifications/send'
$key = ''

$headers = @{
'x-functions-key'= $key
}

$requestBody = @{
    'templateId' = $templateId
    'recipients' = $recipients
    'subject' = $subject
    'body' = $body
}

$JsonBody = $requestBody | ConvertTo-Json

$Params = @{
    Method = "Post"
    Uri = $uri
    Body = $JsonBody
    ContentType = 'application/json'
}

Write-Host "Http endpoint called with uri $($uri)"

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$response = Invoke-WebRequest @Params -Headers $headers -UseBasicParsing

Write-Output $response
