using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Services.Models.Mapping
{
    public class CrewMap : EntityTypeConfiguration<Crew>
    {
        public CrewMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ApplicationId)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Crew");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.LastSeenOn).HasColumnName("LastSeenOn");
        }
    }
}
