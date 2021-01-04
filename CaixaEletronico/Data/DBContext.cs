using CaixaEletronico.Models;
using Microsoft.EntityFrameworkCore;

namespace CaixaEletronico.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Notas> Notas { get; set; }
    }
}
