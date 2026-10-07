using Microsoft.EntityFrameworkCore;
namespace Computer_shopp.modells
{
    public class CumputerShoppDbContext : DbContext
    {
        public CumputerShoppDbContext(DbContextOptions options) : base(options)
        {

        }

        public CumputerShoppDbContext()
        {

        }
        public DbSet<Osystem> Osystems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=localhost;Database=Computer;User=root;Password=;");
        }
    }
}
