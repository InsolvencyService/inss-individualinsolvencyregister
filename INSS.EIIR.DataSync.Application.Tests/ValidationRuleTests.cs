using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.Tests
{
    public class ValidationRuleTests
    {
        [Fact]
        public async Task Given_valid_id_IdValidationRule_passes()
        {
            // arrange
            var sut = new IdValidationRule();

            // act
            var response = await sut.Validate(ValidData.Standard());

            // assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async Task Given_invalid_id_IdValidationRule_fails()
        {
            // arrange
            var sut = new IdValidationRule();

            // act
            var response = await sut.Validate(InvalidData.NoId());

            // assert
            Assert.False(response.IsValid);
        }
    }
}
