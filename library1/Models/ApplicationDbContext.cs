using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace library1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Author> authors { get; set; }
        public DbSet<BookGroup> bookGroups { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<BorrowBook> borrowBooks{ get; set; }
        public DbSet<PaymentTransaction> paymentTransaction{ get; set; }

    }
}
