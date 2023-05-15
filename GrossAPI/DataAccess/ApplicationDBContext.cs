using GrossAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrossAPI.DataAccess
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<FeedbackOrders> FeedbackOrders { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
