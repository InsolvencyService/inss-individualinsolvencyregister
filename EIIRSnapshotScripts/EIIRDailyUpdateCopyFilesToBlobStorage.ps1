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
    [string]$subscriptionId,

    [Parameter(Mandatory=$false)]
    [boolean]$archiveFiles
)

$Logfile = $logFilePath+"\DailyExtractLog.log"

# Function to write to log file
Function Log-Message
{
   Param ([string]$message)
   $timestamp = (Get-Date).toString("dd/MM/yyyy HH:mm:ss")
   
   $logmessage = "$timestamp $Level $message"
   If($Logfile) {
        Add-Content $logfile -Value $logmessage
    }
   Else {
        Write-Output $logmessage
   }   
}


try
{
    # Login to Azure using Managed Identity
    Log-Message("")
    Log-Message("Connecting to Azure with Managed Identity...")  
    Connect-AzAccount -Identity
    
    #Sets the tenant context
    Log-Message("Setting tenant context...")  
    Set-AzContext -Tenant $TenantId
    
    #Sets the tenant and subscription context
    #Set-AzContext -Tenant $TenantId -SubscriptionId $subscriptionId
  
    # Get the current date 
    $today = Get-Date -Hour 0 -minute 0
       

    # Get the storage account context    
    Log-Message("Getting storage account context...")  
    #$storageAccount = Get-AzStorageAccount -ResourceGroupName $azureResouceGroupName -Name $azureStorageAccountName
    $storageAccount = New-AzStorageContext -StorageAccountName $azureStorageAccountName -UseConnectedAccount #Get-AzStorageAccount -ResourceGroupName $azureResouceGroupName -Name $azureStorageAccountName
    $storageContext = $storageAccount.Context

    # List files in the local folder modifed today
    Log-Message("Listing and sorting the files in local folder...")  
     $files = Get-ChildItem -Path "$localFilePath\*.sql" | Where-Object {($_.LastWriteTime -ge $today) } | Sort-Object LastWriteTime

       
    Log-Message("===========================Start uploading files=======================")

     
    # check if any new files to be processed
    if ($files -eq $null){
        Log-Message("No files to upload" )	

    }
 
    foreach ($file in $files)
    {   
        Log-Message("File : $file was created/updated on : " + $file.LastWriteTime)	
        
        $blobName = $file.Name
        
        # Check if the blob already exists
        $blob = Get-AzStorageBlob -Context $storageContext -Container $azureContainerName  -Blob $blobName -ErrorAction Ignore 
		
        
        if (-not $blob)        
		{               
            # Upload the file to the blob storage if it does not exist
            Log-Message ("Uploading the file ($blobName) to the blob storage") 
            $filePath = $file.FullName  

            # Upload each file to the Azure Blob blob container
            Set-AzStorageBlobContent -Container $azureContainerName -File $filePath -Context $storageContext -StandardBlobTier 'Cool' -Force | Out-Null    
                     
            Log-Message ("File : $blobName successfully uploaded to blob storage.")
                
            if($archiveFiles)
            {
                # Move the uploaded file to the Archive folder
                Move-Item $blobName $archivefilePath -ErrorAction SilentlyContinue -Force
                Log-Message ("File : $blobName - moved to archive folder.")
            }
        }
		else
		{			
            Log-Message ("File : $blobName already exists in the blob storage")		
		}
    }
    Log-Message("===========================Finished uploading files=======================")
}
catch
{       
    Log-Message "`r`n Error in uploading files `r"
    Log-Message ("---------------------------------------------")
    Log-Message "Error : $_"
    Log-Message ("---------------------------------------------") 
    
}

