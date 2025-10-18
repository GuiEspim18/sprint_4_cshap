using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using InvestYouApi.entities;
using InvestYouApi;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Construtor do controller, recebe o contexto do banco e o HttpClientFactory.
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    /// <param name="httpClientFactory">Fábrica de clientes HTTP</param>
    public UsersController(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    // ==================== FUNÇÃO AUXILIAR ====================
    /// <summary>
    /// Gera hash SHA256 para a senha do usuário.
    /// </summary>
    /// <param name="senha">Senha em texto puro</param>
    /// <returns>Senha criptografada em Base64</returns>
    private static string GerarHashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // ==================== GET TODOS ====================
    /// <summary>
    /// Retorna todos os usuários cadastrados, incluindo seus endereços.
    /// </summary>
    /// <returns>Lista de usuários</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.Include(u => u.Endereco).ToListAsync();
        return Ok(users);
    }

    // ==================== GET POR ID ====================
    /// <summary>
    /// Retorna um usuário específico pelo ID, incluindo seu endereço.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Usuário encontrado ou NotFound</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.Include(u => u.Endereco)
                                       .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // ==================== POST ====================
    /// <summary>
    /// Cria um novo usuário.
    /// - Criptografa a senha
    /// - Busca o endereço via ViaCEP
    /// </summary>
    /// <param name="user">Objeto User contendo nome, email, senha e endereço com CEP</param>
    /// <returns>Usuário criado com status 201</returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        // Hash da senha
        user.SenhaHash = GerarHashSenha(user.SenhaHash);

        // Buscar endereço via ViaCEP
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync($"https://viacep.com.br/ws/{user.Endereco.Cep}/json/");
        var endereco = JsonConvert.DeserializeObject<Endereco>(response);
        user.Endereco = endereco!;

        // Adicionar usuário ao banco
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // ==================== PUT ====================
    /// <summary>
    /// Atualiza um usuário existente pelo ID.
    /// - Atualiza nome, email, senha e endereço
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="updatedUser">Objeto com informações atualizadas</param>
    /// <returns>NoContent ou NotFound</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.Include(u => u.Endereco)
                                       .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        user.Nome = updatedUser.Nome;
        user.Email = updatedUser.Email;

        // Atualiza senha caso fornecida
        if (!string.IsNullOrEmpty(updatedUser.SenhaHash))
        {
            user.SenhaHash = GerarHashSenha(updatedUser.SenhaHash);
        }

        // Atualiza endereço via ViaCEP
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync($"https://viacep.com.br/ws/{updatedUser.Endereco.Cep}/json/");
        var endereco = JsonConvert.DeserializeObject<Endereco>(response);
        user.Endereco = endereco!;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ==================== DELETE ====================
    /// <summary>
    /// Exclui um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>NoContent ou NotFound</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ==================== PESQUISA LINQ ====================
    /// <summary>
    /// Pesquisa usuários pelo nome usando LINQ.
    /// </summary>
    /// <param name="nome">Nome ou parte do nome a ser buscado</param>
    /// <returns>Lista de usuários que correspondem ao critério</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string nome)
    {
        var users = await _context.Users.Include(u => u.Endereco)
                                        .Where(u => u.Nome.Contains(nome))
                                        .ToListAsync();
        return Ok(users);
    }

    // ==================== ENDPOINT IA ====================
    /// <summary>
    /// Chama o modelo Hugging Face para responder como assistente financeiro.
    /// - Recebe apenas o texto do usuário
    /// - Retorna somente a resposta do modelo
    /// </summary>
    /// <param name="request">Objeto com o campo "Content" representando a mensagem do usuário</param>
    /// <returns>Resposta do assistente financeiro em JSON</returns>
    [HttpPost("ia")]
    public async Task<IActionResult> CallIA([FromBody] IARequest request)
    {
        var apiKey = Environment.GetEnvironmentVariable("HF_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
            return BadRequest(new { error = "Hugging Face API Key não configurada" });

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        // Monta JSON de ChatCompletion
        var chatRequest = new ChatRequest
        {
            model = "openai/gpt-oss-20b:groq",
            messages = new List<Message>
            {
                new Message
                {
                    role = "system",
                    content = "Você é um assistente financeiro que fornece informações sobre investimentos de forma educativa."
                },
                new Message
                {
                    role = "user",
                    content = request.Content
                }
            }
        };

        // Chamada ao Hugging Face
        var response = await client.PostAsJsonAsync(
            "https://router.huggingface.co/v1/chat/completions",
            chatRequest
        );

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

        // Lê a resposta e retorna apenas o conteúdo do assistente
        var json = await response.Content.ReadAsStringAsync();
        var iaResponse = JsonConvert.DeserializeObject<IAResponse>(json);
        var assistantMessage = iaResponse?.choices?.FirstOrDefault()?.message?.content ?? "";

        return Ok(new { response = assistantMessage });
    }
}
