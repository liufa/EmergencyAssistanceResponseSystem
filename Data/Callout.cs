//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Callout
    {
        public long Id { get; set; }
        public int Crew { get; set; }
        public string Route { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
    }
}
