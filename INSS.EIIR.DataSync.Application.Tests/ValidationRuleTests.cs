using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;

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
            var response = await sut.Validate(InvalidData.NegativeId());

            // assert
            Assert.False(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidAliasData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidAliasData_AliasValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new AliasValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(ValidData.ValidAliasData), MemberType = typeof(ValidData))]
        public async Task Given_ValidAliasData_AliasValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new AliasValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidTradingNamesData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidTradingNamesData_TradingNamesValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new TradingNamesValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(ValidData.ValidTradingNamesData), MemberType = typeof(ValidData))]
        public async Task Given_ValidTradingNamesData_TradingNamesValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new TradingNamesValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidRestrictionsTypeData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidRestrictionsTypeData_RestrictionsTypeValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionsTypeValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(ValidData.ValidRestrictionsTypeData), MemberType = typeof(ValidData))]
        public async Task Given_ValidRestrictionsTypeData_RestrictionsTypeValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionsTypeValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidRestrictionsData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidRestrictionsData_RestrictionValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(ValidData.ValidRestrictionData), MemberType = typeof(ValidData))]
        public async Task Given_ValidRestrictionData_RestrictionValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidBKTStatusData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidBKTStatusData_BKTStatusValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new BKTStatusValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }


        [Theory]
        [MemberData(nameof(ValidData.ValidBKTStatusData), MemberType = typeof(ValidData))]
        public async Task Given_ValidBKTStatusData_BKTStatusValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new BKTStatusValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }

        [Theory]
        [MemberData(nameof(InvalidData.InvalidRestrictionEndDateData), MemberType = typeof(InvalidData))]
        public async Task Given_InvalidRestrictionEndDate_RestrictionEndDateValidationRule_fails(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionEndDateValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.False(response.IsValid);
        }


        [Theory]
        [MemberData(nameof(ValidData.ValidRestrictionEndDateData), MemberType = typeof(ValidData))]
        public async Task Given_ValidRestrictionEndDateData_RestrictionEndDateValidationRule_passes(InsolventIndividualRegisterModel model)
        {
            // arrange
            var sut = new RestrictionEndDateValidationRule();

            // act
            var response = await sut.Validate(model);

            // assert
            Assert.True(response.IsValid);
        }


    }
}
