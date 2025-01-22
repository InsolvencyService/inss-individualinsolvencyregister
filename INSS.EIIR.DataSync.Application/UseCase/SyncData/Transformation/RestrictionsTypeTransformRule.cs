using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation
{
    public class RestrictionsTypeTransformRule : ITransformRule
    {
        public async Task<TransformRuleResponse> Transform(InsolventIndividualRegisterModel model)
        {
            if (model.restrictionsType == null || model.restrictionsType=="")
                model.restrictionsType = null;

            return new TransformRuleResponse()
            {
                ErrorMessage = "",
                IsError = false,
                Model = model
            };

        }

    }
}
