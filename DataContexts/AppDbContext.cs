using ApiFinanceiro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Despesa> Despesas { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Despesa>()
                .HasMany(d => d.Tags)
                .WithMany(t => t.Despesas)
                .UsingEntity<Dictionary<string, object>>(
                "DespesaTag",
                    f => f.HasOne<Tag>().WithMany().HasForeignKey("tag_id"),
                    f => f.HasOne<Despesa>().WithMany().HasForeignKey("despesa_id"),
                    f => f.ToTable("despesas_tags")
                );
        }
    }
}
