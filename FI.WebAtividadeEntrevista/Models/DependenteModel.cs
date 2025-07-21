using FI.AtividadeEntrevista.DML;
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public string Id { get; set; }

        [Required]
        public long IdCliente { get; set; }

        [Required(ErrorMessage = "Informe o nome do beneficiario")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o CPF")]
        [RegularExpression(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", ErrorMessage = "Digite um CPF válido no formato 000.000.000-00 ou 00000000000")]
        public string Cpf { get; set; }

        public Beneficiario toBeneficiario()
        {
            var beneficiario = new Beneficiario();

            beneficiario.Id = int.TryParse(this.Id, out var id) ? id : 0;
            beneficiario.IdCliente = this.IdCliente;
            beneficiario.Nome = this.Nome;
            beneficiario.Cpf = this.Cpf;
            return beneficiario;                
        }
    }
}
