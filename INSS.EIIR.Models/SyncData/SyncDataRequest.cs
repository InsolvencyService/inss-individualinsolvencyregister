namespace INSS.EIIR.Models.SyncData
{

    public class SyncDataRequest
    {
        public SyncDataEnums.Mode Modes { get; set; }

        public SyncDataEnums.Datasource DataSources { get; set; }
    }
}
