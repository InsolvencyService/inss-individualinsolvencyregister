using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation
{
    public class AliasTransformRule : ITransformRule
    {
        public async Task<TransformRuleResponse> Transform(InsolventIndividualRegisterModel model)
        {
            if (model.individualAlias != null && model.individualAlias.ToLower().Replace(" ", "") == Common.NoOtherNames.ToLower().Replace(" ", ""))
                model.individualAlias = Common.NoOtherNames;    

            return new TransformRuleResponse()
            {
                ErrorMessage = "",
                IsError = false,
                Model = model
            };

        }
    }

}
