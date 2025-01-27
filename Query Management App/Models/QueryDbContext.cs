using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Query_Management_App.Models
{
    public class QueryDbContext(DbContextOptions<QueryDbContext> options) : DbContext(options)
    {
        public DbSet<Query> Queries { get; set; }
    }
}
