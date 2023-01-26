using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int ConferenceroomId { get; set; }
        public virtual Conferenceroom? Conferenceroom { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }  
    }
}
