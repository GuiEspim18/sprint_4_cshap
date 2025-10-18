using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa um usuário do sistema.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identificador único do usuário no banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string Nome { get; set; } = "";

        /// <summary>
        /// Email do usuário.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// Hash da senha do usuário.
        /// É gerada a partir da senha fornecida e armazenada para autenticação segura.
        /// </summary>
        public string SenhaHash { get; set; } = "";

        /// <summary>
        /// Endereço associado ao usuário.
        /// Pode ser preenchido automaticamente via API do ViaCEP a partir do CEP.
        /// </summary>
        public Endereco Endereco { get; set; }
    }
}
