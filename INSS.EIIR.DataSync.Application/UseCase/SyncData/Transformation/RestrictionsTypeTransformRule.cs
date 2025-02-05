using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation
{
    public class RestrictionsTypeTransformRule : ITransformRule
    {
        public async Task<TransformRuleResponse> Transform(InsolventIndividualRegisterModel model)
        {
            if (model.restrictionsType == null || model.restrictionsType == "")
                model.restrictionsType = null;

            //Allows for "Order Made", hopefully INSSight we factor this out before going live
            if (model.restrictionsType != null && model.restrictionsType.StartsWith("Order"))
                model.restrictionsType = "Order";

            return new TransformRuleResponse()
            {
                ErrorMessage = "",
                IsError = false,
                Model = model
            };

        }

    }
}
