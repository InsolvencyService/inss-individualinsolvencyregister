using System.ComponentModel.DataAnnotations;

namespace INSS.EIIR.Web.Models;

public class User
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}