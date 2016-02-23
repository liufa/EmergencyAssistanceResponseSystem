using Services.Models.Mapping;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Services.Models
{
    public partial class EarsContext : DbContext
    {
        static EarsContext()
        {
            Database.SetInitializer<EarsContext>(null);
        }

        public EarsContext()
            : base("Name=EarsContext")
        {
        }

        public DbSet<Callout> Callouts { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CalloutMap());
            modelBuilder.Configurations.Add(new CrewMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
