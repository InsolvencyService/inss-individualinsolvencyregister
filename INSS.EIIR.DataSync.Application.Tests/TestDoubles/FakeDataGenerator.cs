using Bogus;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class FakeDataGenerator : IDataSource
    {
        private readonly int RandomSeed = 1234;
        private readonly Faker<InsolventIndividualRegisterModel> _fakeIIR;

        public FakeDataGenerator()
        {
            Randomizer.Seed = new Random(RandomSeed);

            _fakeIIR = new Faker<InsolventIndividualRegisterModel>()
                .RuleFor(m => m.Id, f => Guid.NewGuid().ToString())
                .RuleFor(m => m.individualForenames, f => f.Person.FirstName)
                .RuleFor(m => m.individualSurname, f => f.Person.LastName);
        }

        public async Task<IEnumerable<InsolventIndividualRegisterModel>> GetInsolventIndividualRegistrationsAsync()
        {
            return Items();
        }

        public IEnumerable<InsolventIndividualRegisterModel> Items()
        {
            yield return _fakeIIR.Generate();
        }
    }
}
