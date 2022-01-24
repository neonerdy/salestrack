using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class SalesTrackContext : DbContext
    {
        public SalesTrackContext() : base("name=SalesTrackContext")
        {

        }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<AccountManager> AccountManagers { get; set; }
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderMonth> OrderMonths { get; set; }
        public DbSet<Pipeline> Pipelines { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        

    }
}