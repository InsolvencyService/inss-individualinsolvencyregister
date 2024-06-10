using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.Models.Tests
{
    public class CaseResultTests
    {
        /// <summary>
        /// Test the determination of IIRRecordType
        /// Property DOES NOT detect unhappy paths if supplied data is not compliant with what is expected
        /// e.g. Restrictions are not relevant to IVA, if hasRestrictions or restrictionsType provided they are ignored rathe than exception thrown
        /// e.g. hasRestrictions = false && restrictionsType = "Order"
        /// e.g. restrictionsType = "john was here"
        /// </summary>
        /// <param name="insolvencytype"></param>
        /// <param name="hasRestrictions"></param>
        /// <param name="restrictionsType"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData("Bankruptcy",false, null, IIRRecordType.BKT)]
        [InlineData("Bankruptcy",true,"Order",  IIRRecordType.BRO)]
        [InlineData("Bankruptcy", true, "Undertaking", IIRRecordType.BRU)]
        [InlineData("Bankruptcy", false, "Undertaking", IIRRecordType.BKT)]
        [InlineData("Individual Voluntary Arrangement",false,null,IIRRecordType.IVA)]
        [InlineData("Individual Voluntary Arrangement",true,"Order",IIRRecordType.IVA)]
        [InlineData("Debt Relief Order",false,null, IIRRecordType.DRO)]
        [InlineData("Debt Relief Order", true,"Order", IIRRecordType.DRRO)]
        [InlineData("Debt Relief Order", false, "Undertaking", IIRRecordType.DRO)]
        [InlineData("Debt Relief Order", true, "Undertaking", IIRRecordType.DRRU)]
        public void Ensure_Correct_RecordType_Determined(string insolvencytype, bool hasRestrictions, string restrictionsType, IIRRecordType expected)
        {
            //Arrange
            var cr = new CaseModels.CaseResult();
            cr.insolvencyType = insolvencytype;
            cr.hasRestrictions = hasRestrictions;
            cr.restrictionsType = restrictionsType;

            //Act
            var result = cr.RecordType;

            //Assert
            Assert.Equal<IIRRecordType>(expected, result);

        }

        [Fact]
        public void Ensure_Unknown_RecordType_Error()
        {
            //Arrange
            var cr = new CaseModels.CaseResult();
            cr.insolvencyType = "john was here";

            //Act
            Action action = () => { var result =  cr.RecordType; }; 

            //Assert
            Exception ex = Assert.Throws<Exception>(action);
            Assert.Equal("Undefined Insolvency Type", ex.Message);

        }
    }
}