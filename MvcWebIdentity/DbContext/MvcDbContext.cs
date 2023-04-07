
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcWebIdentity.Entities;

namespace MvcWebIdentity.Context
{
    public class MvcDbContext : IdentityDbContext
    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options) :
            base(options)
        { }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno
                {
                    AlunoId = 1,
                    Nome = "Marcos josé",
                    Email = "marcosjoseDV@Hotmail.com",
                    Idade = 28,
                    Curso = "Arte"
                });
        }
    }
}
