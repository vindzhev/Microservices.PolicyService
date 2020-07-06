namespace PolicyService.Infrastructure.Persistance
{
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Microsoft.EntityFrameworkCore;
    
    using PolicyService.Domain.Entities;

    public class PolicyDbContext : DbContext
    {
        public PolicyDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Policy> Policies { get; set; }

        public Task<int> SaveChanges(CancellationToken cancellationToken = new CancellationToken()) => this.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Address>();

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
