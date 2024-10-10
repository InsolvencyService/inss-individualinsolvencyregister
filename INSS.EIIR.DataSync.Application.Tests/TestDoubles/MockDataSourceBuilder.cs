using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataSourceBuilder
    {
        private InsolventIndividualRegisterModel _fakeData = new InsolventIndividualRegisterModel();

        public static MockDataSourceBuilder Create() { return new MockDataSourceBuilder(); }

        public MockDataSourceBuilder ThatHas(InsolventIndividualRegisterModel model)
        {
            _fakeData = model;

            return this;
        }

        public IDataSourceAsync<InsolventIndividualRegisterModel> Build()
        {
            var mock = Substitute.For<IDataSourceAsync<InsolventIndividualRegisterModel>>();
            
            mock.GetInsolventIndividualRegistrationsAsync().Returns(GetTestValues());

            return mock;
        }

        private async IAsyncEnumerable<InsolventIndividualRegisterModel> GetTestValues()
        {

            yield return _fakeData;

            await Task.CompletedTask; // to make the compiler warning go away
        }
    }
}
