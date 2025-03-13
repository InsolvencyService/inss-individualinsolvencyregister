using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;
using System.Xml.Linq;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class TradingNamesValidationRule : IValidationRule

    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.tradingNames == null)
            {
                isValid = false;
                errorMessage = $"case {model.caseNo} tradingNames may not be null";

            }
            else
            {
                if (model.tradingNames != Common.NoTradingNames)
                {
                    if (!IsValidXml(model.tradingNames))
                    {
                        isValid = false;
                        errorMessage = $"case {model.caseNo} tradingNames cannot be parsed as XML";
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
