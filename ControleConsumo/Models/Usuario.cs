using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleConsumo.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome de usuário")]
    [Display(Name = "Usuário")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o perfil")]
    public Perfil Perfil { get; set; }
}

public enum Perfil
{
    Admin,
    User
}
