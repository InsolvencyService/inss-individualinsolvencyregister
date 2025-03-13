using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SyncData;
using System.Text.Json;


namespace INSS.EIIR.DataSync.Infrastructure.Fake.Source
{
    public class InsSightSQLSourceFake_withValidationFaults : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private IMapper _mapper;

        public InsSightSQLSourceFake_withValidationFaults(IMapper mapper)
        {
            _mapper = mapper;
        }

        public SyncDataEnums.Datasource Type => SyncDataEnums.Datasource.FakeBKTandIVA_VF;

        public string Description => "Fake Bankruptcies and IVAs with a couple of Validation Faults from searchdata_withValidtionFaults.json";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            using (Stream r = new FileStream("searchdata_withValidationFaults.json", FileMode.Open))
            {
                await foreach (var record in JsonSerializer.DeserializeAsyncEnumerable<IndividualSearch>(r))
                {
                    if (record.InsolvencyType != InsolvencyType.DRO)
                    {
                        yield return _mapper.Map<CaseResult, InsolventIndividualRegisterModel>(_mapper.Map<IndividualSearch, CaseResult>(record.SetDynamicTestData()));
                     
                    }
                }
            }
        }

    }

}
