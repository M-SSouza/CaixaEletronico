using System.ComponentModel.DataAnnotations;

namespace CaixaEletronico.Models
{
    public class Saque
    {
        [Required(ErrorMessage = "Informe o valor do saque.")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Valor minímo é 0 e o máximo é 10000.")]
        public int? Valor { get; set; }
    }
}
