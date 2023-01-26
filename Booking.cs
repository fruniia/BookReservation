using BookReservation.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation
{
    public class Booking
    {
        public StartScene MyStartScene;
        public AdminScene MyAdminScene;
        public StatisticScene MyStatisticScene;

        public Booking()
        {
            MyStartScene = new StartScene(this);
            MyAdminScene = new AdminScene(this);
            MyStatisticScene = new StatisticScene(this);
        }
        public void Start()
        {
            MyStartScene.Run();
        }
    }
}
