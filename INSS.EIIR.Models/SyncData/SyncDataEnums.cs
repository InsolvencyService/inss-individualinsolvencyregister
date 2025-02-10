using System.Text.Json.Serialization;

namespace INSS.EIIR.Models.SyncData
{
    public class SyncDataEnums
    {
        //JsonConverter here does have desired effect => enums appear as string inputs through OpenAPI/Swagger UI
        //Many other options attempted (this is .NET 9 syntax) - parked for now to concentrate on implementing parameters as intger values
        [Flags, JsonConverter(typeof(JsonStringEnumConverter<Mode>))]
        public enum Mode : uint
        {
            Default = 0,                    //Run SyncData as originally implemented
            Test = 1,                       //Create SearchIndex but do not swap, Create XML but not ZIP   
            IgnorePreConditionChecks = 2,   //Pre-condition checks will prevent SyncData from running, e.g. when previously run is current day
            DisableXMLExtract = 4,          //Permits just IndexRebuild to take place
            DisableIndexRebuild = 8,        //Permits just XMLExtract to take place
        }

        [Flags, JsonConverter(typeof(JsonStringEnumConverter<Datasource>))]
        public enum Datasource : uint
        {
            None = 0,                       //Run SyncData with no datasources
            FakeDRO = 1,                    //Fake DRO dataset based on searchdata.json
            FakeBKTandIVA = 2,              //Fake BKT and IVA dataset based on searchdata.json
            IscisDRO = 4,                   //DROs from ISCIS
            IscisBKTandIVA = 8,             //BKTs and IVAs from ISCIS
            InnSightBKTandIVA = 16,         //BKTs and IVAs from INNSight
            InnSightDRO = 32,               //DROs from INNSight
        }

    }
}
