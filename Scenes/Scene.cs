using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation.Scenes
{
    public class Scene
    {
        protected Booking MyReservation;
        public Scene(Booking reservation)
        {
            MyReservation = reservation;
        }
        virtual public void Run()
        {

        }
    }
}
