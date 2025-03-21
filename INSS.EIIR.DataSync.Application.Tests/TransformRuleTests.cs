﻿using INSS.EIIR.DataSync.Application.Tests.FakeData;
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
        [InlineData(null, "")]
        [InlineData(", , , ,  ", "")]
        [InlineData("65 Bayham Road, , , Morden, SM4 5JH", "65 Bayham Road, Morden, SM4 5JH")]
        [InlineData("31a Mansfield Road, , , NOTTINGHAM, , NG1 3FB", "31a Mansfield Road, NOTTINGHAM, NG1 3FB")]
        [InlineData("22 Newthorpe Road, Norton, , Doncaster, DN6 9ED", "22 Newthorpe Road, Norton, Doncaster, DN6 9ED")]
        [InlineData("14 Belmont Road, Luton, Beds, , LU1 1LL", "14 Belmont Road, Luton, Beds, LU1 1LL")]
        [InlineData("20 Ashington Grove, , , COVENTRY, , CV3 4DE", "20 Ashington Grove, COVENTRY, CV3 4DE")]
        public async Task Given_individualAddress_AddressTransformRule_normalisesAddress(string? input, string? expected)
        {
            // arrange
            var sut = new AddressTransformRule();
            var model = new InsolventIndividualRegisterModel() { individualAddress = input };

            // act
            var response = await sut.Transform(model);

            // assert
            Assert.Equal(expected, response.Model.individualAddress);
        }

    }
}
