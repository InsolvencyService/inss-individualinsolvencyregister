using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
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
        List<InsolventIndividualRegisterModel> _data = new List<InsolventIndividualRegisterModel>();

        public EiirFakeDataSource()
        {
            using (StreamReader r = new StreamReader("searchdata.json"))
            {
                string json = r.ReadToEnd();
                _data = JsonSerializer.Deserialize<List<InsolventIndividualRegisterModel>>(json);
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
