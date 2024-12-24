using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.CustomValidators
{
    /// <summary>
    /// Validates a string such that it can only characters up to code point 126 (127 is DEL)
    /// Widens scope of possible input to those generally visible on a standard UK keyboard while avoiding
    /// characters with potential encoding issues as result of EIIR database using a single byte collation
    /// </summary>
    public class ContainsOnly7bitCharactersAttribute : ValidationAttribute
    {

        public ContainsOnly7bitCharactersAttribute()
        {
            const string defaultErrorMessage = "Can only contain characters you can actually see on your keyboard... more or less \uD83D\uDE09";  //utf16 code for wink emoji ;)
            ErrorMessage ??= defaultErrorMessage;

        }

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var byteArray = Encoding.UTF8.GetBytes(value.ToString());

            foreach (var b in byteArray)
            {
                if (b >= 0x7F)
                    return new ValidationResult(ErrorMessage);

            }

            return ValidationResult.Success;
        }
    }


}
