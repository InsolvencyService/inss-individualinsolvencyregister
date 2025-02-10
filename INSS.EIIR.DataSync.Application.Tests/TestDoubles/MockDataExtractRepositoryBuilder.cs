using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.ExtractModels;
using NSubstitute;



namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataExtractRepositoryBuilder
    {
        private Extract _fakeData = new Extract();

        public static MockDataExtractRepositoryBuilder Create() { return new MockDataExtractRepositoryBuilder(); }

        public MockDataExtractRepositoryBuilder ThatReturns(Extract model)
        {
            _fakeData = model;

            return this;
        }

        public IExtractRepository Build()
        {
            var mock = Substitute.For<IExtractRepository>();

            mock.GetExtractAvailable().Returns(_fakeData);

            return mock;
        }

    }
}
