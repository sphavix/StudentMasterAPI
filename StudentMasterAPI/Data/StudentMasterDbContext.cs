using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StudentMasterAPI.Models;

namespace StudentMasterAPI.Data
{
    public partial class StudentMasterDbContext : DbContext
    {

        public StudentMasterDbContext(DbContextOptions<StudentMasterDbContext> options)
            : base(options)
        {
        }


        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
