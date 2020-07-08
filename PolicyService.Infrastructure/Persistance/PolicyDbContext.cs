namespace PolicyService.Infrastructure.Persistance
{
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using MicroservicesPOC.Shared.Domain;
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

            var elementMetadata = builder.Entity<Policy>().Metadata.FindNavigation(nameof(Policy.Versions));
            elementMetadata.SetField("_versions");
            elementMetadata.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
