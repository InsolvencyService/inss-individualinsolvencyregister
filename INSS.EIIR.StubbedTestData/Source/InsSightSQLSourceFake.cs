using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.IndexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Fake.Source
{
    public class InsSightSQLSourceFake : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private IMapper _mapper;

        public InsSightSQLSourceFake(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            using (Stream r = new FileStream("searchdata.json", FileMode.Open))
            {
                await foreach (var record in JsonSerializer.DeserializeAsyncEnumerable<IndividualSearch>(r))
                {
                    if (record.InsolvencyType != InsolvencyType.DRO)
                        yield return _mapper.Map<CaseResult, InsolventIndividualRegisterModel>(_mapper.Map<IndividualSearch, CaseResult>(record));
                }
            }
        }
    }
}
