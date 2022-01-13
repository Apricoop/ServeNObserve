using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Persistence.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Add here your models which you want to create and use with Db
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<JobEmployee> JobEmployee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<User>().HasQueryFilter(u => u.isActive); // user tablosuna sorgu atilirken default query ekler => where(user => user.isActive)

            modelBuilder.Entity<Job>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Job>().HasQueryFilter(u => u.isActive); // user tablosuna sorgu atilirken default query ekler => where(user => user.isActive)

            modelBuilder.Entity<Employee>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Employee>().HasQueryFilter(u => u.isActive); // user tablosuna sorgu atilirken default query ekler => where(user => user.isActive)

            modelBuilder.Entity<Service>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Service>().HasQueryFilter(u => u.isActive); // user tablosuna sorgu atilirken default query ekler => where(user => user.isActive)


            modelBuilder.Entity<JobEmployee>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<JobEmployee>().HasQueryFilter(u => u.isActive);
        }
    }
}
