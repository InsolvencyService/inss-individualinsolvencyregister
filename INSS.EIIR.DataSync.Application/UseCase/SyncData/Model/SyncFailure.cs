using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class SyncFailure
    {
        public InsolventIndividualRegisterModel Model { get; set; }
        public bool IsError { get; set; }
        public List<string> ErrorMessages { get; set; }

        public static implicit operator SyncFailure(TransformResponse response)
        {
            return new SyncFailure()
            {
                Model = response.Model,
                ErrorMessages = response.ErrorMessages,
                IsError = response.IsError,
            };
        }

        public static implicit operator SyncFailure(ValidationResponse response)
        {
            return new SyncFailure()
            {
                Model = response.Model,
                ErrorMessages = response.ErrorMessages,
                IsError = response.IsValid,
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is SyncFailure failure)
            {
                return this.Model.Equals(failure.Model) &&
                    this.IsError.Equals(failure.IsError) &&
                    this.ErrorMessages.SequenceEqual(failure.ErrorMessages);
            }
            else return false;
        }
    }
}
