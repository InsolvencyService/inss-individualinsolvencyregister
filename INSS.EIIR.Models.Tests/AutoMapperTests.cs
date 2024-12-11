using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.Tests
{
    public class AutoMapperTests
    {

        [Theory]
        [InlineData("john was here", null, null)]
        [InlineData(null, null, null)]
        [InlineData("ANNULLED (debts paid in full) On 04/09/2024", "04/09/2024", "ANNULLED (debts paid in full)")]
        [InlineData("ANNULLED (debts paid in full) On 25/09/2024", "25/09/2024", "ANNULLED (debts paid in full)")]
        [InlineData("Annulled (Order Ought not to have been made) On 28/11/2024", "28/11/2024", "Annulled (Order Ought not to have been made)")]
        [InlineData("ANNULLED (IVA approved) On 23/10/2024", "23/10/2024", "ANNULLED (IVA approved)")]

        public void Mapping_of_AnnullData(string caseStatus, string expectedAnnulDate, string expectedAnnulReason)
        { 

            //Arrange
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
            });

            var mapper = new Mapper(mapperConfig);
            var caseRecord = new CaseResult() { caseStatus = caseStatus };

            //Act
            var iirModel = mapper.Map<CaseResult, InsolventIndividualRegisterModel>(mapper.Map<CaseResult, InsolventIndividualRegisterModel>(caseRecord));

            //Assert
            Assert.Equal(expectedAnnulDate, iirModel.annulDate);
            Assert.Equal(expectedAnnulReason, iirModel.annulReason);

        }

    }
}
