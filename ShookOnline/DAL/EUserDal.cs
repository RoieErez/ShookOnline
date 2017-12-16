using ShookOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShookOnline.DAL
{
    public class EUserDal:DbContext
    {
        public DbSet<EUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EUser>().ToTable("ExternalUser");
        }
    }
}