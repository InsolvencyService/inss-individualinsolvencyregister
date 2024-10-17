using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.ExtractModels;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
