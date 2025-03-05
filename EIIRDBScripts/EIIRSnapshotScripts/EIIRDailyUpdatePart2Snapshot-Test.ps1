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

    # SQL queries to produces snapsot table

    $query1 = "exec createeiirSnapshotTABLE"

    Invoke-Sqlcmd -ServerInstance $AzureSQLServerName -Username $username -Password $password -Database $AzureSQLDatabaseName -Query $query1 -Verbose -QueryTimeout 300

    $query2 = "EXEC extr_avail_INS" 

    Invoke-Sqlcmd -ServerInstance $AzureSQLServerName -Username $username -Password $password -Database $AzureSQLDatabaseName -Query $query2 -Verbose -QueryTimeout 300
}
catch
{
    # handling an error - prepare the failure message

    $templateId = '442d5558-fe8a-4a5e-91c1-23f6ea9e7e76'
    $recipients = $emailRecipients
    $subject = 'EIIR daily update [2]: Snapshot FAILED'
    $body = 'EIIR daily update [2]: Snapshot FAILED'

    $response = & "C:\TheInsolvencyService\Eiir\eiirextract-test\EiirHttpRequest-Test.ps1" $templateId $recipients $subject $body

    Write-Output $response.StatusCode
}