using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Sink.AISearch.AutoMapperProfiles;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Fake.Source
{
    public class EiirFakeDataSource : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        IEnumerable<InsolventIndividualRegisterModel> _data = new List<InsolventIndividualRegisterModel>();

        public EiirFakeDataSource(IMapper mapper)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new IndividualSearchToInsolventIndividualMapper());
            });
            mapper = new Mapper(config);

            using (StreamReader r = new StreamReader("searchdata.json"))
            {
                string json = r.ReadToEnd();
                var individualSearchList = JsonSerializer.Deserialize<List<IndividualSearch>>(json);

                InsolventIndividualRegisterModel model = mapper.Map<IndividualSearch, InsolventIndividualRegisterModel>(individualSearchList.Last());

                _data = individualSearchList.Select(mapper.Map<IndividualSearch, InsolventIndividualRegisterModel>);
            }
        }

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            foreach (var model in _data)
            {
                yield return model;
            }
        }
    }
}
