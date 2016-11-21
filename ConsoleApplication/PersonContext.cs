using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Mvvm;

namespace ConsoleApplication
{
    public class PersonContext : DbContext
    {
        public PersonContext() : base("DbConnection")
        {

        }
        public DbSet<Person> Persons { get; set; }
    }
}
