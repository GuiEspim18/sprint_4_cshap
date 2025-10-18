using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa um endereço de usuário.
    /// Pode ser preenchido automaticamente via API do ViaCEP a partir do CEP.
    /// </summary>
    public class Endereco
    {
        /// <summary>
        /// Identificador único do endereço no banco de dados.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código de Endereçamento Postal (CEP) do endereço.
        /// </summary>
        public string Cep { get; set; } = "";

        /// <summary>
        /// Nome da rua, avenida ou logradouro.
        /// </summary>
        public string Logradouro { get; set; } = "";

        /// <summary>
        /// Nome do bairro.
        /// </summary>
        public string Bairro { get; set; } = "";

        /// <summary>
        /// Nome da cidade/localidade.
        /// </summary>
        public string Localidade { get; set; } = "";

        /// <summary>
        /// Sigla do estado (UF).
        /// </summary>
        public string Uf { get; set; } = "";
    }
}
