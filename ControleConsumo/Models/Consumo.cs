using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleConsumo.Models;

[Table("Consumos")]
public class Consumo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe a descrição")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a data")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }

    [Required(ErrorMessage = "Informe o valor")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Informe a quilometragem")]
    [Display(Name = "Quilometragem")]
    public int Km { get; set; }

    [Display(Name = "Tipo de combustível")]
    public TipoCombustivel Tipo { get; set; }

    [Required(ErrorMessage = "Informe o veículo")]
    [Display(Name = "Veículo")]
    public int VeiculoId { get; set; }

    [ForeignKey("VeiculoId")]
    public Veiculo? Veiculo { get; set; }
}

public enum TipoCombustivel
{
    Gasolina,
    Etanol
}
