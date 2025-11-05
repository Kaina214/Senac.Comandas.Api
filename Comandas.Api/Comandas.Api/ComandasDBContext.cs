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

            modelBuilder.Entity<Mesa>()
                .HasData(

                new Mesa
                {
                    Id = 1,
                    NumeroMesa = 1,
                    SituacaoMesa = 0
                },
                new Mesa
                {
                    Id = 2,
                    NumeroMesa = 2,
                    SituacaoMesa = 0
                },
                new Mesa
                {
                    Id = 3,
                    NumeroMesa = 3,
                    SituacaoMesa = 0
                },
                modelBuilder.Entity<CardapioItem>()
                .HasData(
                    new CardapioItem
                    {
                        Id = 1,
                        Titulo = "Coxinha",
                        Descricao = "Coxinha de frango com catupiry",
                        Preco = 6.50M,
                        PossuiPreparo = true
                    }
            ));

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

    }
}


