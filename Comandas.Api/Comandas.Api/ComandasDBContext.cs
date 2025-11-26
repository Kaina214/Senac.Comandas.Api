using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Comandas.Api
{
    public class ComandasDbContext: DbContext
    {
        public ComandasDbContext(DbContextOptions<ComandasDbContext> options)
           : base(options)
        {
        }
        //definir algumas configuracoes adicionais no banco 
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Usuario>()
                .HasData(
                    
                new Models.Usuario
                {
                    Id = 1,
                    Nome = "Admin",
                    Email = "adimin@admin.com",
                    Senha = "admin123"
                }
            );

            _ = modelBuilder.Entity<Mesa>()
                .HasData
              (

                new Models.Mesa()
                {
                    Id = 1,
                    NumeroMesa = 1,
                    SituacaoMesa = 0
                },
                new Models.Mesa()
                {
                    Id = 2,
                    NumeroMesa = 2,
                    SituacaoMesa = 0,
                },
                new Models.Mesa()
                {
                    Id = 3,
                    NumeroMesa = 3,
                    SituacaoMesa = 0
                }
              );
                modelBuilder.Entity<CardapioItem>()
                .HasData(
                    new CardapioItem
                    {
                        Id = 1,
                        Titulo = "Coxinha",
                        Descricao = "Coxinha de frango com catupiry",
                        Preco = 6.50M,
                        PossuiPreparo = true
                    },
                    new CardapioItem
                    {
                        Id = 2,
                        Titulo = "Pizza",
                        Descricao = "Uma fatia de Pizza de bacon ",
                        Preco = 12.50M,
                        PossuiPreparo = true
                    },
                    new CardapioItem
                    {
                        Id = 3,
                        Titulo = "Pastel",
                        Descricao = "Pastel de frango com catupiry",
                        Preco = 8.50M,
                        PossuiPreparo = true
                    }
                );

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
        public DbSet<Models.PedidoCozinha> PedidoCozinhas{ get; set; } = default!;
        public DbSet<Models.PedidoCozinhaItem> PedidoCozinhaItens { get; set; } = default!;
        public DbSet<Models.CardapioItem> CardapioItens { get; set; } = default!;
        public DbSet<Models.CategoriaCardapio> CategoriaCardapios { get; set; } = default!;
        public object CategoriaCardapio { get; internal set; }
    }
}


