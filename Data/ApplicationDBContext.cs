using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    // This class will be used to interact with the database, allowing us
    // to perform CRUD operations on the database. We will inject this class
    // into the controller classes that use the DB (dependency injection).
    public class ApplicationDBContext : DbContext
    {
        // The `base` is for passing the options to the base class (DbContext)
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        // DbSet allows us to manipulate the database table (create tables, etc).
        public DbSet<Models.Stock> Stocks { get; set; } = null!;
        public DbSet<Models.Comment> Comments { get; set; } = null!;
    }
}