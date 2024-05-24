using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.Models.Tests
{
    public class CaseResultTests
    {
        [Theory]
        [InlineData("Bankruptcy", IIRRecordType.BKT)]
        [InlineData("Individual Voluntary Arrangement", IIRRecordType.IVA)]
        [InlineData("Debt Relief Order", IIRRecordType.DRO)]
        public void Ensure_Correct_RecordType_Determined(string insolvencytype, IIRRecordType expected)
        {
            //Arrange
            var cr = new CaseModels.CaseResult();
            cr.insolvencyType = insolvencytype;

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
            //var result = cr.RecordType;
            Action action = () => { var result =  cr.RecordType; }; 

            //Assert
            Exception ex = Assert.Throws<Exception>(action);
            Assert.Equal("Undefined Insolvency Type", ex.Message);

        }
    }
}