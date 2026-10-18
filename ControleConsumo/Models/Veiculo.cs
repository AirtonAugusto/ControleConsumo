using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleConsumo.Models;

[Table("Veiculos")]
public class Veiculo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a placa")]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o ano de fabricação")]
    [Display(Name = "Ano de fabricação")]
    public int AnoFabricacao { get; set; }

    [Required(ErrorMessage = "Informe o ano do modelo")]
    [Display(Name = "Ano do modelo")]
    public int AnoModelo { get; set; }
    public ICollection<Consumo> Consumos { get; set; } = new List<Consumo>();
}