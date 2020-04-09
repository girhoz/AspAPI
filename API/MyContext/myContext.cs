using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace API.MyContext
{
    public class myContext : DbContext
    {
        public myContext() : base("TestAPI") { }
        public DbSet<Department> Departments { get; set; } //Mendaftarkan tabel yang sudah dibuat
        public DbSet<Division> Divisions { get; set; }
    }
}