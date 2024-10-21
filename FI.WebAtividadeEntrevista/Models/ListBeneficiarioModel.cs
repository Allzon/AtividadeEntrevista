using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo da Lista de Beneficiario
    /// </summary>
    public class ListBeneficiarioModel
    {
        public long ClienteId { get; set; }

        public List<BeneficiarioModel> Beneficiarios { get; set; }
    }
}