using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class TechnicalContext : DbContext
    {
        public TechnicalContext(DbContextOptions options) : base(options)
        {
        }
        public TechnicalContext() : base()
        { }
       
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Check> Checks { set; get; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Phones> Phones { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Sector> Sector { get; set; }
        public virtual DbSet<OrderResult> OrderResult { get; set; }
        public virtual DbSet<CheckResult> CheckResult { get; set; }
        public virtual DbSet<Lock> Lock { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

        }
    }
}
