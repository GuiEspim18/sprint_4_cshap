using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestYouApi.entities
{
    /// <summary>
    /// Representa uma requisição de chat para a API Hugging Face / OpenAI.
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// Identifica o modelo a ser usado para gerar respostas.
        /// Exemplo: "openai/gpt-oss-20b:groq"
        /// </summary>
        public string model { get; set; } = "";

        /// <summary>
        /// Lista de mensagens que compõem a conversa.
        /// Cada mensagem deve ter um papel (system, user, assistant) e o conteúdo de texto.
        /// </summary>
        public List<Message> messages { get; set; } = new();
    }
}
