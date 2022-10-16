using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StudentMasterAPI.Models;

namespace StudentMasterAPI.Data
{
    public partial class StudentMasterDbContext : IdentityDbContext<User>
    {

        public StudentMasterDbContext(DbContextOptions<StudentMasterDbContext> options)
            : base(options)
        {
        }


        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
