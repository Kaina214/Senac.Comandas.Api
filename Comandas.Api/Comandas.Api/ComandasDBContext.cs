using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api;

public class ComandasDbContext : DbContext
{
    public ComandasDbContext(DbContextOptions<ComandasDbContext> options)
        : base(options)
    {
    }
    // definir algumas configuracoes adicionais no banco
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Usuario>()
            .HasData(
                new Models.Usuario
                {
                    Id = 1,
                    Nome = "Admin",
                    Email = "admin@admin.com",
                    Senha = "admin123"
                }
             );
        modelBuilder.Entity<Models.CardapioItem>()
            .HasData(
                new Models.CardapioItem
                {
                    Id = 1,
                    Titulo = "XIS DE FRANGO",
                    Descricao = "XIS DE FRANGO",
                    PossuiPreparo = true,
                    Preco = 23
                },
                new Models.CardapioItem
                {
                    Id = 2,
                    Titulo = "COCA COLA LATA 500ML",
                    Descricao = "COCA COLA LATA 500ML",
                    PossuiPreparo = false,
                    Preco = 4
                },
                new Models.CardapioItem
                {
                    Id = 3,
                    Titulo = "TORRADA COM OVO",
                    Descricao = "TORRADA COM OVO",
                    PossuiPreparo = true,
                    Preco = 6
                }
        );
        modelBuilder.Entity<Models.Mesa>()
            .HasData(
               new Models.Mesa { Id = 1, NumeroMesa = 1, SituacaoMesa = (int)SituacaoMesa.Livre },
               new Models.Mesa { Id = 2, NumeroMesa = 2, SituacaoMesa = (int)SituacaoMesa.Livre },
               new Models.Mesa { Id = 3, NumeroMesa = 3, SituacaoMesa = (int)SituacaoMesa.Livre }
            );
        // inserir as categorias no banco na primeira vez
        modelBuilder.Entity<Models.CategoriaCardapio>()
            .HasData(
               new Models.CategoriaCardapio { Id = 1, Nome = "Lanches" },
               new Models.CategoriaCardapio { Id = 2, Nome = "Bebidas" },
               new Models.CategoriaCardapio { Id = 3, Nome = "Acompanhamentos" }
            );
        base.OnModelCreating(modelBuilder);

    }
    public DbSet<Models.Usuario> Usuarios { get; set; } = default!;
    public DbSet<Models.Mesa> Mesas { get; set; } = default!;
    public DbSet<Models.Reserva> Reservas { get; set; } = default!;
    public DbSet<Models.Comanda> Comandas { get; set; } = default!;
    public DbSet<Models.ComandaItem> ComandaItens { get; set; } = default!;
    public DbSet<Models.PedidoCozinha> PedidoCozinhas { get; set; } = default!;
    public DbSet<Models.PedidoCozinhaItem> PedidoCozinhaItens { get; set; } = default!;
    public DbSet<Models.CardapioItem> CardapioItens { get; set; } = default!;
    public DbSet<Models.CategoriaCardapio> CategoriaCardapios { get; set; } = default!;
    public object CategoriaCardapio { get; internal set; }
}