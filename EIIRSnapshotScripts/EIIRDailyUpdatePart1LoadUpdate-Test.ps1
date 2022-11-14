$emailRecipients = "thjef@netcompany.com,CWS.Applicationsupport@insolvency.gov.uk,andrew.butler@insolvency.gov.uk,karen.jarrett@insolvency.gov.uk,InsolvencyService.Support.Services@netcompany.com"

try
{
    # Azure SQL Manged Instance

    $AzureSQLServerName = "sqldb-uksouth-prod-pweb002.fe6a52c730f5"
    $AzureSQLDatabaseName = "iirwebdb"

    # Adds full URL to the end of the SQL Server name

    $AzureSQLServerName = $AzureSQLServerName + ".database.windows.net"

    # SQL Credentials

    $username = "websitesadmin"
    $password = ""

    # Process each SQL file

    $processedFiles = "Processed"
    $files = Get-ChildItem "C:\autojobs\eiir\iif_*.sql"

    foreach ($f in $files)
    {
        Invoke-Sqlcmd -ServerInstance $AzureSQLServerName -Username $username -Password $password -Database $AzureSQLDatabaseName -InputFile $f.FullName -Verbose -QueryTimeout 300

        $processedFiles = $processedFiles + " : " + $f.Name

        Move-Item $f.FullName C:\autojobs\eiir\archive -ErrorAction SilentlyContinue -Force
    }
}
catch
{
    # handling an error - prepare the failure message

    $templateId = '442d5558-fe8a-4a5e-91c1-23f6ea9e7e76'
    $recipients = $emailRecipients
    $subject = 'EIIR daily update [1]: Update failed'
    $body = 'EIIR daily update [1]: Update failed'

    $response = & "C:\TheInsolvencyService\Eiir\eiirextract-test\EiirHttpRequest-Test.ps1" $templateId $recipients $subject $body

    Write-Output $response.StatusCode
}