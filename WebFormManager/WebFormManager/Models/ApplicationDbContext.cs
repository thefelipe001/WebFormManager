

using Microsoft.EntityFrameworkCore;

namespace WebFormManager.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FormularioPeliculas> FormularioPeliculas { get; set; }

    }
}
