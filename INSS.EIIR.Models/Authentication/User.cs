using System.ComponentModel.DataAnnotations;

namespace INSS.EIIR.Models.Authentication;

public class User
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string UserRole { get; set; }
}