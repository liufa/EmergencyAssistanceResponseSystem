using System;
using System.Collections.Generic;

namespace Services.Models
{
    public partial class Crew
    {
        public Crew()
        {
            this.Callouts = new List<Callout>();
        }

        public System.Guid Id { get; set; }
        public string ApplicationId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public Nullable<System.DateTime> LastSeenOn { get; set; }
        public virtual ICollection<Callout> Callouts { get; set; }
    }
}
