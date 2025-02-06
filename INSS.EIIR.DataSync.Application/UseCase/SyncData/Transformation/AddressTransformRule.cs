using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation
{
    public class AddressTransformRule : ITransformRule
    {

        public async Task<TransformRuleResponse> Transform(InsolventIndividualRegisterModel model)
        {

            var transformedAddress = string.Join(", ", (model.individualAddress ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
            model.individualAddress = transformedAddress;

            return new TransformRuleResponse()
            {
                ErrorMessage = "",
                IsError = false,
                Model = model
            };

        }

    }
}
