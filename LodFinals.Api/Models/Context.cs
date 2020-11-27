using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LodFinals.Api.Models;

namespace LodFinals.Api.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Syllabus> Syllabus { get; set; }
    }
}
