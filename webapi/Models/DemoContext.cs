using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace webapi.Models
{
    public class DemoContext : DbContext
    {
       public DemoContext(DbContextOptions<DemoContext>
          options) : base(options)
            {

            }

    public DbSet<product> products { get; set; }
    public DbSet<catogory> catogories { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<catogory>().HasData(
        //        new catogory { CID = 1, CName = "Stationary"}),
        //        new catogory { CID = 2 , CName = "Hardware"},
        //        new catogory { CID = 3 , CName = "Accounts"};
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}


