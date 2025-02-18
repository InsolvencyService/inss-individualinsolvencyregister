using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using System.Globalization;

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
        [InlineData("Bankruptcy", true, "Interim Order", IIRRecordType.IBRO)]
        [InlineData("Bankruptcy", false, "Interim Order", IIRRecordType.BKT)]
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

        /// <summary>
        /// Test the determination of IncludeCaseDetailsInXML
        /// </summary>
        /// Only tests valid input scenarios for determination of RecordType 
        [Theory]
        [InlineData("Bankruptcy", false, null, "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Bankruptcy", true, "Order", "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Bankruptcy", true, "Undertaking", "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Bankruptcy", true, "Interim Order", "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Bankruptcy", true, "Order", "10/08/2023", "10/11/2024 00:00:00", true)]
        [InlineData("Bankruptcy", true, "Order", "10/08/2023", "10/11/2024 07:15:00", false)]
        [InlineData("Bankruptcy", true, "Undertaking", "10/08/2023", "10/11/2024 07:15:00", false)]       
        [InlineData("Bankruptcy", true, "Interim Order", "10/08/2023", "10/11/2024 07:15:00", false)]
        [InlineData("Individual Voluntary Arrangement", false, null, "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Debt Relief Order", false, null, "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Debt Relief Order", true, "Order", "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Debt Relief Order", true, "Undertaking", "10/08/2023", "09/11/2024 07:15:00", true)]
        [InlineData("Debt Relief Order", true, "Order", "10/08/2023", "10/11/2024 07:15:00", true)]
        [InlineData("Debt Relief Order", true, "Undertaking", "10/08/2023", "10/11/2024 07:15:00", true)]      
        public void IncludeCaseDetailsInXML_currentdate(string insolvencytype, bool hasRestrictions, string restrictionsType, string insolvencyDate, string currentDate, bool expected)
        {
            //Arrange
            var cr = new CaseModels.CaseResult();
            cr.insolvencyType = insolvencytype;
            cr.hasRestrictions = hasRestrictions;
            cr.restrictionsType = restrictionsType;
            cr.insolvencyDate = insolvencyDate;

            //Act
            var result = cr.IncludeCaseDetailsInXML(DateTime.ParseExact(currentDate, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture));

            //Assert
            Assert.Equal(expected, result);

        }


    }
}