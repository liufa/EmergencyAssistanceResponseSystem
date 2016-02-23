using System;
using System.Collections.Generic;

namespace Services.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string GcmUserId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public Nullable<System.DateTime> LastSeenOn { get; set; }
        public bool IsActive { get; set; }
    }
}
