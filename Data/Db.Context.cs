﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EarsEntities : DbContext
    {
        public EarsEntities()
            : base("name=EarsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Callout> Callout { get; set; }
        public virtual DbSet<Crew> Crew { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    
        public virtual ObjectResult<GetNextBatchOfUsersToBeNotified_Result> GetNextBatchOfUsersToBeNotified()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetNextBatchOfUsersToBeNotified_Result>("GetNextBatchOfUsersToBeNotified");
        }
    }
}
