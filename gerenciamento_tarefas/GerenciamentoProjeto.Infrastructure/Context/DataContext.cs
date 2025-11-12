using Microsoft.EntityFrameworkCore;
using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }

            #region ForeignKey

            //HISTORICO
            modelBuilder.Entity<Historico>().HasOne(x => x.Tarefa).WithMany(x => x.Historico).HasForeignKey(x => x.TarefaId);
            modelBuilder.Entity<Historico>().HasOne(x => x.Usuario).WithMany(x => x.Historico).HasForeignKey(x => x.UsuarioId);

            //PROJETO
            modelBuilder.Entity<Projeto>().HasOne(x => x.Usuario).WithMany(x => x.Projeto).HasForeignKey(x => x.UsuarioId);

            //TAREFA
            modelBuilder.Entity<Tarefa>().HasOne(x => x.Projeto).WithMany(x => x.Tarefa).HasForeignKey(x => x.ProjetoId);
            modelBuilder.Entity<Tarefa>().HasOne(x => x.Prioridade).WithMany(x => x.Tarefa).HasForeignKey(x => x.PrioridadeId);
            modelBuilder.Entity<Tarefa>().HasOne(x => x.Status).WithMany(x => x.Tarefa).HasForeignKey(x => x.StatusId);

            //USUARIO
            modelBuilder.Entity<Usuario>().HasOne(x => x.Cargo).WithMany(x => x.Usuario).HasForeignKey(x => x.CargoId);
            #endregion
        }

        #region "Tabelas"

        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Historico> Historico { get; set; }
        public DbSet<Prioridade> Prioridade { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        #endregion
    }
}