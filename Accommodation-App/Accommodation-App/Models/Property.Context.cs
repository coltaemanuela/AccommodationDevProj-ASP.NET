﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accommodation_App.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StudAccommodationEntities1Entities : DbContext
    {
        public StudAccommodationEntities1Entities()
            : base("name=StudAccommodationEntities1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Property> Properties { get; set; }

        public System.Data.Entity.DbSet<Accommodation_App.Models.User> Users { get; set; }
    }
}
