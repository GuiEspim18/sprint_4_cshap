using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa uma escolha retornada pelo modelo de IA em uma resposta de chat.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Mensagem gerada pelo modelo correspondente a esta escolha.
        /// Contém o papel (role) e o conteúdo da resposta.
        /// </summary>
        public Message message { get; set; } = new();
    }
}
