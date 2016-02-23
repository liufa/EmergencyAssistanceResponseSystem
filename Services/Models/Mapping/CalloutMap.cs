using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Services.Models.Mapping
{
    public class CalloutMap : EntityTypeConfiguration<Callout>
    {
        public CalloutMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Route)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Callout");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Crew).HasColumnName("Crew");
            this.Property(t => t.Route).HasColumnName("Route");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.LastSignal).HasColumnName("LastSignal");
            this.Property(t => t.IsFinished).HasColumnName("IsFinished");
            this.Property(t => t.LastBroadcast).HasColumnName("LastBroadcast");

            // Relationships
            this.HasRequired(t => t.Crew1)
                .WithMany(t => t.Callouts)
                .HasForeignKey(d => d.Crew);

        }
    }
}
