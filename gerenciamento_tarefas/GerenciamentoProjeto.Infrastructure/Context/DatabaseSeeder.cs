using GerenciamentoProjeto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoProjeto.Infrastructure.Context
{
    public static class DatabaseSeeder
    {
        public static void Seed(DataContext context)
        {
            context.Database.Migrate();

            if (!context.Prioridade.Any())
            {
                context.Prioridade.Add(new Prioridade { Descricao = "Baixa", });
                context.Prioridade.Add(new Prioridade { Descricao = "Média", });
                context.Prioridade.Add(new Prioridade { Descricao = "Alta", });
                context.SaveChanges();
            }

            if (!context.Status.Any())
            {
                context.Status.Add(new Status { Descricao = "Pendente" });
                context.Status.Add(new Status { Descricao = "Em Andamento" });
                context.Status.Add(new Status { Descricao = "Concluída" });
                context.SaveChanges();
            }

            if (!context.Cargo.Any())
            {
                context.Cargo.Add(new Cargo { Descricao = "Gerente", });
                context.Cargo.Add(new Cargo { Descricao = "Colaborador", });
                context.SaveChanges();
            }

            if (!context.Usuario.Any())
            {
                List<Cargo> cargos = context.Cargo.ToList();

                context.Usuario.Add(new Usuario { Nome = "Usuário Gerente", Email = "gerente@gmail.com", CargoId = cargos.FirstOrDefault(x => x.Descricao == "Gerente").Id });
                context.Usuario.Add(new Usuario { Nome = "Usuário Colaborador", Email = "colaborador@gmail.com", CargoId = cargos.FirstOrDefault(x => x.Descricao == "Colaborador").Id });

                context.SaveChanges();
            }
        }
    }
}