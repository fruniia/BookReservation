using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation.Models
{
    public class Conferenceroom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? NumberOfSeats { get; set; }
        public int? Size { get; set; }
        public bool? LCDScreen { get; set; }
        public bool? MicroPhone { get; set; }
        public bool? WhiteBoard { get; set; }
        public bool? Water { get; set; }
        ICollection<Reservation>? Reservations { get; set; } 
    }
}
