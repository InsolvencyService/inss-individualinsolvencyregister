# Define parameters
param (
    [Parameter(Mandatory=$true)]
    [string]$TenantId, 
    
    [Parameter(Mandatory=$true)]
    [string] $azureResouceGroupName,

    [Parameter(Mandatory=$true)]
    [string] $azureStorageAccountName,
    
    [Parameter(Mandatory=$true)]
    [string] $azureContainerName,
    
    [Parameter(Mandatory=$true)]
    [string]$localFilePath,

    [Parameter(Mandatory=$false)]
    [string] $archivefilePath,
    
    [Parameter(Mandatory=$false)]
    [string]$logFilePath,

    [Parameter(Mandatory=$false)]
    [boolean]$archiveFiles,

    [Parameter(Mandatory=$false)]
    [int]$waitTime = 30
)

$Logfile = $logFilePath+"\DailyExtractLog.log"

# Function to write to log file
Function Log-Message
{
   Param ([string]$message)
   $timestamp = (Get-Date).toString("dd/MM/yyyy HH:mm:ss ffff")
   
   $logmessage = "$timestamp $Level $message"
   If($Logfile) {
        Add-Content $logfile -Value $logmessage
    }
   Else {
        Write-Output $logmessage
   }   
}

Log-Message("")
Log-Message ("Script started.")

try {
    # Connect to Azure with Managed Identity    
    Log-Message("Connecting to Azure with Managed Identity...")
    Connect-AzAccount -Identity
    #Sets the tenant context
    Log-Message("Setting tenant context...")
    Set-AzContext -Tenant $TenantId
    # Get the current date 
    $today = Get-Date -Hour 0 -minute 0

    # Get the storage account context
    Log-Message("Getting storage account context...")
    #$storageAccount = Get-AzStorageAccount -ResourceGroupName $azureResouceGroupName -Name $azureStorageAccountName
    $storageAccount = New-AzStorageContext -StorageAccountName $azureStorageAccountName -UseConnectedAccount #Get-AzStorageAccount -ResourceGroupName $azureResouceGroupName -Name $azureStorageAccountName
    $storageContext = $storageAccount.Context

    # List files in the local folder modifed today
    Log-Message("Listing and sorting the files in local folder...") 
    #$files = Get-ChildItem -Path "$localFilePath\*.sql"  ## Uncomment for prod
    $files = Get-ChildItem -Path "$localFilePath\*.sql" | Where-Object {($_.LastWriteTime -ge $today) } | Sort-Object LastWriteTime ## Remove for prod
    #$fileCount = (Get-ChildItem -Path "$localFilePath\*.sql"## Uncomment for prod
    $fileCount = (Get-ChildItem -Path "$localFilePath\*.sql" | Where-Object {($_.LastWriteTime -ge $today) } | Measure-Object).Count ## Remvoe for prod
    Log-Message("===========================Start uploading files=======================")

     
    # check if any new files to be processed
    if ($files -eq $null){
        Log-Message("No files to upload" )	

    }

    Log-Message("Uploading to Storage account: $azureStorageAccountName - Container: $azureContainerName ...") 

    $uploadedFilepCount = 0

    foreach ($file in $files) {
        $blobName = $file.Name
        $filePath = $file.FullName

        # Retry mechanism for checking the previous file existence
        $retryCount = 1
        $maxRetries = 5
        $fileUploaded = $false
        
        $blob = Get-AzStorageBlob -Context $storageContext -Container $azureContainerName  -ErrorAction Ignore  
        
        if ($blob.Count -eq 0)
        {
            # Upload the file to the blob storage if it does not exist
            Log-Message("Uploading the file ($blobName) to the blob storage") 
            # Upload each file to the Azure Blob blob container
            Set-AzStorageBlobContent -Container $azureContainerName -File $filePath -Context $storageContext -StandardBlobTier 'Cool' -Force | Out-Null    
            Log-Message("File : $blobName successfully uploaded to blob storage.")
            
            $uploadedFilepCount ++ 
            
            if($archiveFiles -and $uploadedFilepCount -eq $fileCount)
            {                
                # Move the uploaded file to the Archive folder
                Move-Item $filePath $archivefilePath -ErrorAction SilentlyContinue -Force
                Log-Message("File : $blobName - moved to archive folder.")                
            }                           
            
        } 
        else 
        {            
            Log-Message("File already exists in blob storage...")
            
        }

        if ($uploadedFilepCount -lt $fileCount)
        {          
        
            Start-Sleep -Seconds $waitTime
            
            while ($retryCount -le $maxRetries -and -not $fileUploaded) 
            {                  
                $blob = Get-AzStorageBlob -Context $storageContext -Container $azureContainerName  -ErrorAction Ignore  
           
                $count = $retryCount++
                   
                if ($blob.Count -eq 0)            
                {
                    if($archiveFiles)
                    {
                        # Move the uploaded file to the Archive folder
                        Move-Item $filePath $archivefilePath -ErrorAction SilentlyContinue -Force 
                        Log-Message("File : $blobName - moved to archive folder...")
                    }

                    $fileUploaded = $true          

                }
                else
                {                      
                    Log-Message("File(s) already exists in blob storage. Waiting for removal... Retry # : $count ")
                    Start-Sleep -Seconds $waitTime
                }                        
                    
                
                }        
            if (-not $fileUploaded) 
            {                
                throw "Failed to upload file after $maxRetries retries. File(s) still exists in blob storage."
            }
        }
    }

    Log-Message("===========================Finished uploading files=======================")
    Log-Message("Script execution completed.")
} 
catch 
{
    Log-Message(" ")
    Log-Message("---------------------------------------------")
    Log-Message("Error in uploading files:")
    Log-Message("Error: $_")
    Log-Message("---------------------------------------------")

}

