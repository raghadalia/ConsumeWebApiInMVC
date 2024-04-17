using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.Authentication_Models;
using ToDo.Data.Migrations;
using ToDo.Models;
namespace ToDo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(
       DbContextOptionsBuilder
       optionsBuilder
        )
        {

            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TodoTest; Trusted_Connection = True; MultipleActiveResultSets = true")
                .LogTo(Console.WriteLine,
                      new[] { DbLoggerCategory.Database.Command.Name },
                      LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        public DbSet<ToDos> ToDos { get; set; }
    }

}
