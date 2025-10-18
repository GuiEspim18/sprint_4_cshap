using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvestYouApi.entities;

namespace InvestYouApi
{
    /// <summary>
    /// Representa o contexto do banco de dados da aplicação.
    /// Utiliza Entity Framework Core para mapear as entidades para tabelas do banco.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração do DbContext.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext, como string de conexão e provedor.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Conjunto de usuários no banco de dados.
        /// Mapeia a entidade <see cref="User"/> para a tabela correspondente.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Conjunto de endereços no banco de dados.
        /// Mapeia a entidade <see cref="Endereco"/> para a tabela correspondente.
        /// </summary>
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
