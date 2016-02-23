using System;
using System.Collections.Generic;

namespace Services.Models
{
    public partial class Callout
    {
        public long Id { get; set; }
        public System.Guid Crew { get; set; }
        public string Route { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public System.DateTime LastSignal { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<System.DateTime> LastBroadcast { get; set; }
        public virtual Crew Crew1 { get; set; }
    }
}
