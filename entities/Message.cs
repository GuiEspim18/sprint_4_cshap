using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa uma mensagem em uma conversa com o modelo de IA.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Papel da mensagem no chat.
        /// Pode ser "system" (instruções do sistema), "user" (mensagem do usuário) ou "assistant" (resposta do modelo).
        /// </summary>
        public string role { get; set; } = "";

        /// <summary>
        /// Conteúdo textual da mensagem.
        /// </summary>
        public string content { get; set; } = "";
    }
}
