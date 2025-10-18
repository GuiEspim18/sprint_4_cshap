using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa a resposta retornada pelo modelo de IA.
    /// Contém uma lista de escolhas, cada uma com uma mensagem gerada pelo modelo.
    /// </summary>
    public class IAResponse
    {
        /// <summary>
        /// Lista de escolhas retornadas pelo modelo de IA.
        /// Cada item contém o texto da resposta e o papel (role) da mensagem.
        /// </summary>
        public List<Choice> choices { get; set; } = new();
    }
}
