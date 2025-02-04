using Microsoft.EntityFrameworkCore;
using StageEs.Data;

public class AziendaDbContext : DbContext
{
    public AziendaDbContext(DbContextOptions<AziendaDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<TestataDocumento> TestataDocumenti { get; set; }
    public DbSet<RigaDocumento> RigaDocumenti { get; set; }
}
