using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;
using System.Globalization;
using System.Xml.Linq;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class BKTStatusValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.RecordType == Models.CaseModels.IIRRecordType.BKT && model.IncludeCaseDetailsInXML(DateTime.Now)) {

                if ((model.caseStatus ?? "").StartsWith("Discharged On ", StringComparison.OrdinalIgnoreCase) ||
                    (model.caseStatus ?? "").StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase) ||
                    (model.caseStatus ?? "").StartsWith("Currently Bankrupt : Automatic Discharge", StringComparison.OrdinalIgnoreCase)) {

                    DateOnly aDate;
                    if (!DateOnly.TryParse(model.caseStatus[^10..^0], CultureInfo.GetCultureInfo("en-GB"),out aDate))
                    {
                        isValid = false;
                        errorMessage = $"case {model.caseNo} caseStatus must end in a date formatted \"dd/MM/yyyy\" for the record state";
                    }
                }
            }

            return new ValidationRuleResponse()
            {
                IsValid = isValid,
                ErrorMessage = errorMessage
            };
        }

        private bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
