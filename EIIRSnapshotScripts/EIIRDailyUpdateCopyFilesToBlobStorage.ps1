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
    [string]$subscriptionId
)

$Logfile = $localFilePath+"\Logs\logs.log"

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

    $files = Get-ChildItem $localFilePath 

    LogWrite("")    
    LogWrite("===========================Start processing files=======================")
 
    foreach ($azureBlob in $files)
    {        
        Write-Output("File : $azureBlob was created/updated on : " + $azureBlob.LastWriteTime)		
        WriteLogs("File : $azureBlob was created/updated on : " + $azureBlob.LastWriteTime)	
        
        $blob = Get-AzStorageBlob -Context $storageContext -Container $azureContainerName  -Blob $azureBlob.Name -ErrorAction Ignore 
		
        # Check if the file already exists in the container
        if (-not $blob)        
		{        
            # Check if the files is not created before the current date
            if ($azureBlob.CreationTime -ge $today -or $azureBlob.LastWriteTime -gt $today)
            {
                Write-Output ("Procssing file :  $azureBlob")        
                WriteLogs ("Procssing file :  $azureBlob")   

                # Upload each file to the Azure Blob blob container
                Set-AzStorageBlobContent -Container $azureContainerName -File $azureBlob.FullName   -Context $storageContext -StandardBlobTier 'Cold' -Force | Out-Null    

                Write-Host "File : $azureBlob - successfully uploaded to blob container."
                WriteLogs "File : $azureBlob - successfully uploaded to blob container."
            
            }
            else
		    {
			    Write-Output ( "File : $azureBlob  created in the past ")		
                LogWrite ( "File : $azureBlob  created in the past ")		
		    }
        }
		else
		{
			Write-Output ("File : $azureBlob  already exists ")		
            LogWrite ("File : $azureBlob already exists ")		
		}
    }
    LogWrite("===========================Finished processing files=======================")
}
catch
{   
    Write-Output "`r`n Error in uploading files `r`n "
    Write-Output ("---------------------------------------------")
    Write-Output $_
    
    WriteLogs "`r`n Error in uploading files `r`n "
    WriteLogs ("---------------------------------------------")
    WriteLogs $_
}

#Write-Output "`r`n"
#Write-Output ("---------------------------------------------")
#Write-Output ("--------------List of Blobs -----------------")
# Get-AzStorageBlob -Container $azureContainerName  -Context $storageContext | Select-Object -Property Name,Length,AccessTier 

#Usage:  .\EIIRDailyExtractCopyFilesToBlobStorage.ps1  '8f9b88a7-3f3e-4be3-aae4-2006d4c42306' 'mk-test-rg' 'mkteststacct' 'mkblobs1' "C:\Autojobs-v1\*.sql"