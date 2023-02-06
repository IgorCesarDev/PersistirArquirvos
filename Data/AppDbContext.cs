using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersistirArquirvos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistirArquirvos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Server=localhost;database=userdb;user id=root; password=root;";
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}

