﻿using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class XMLSinkOptions
    {
        //Thie name of storage to be saved to
        public string StorageName { get; set; } 

        //The storage connection string
        public string StoragePath { get; set; }

        //The number of records to be written to BlobStorage at a time
        public int WriteToBlobRecordBufferSize { get; set; }
        public SyncDataEnums.Datasource PermittedDataSources { get; set; }
    }
}
