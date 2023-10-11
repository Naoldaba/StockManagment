using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions):base(dbContextOptions){}


        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comments> Comments {get; set;}
        public DbSet<Portfolio> Portfolios {get; set;}
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new {p.appUserId, p.stockId}));

            builder.Entity<Portfolio>()
                .HasOne(u=>u.appUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.appUserId);
   
            builder.Entity<Portfolio>()
                .HasOne(u=>u.stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.stockId);
            
            List<IdentityRole> roles= new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="ADMIN"
                },

                new IdentityRole
                {
                    Name="User",
                    NormalizedName="USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }

}