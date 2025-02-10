using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.Constants;
using System.Text.Json;
using AutoMapper;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SyncData;


namespace INSS.EIIR.StubbedTestData
{
    public class EIIRSQLSourceFake : IDataSourceAsync<InsolventIndividualRegisterModel>
    {

        private IMapper _mapper;

        public EIIRSQLSourceFake(IMapper mapper)
        {
            _mapper = mapper;
        }

        public SyncDataEnums.Datasource Type =>  SyncDataEnums.Datasource.FakeDRO;

        public string Description => "Fake DRO Data from searchdata.json";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            List<IndividualSearch> source = new List<IndividualSearch>();

            using (Stream r = new FileStream("searchdata.json", FileMode.Open))
            {
                await foreach (var record in JsonSerializer.DeserializeAsyncEnumerable<IndividualSearch>(r))
                {
                    if (record.InsolvencyType == InsolvencyType.DRO)
                        yield return _mapper.Map<CaseResult, InsolventIndividualRegisterModel>(_mapper.Map<IndividualSearch, CaseResult>(record));                
                }
            }
        }
    }
}
