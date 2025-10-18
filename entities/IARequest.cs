using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa a requisição enviada para o endpoint de IA.
    /// Contém o prompt ou conteúdo que será processado pelo modelo de inteligência artificial.
    /// </summary>
    public class IARequest
    {
        /// <summary>
        /// Texto ou mensagem do usuário que será enviada para a IA.
        /// Exemplo: "Explique como criar um CRUD em C# com MySQL".
        /// </summary>
        public string Content { get; set; } = "";
    }
}
