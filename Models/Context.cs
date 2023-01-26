using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BookReservation.Models
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = tcp:nykopingproganna.database.windows.net, 1433; Initial Catalog = bookingdbanna; Persist Security Info = False; User ID = annaadmin; Password = Prog2!2022; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }
        public DbSet<Reservation>? Reservations { get; set; }
        public DbSet<Person>? Persons { get; set; }
        public DbSet<Conferenceroom>? Conferencerooms { get; set; }

    }
}
