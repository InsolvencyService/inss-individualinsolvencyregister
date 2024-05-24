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
    [string]$subscriptionId
)

$Logfile = $logFilePath+"\logs.log"

# Function to write to log file
Function WriteLogs
{
   Param ([string]$logstring)
   $Stamp = (Get-Date).toString("yyyy/MM/dd HH:mm:ss")
   $Line = "$Stamp $Level $logstring"
   If($Logfile) {
        Add-Content $logfile -Value $Line
    }
   Else {
        Write-Output $Line
   }   
}


try
{
    # Login to Azure using Managed Identity
    Connect-AzAccount -Identity
    
    #Sets the tenant
    Set-AzContext -Tenant $TenantId
    
    #Sets the tenant and subscription
    #Set-AzContext -Tenant $TenantId -SubscriptionId $subscriptionId
  
    # Get the current date 
    $today = Get-Date -Hour 0 -minute 0

    # Get the storage account context
    $storageAccount = Get-AzStorageAccount -ResourceGroupName $azureResouceGroupName -Name $azureStorageAccountName
    $storageContext = $storageAccount.Context

    #$files = Get-ChildItem "$localFilePath\*.sql"  
     $files = Get-ChildItem -Path "$localFilePath\*.sql" | Where-Object {
        ($_.CreationTime -ge $today) -or ($_.LastWriteTime -ge $today)
    }

   


    WriteLogs("")    
    WriteLogs("===========================Start processing files=======================")

     
    # check if any new files to be processed
    if ($files -eq $null){
        WriteLogs("No new files to process" )	

    }
 
    foreach ($azureBlob in $files)
    {   
        WriteLogs("File : $azureBlob was created/updated on : " + $azureBlob.LastWriteTime)	
        
        $blob = Get-AzStorageBlob -Context $storageContext -Container $azureContainerName  -Blob $azureBlob.Name -ErrorAction Ignore 
		
        # Check if the file already exists in the container
        if (-not $blob)        
		{               
            WriteLogs ("Procssing file :  $azureBlob")   

            # Upload each file to the Azure Blob blob container
            Set-AzStorageBlobContent -Container $azureContainerName -File $azureBlob.FullName   -Context $storageContext -StandardBlobTier 'Cold' -Force | Out-Null    
                     
            WriteLogs "File : $azureBlob - successfully uploaded to blob container."
                
            Move-Item $azureBlob.FullName $archivefilePath -ErrorAction SilentlyContinue -Force
            WriteLogs "File : $azureBlob - moved to archive."
        }
		else
		{			
            WriteLogs ("File : $azureBlob already exists ")		
		}
    }
    WriteLogs("===========================Finished processing files=======================")
}
catch
{       
    WriteLogs "`r`n Error in uploading files `r`n "
    WriteLogs ("---------------------------------------------")
    WriteLogs $_
}

#Write-Output "`r`n"
#Write-Output ("---------------------------------------------")
#Write-Output ("--------------List of Blobs -----------------")
# Get-AzStorageBlob -Container $azureContainerName  -Context $storageContext | Select-Object -Property Name,Length,AccessTier 

#Usage:  .\EIIRDailyUpdateCopyFilesToBlobStorage.ps1  'tenantid' 'mk-test-rg' 'mkteststacct' 'mkblobs1' "C:\Autojobs-v1\EIIR"  "C:\Autojobs-v1\EIIR\Archive" "C:\Autojobs-v1\Logs"