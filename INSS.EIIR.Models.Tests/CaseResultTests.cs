using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.Models.Tests
{
    public class CaseResultTests
    {
        [Theory]
        [InlineData("Bankruptcy",false, IIRRecordType.BKT)]
        [InlineData("Bankruptcy", true, IIRRecordType.BRO)]
        [InlineData("Individual Voluntary Arrangement",false, IIRRecordType.IVA)]
        [InlineData("Individual Voluntary Arrangement", true, IIRRecordType.IVA)]
        [InlineData("Debt Relief Order",false, IIRRecordType.DRO)]
        [InlineData("Debt Relief Order", true, IIRRecordType.DRO)]
        public void Ensure_Correct_RecordType_Determined(string insolvencytype, bool isBro, IIRRecordType expected)
        {
            //Arrange
            var cr = new CaseModels.CaseResult();
            cr.insolvencyType = insolvencytype;
            cr.broIsBro = isBro;

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