using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation;
using INSS.EIIR.Models.SyncData;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.CaseModels;


namespace INSS.EIIR.DataSync.Application.Tests
{
    public class TransformRuleTests
    {

        [Theory]
        [InlineData(Common.NoOtherNames, Common.NoOtherNames)]
        [InlineData("No Other Names Found", Common.NoOtherNames)]
        [InlineData("No Other Names", "No Other Names")]
        [InlineData(null, null)]
        [InlineData("<OtherNames><OtherName><Forenames>John Scott</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>",
                    "<OtherNames><OtherName><Forenames>John Scott</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>")]
        public async Task Given_Alias_AliasTransformRule_normalisesAlias (string? input, string? expected)
        {
            // arrange
            var sut = new AliasTransformRule();
            var model = new InsolventIndividualRegisterModel() { individualAlias = input };

            // act
            var response = await sut.Transform(model);

            // assert
            Assert.Equal(expected, response.Model.individualAlias);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("SomeText", "SomeText")]
        public async Task Given_RestrictionsType_RestrictionsTypeTransformRule_normalisesRestrictionsType(string? input, string? expected)
        {
            // arrange
            var sut = new RestrictionsTypeTransformRule();
            var model = new InsolventIndividualRegisterModel() { restrictionsType = input };

            // act
            var response = await sut.Transform(model);

            // assert
            Assert.Equal(expected, response.Model.restrictionsType);
        }

    }
}
