using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.SyncData;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataSourceBuilder
    {
        private InsolventIndividualRegisterModel _fakeData = new InsolventIndividualRegisterModel();
        private Exception _exception = null;

        public static MockDataSourceBuilder Create() { return new MockDataSourceBuilder(); }

        public MockDataSourceBuilder ThatHas(InsolventIndividualRegisterModel model)
        {
            _fakeData = model;

            return this;
        }

        public MockDataSourceBuilder ThrowsException(Exception ex)
        {
            _exception = ex;

            return this;
        }


        public IDataSourceAsync<InsolventIndividualRegisterModel> Build()
        {
            var mock = Substitute.For<IDataSourceAsync<InsolventIndividualRegisterModel>>();
            
            if (_exception != null)
                mock.GetInsolventIndividualRegistrationsAsync().Throws(_exception);
            else
                mock.GetInsolventIndividualRegistrationsAsync().Returns(GetTestValues());

            mock.Type.Returns(SyncDataEnums.Datasource.FakeBKTandIVA);
            mock.Description.Returns("Mocked Date Source");

            return mock;
        }

        private async IAsyncEnumerable<InsolventIndividualRegisterModel> GetTestValues()
        {

            yield return _fakeData;

            await Task.CompletedTask; // to make the compiler warning go away
        }
    }
}
