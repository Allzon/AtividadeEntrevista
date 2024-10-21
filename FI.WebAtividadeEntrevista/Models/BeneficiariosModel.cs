using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAtividadeEntrevista.Models.Validators;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [CpfValidator(ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        /// <summary>
        /// Id do Cliente
        /// </summary>
        [Required]
        public long IdCliente { get; set; }

    }
}