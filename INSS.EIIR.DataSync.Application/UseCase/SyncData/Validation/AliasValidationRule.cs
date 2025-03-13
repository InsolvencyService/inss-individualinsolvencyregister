using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;
using System.Xml.Linq;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class AliasValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.individualAlias == null)
            {
                isValid = false;
                errorMessage = $"case {model.caseNo} individualAlias may not be null";
            }
            else
            {
                if (model.individualAlias.ToLower().Replace(" ", "") != Common.NoOtherNames.ToLower().Replace(" ","")) 
                {
                    if (!IsValidXml(model.individualAlias)) 
                    {
                        isValid = false;
                        errorMessage = $"case {model.caseNo} individualAlias cannot be parsed as XML";
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
