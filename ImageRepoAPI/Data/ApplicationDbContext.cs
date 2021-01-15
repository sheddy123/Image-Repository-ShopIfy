using ImageRepoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<ImageUploads> ImageUploads { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
