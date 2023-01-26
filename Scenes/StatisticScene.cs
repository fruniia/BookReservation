using BookReservation.Enums;
using BookReservation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookReservation.Scenes
{
    public class StatisticScene : Scene
    {
        public StatisticScene(Booking reservation) : base(reservation)
        {

        }

        public override void Run()
        {
            string prompt = "Statistics\nWhat would you like to do?\n";
            string[] options = Enum.GetNames(typeof(StatisticMenu));

            Menu statisticMenu = new Menu(prompt, options);
            int selectedIndex = statisticMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    GetNumberOfBookingsPerConferenceroom();
                    ConsoleUtils.WaitForKeyPress();
                    Run();
                    break;
                case 1:
                    int week = ConsoleUtils.GetIntFromUser("Choose the week you will see statistics for: ");
                    GetAvailabilityPerWeek(week);
                    ConsoleUtils.WaitForKeyPress();
                    Run();
                    break;
                case 2:
                    GetMostFrequentGuest();
                    ConsoleUtils.WaitForKeyPress();
                    Run();
                    break;
                case 3:
                    MyReservation.MyAdminScene.Run();
                    break;
                case 4:
                    MyReservation.MyStartScene.Run();
                    break;
                case 5:
                    ConsoleUtils.QuitConsole();
                    break;
            }
        }
        private void GetNumberOfBookingsPerConferenceroom()
        {
            using (var context = new Context())
            {
                var bookedRoom = from reservation in context.Reservations
                                 join room in context.Conferencerooms on reservation.ConferenceroomId equals room.Id
                                 select new 
                                 {
                                     Name = room.Name,
                                     Date = reservation.Date,
                                 };

                var numberOfBookings = bookedRoom.Where(s => s.Date != null).Count();
                var reservedRoom = from booked in bookedRoom.ToList()
                                   where booked.Date != null
                                   group booked by booked.Name;

                Console.WriteLine();
                foreach (var r in reservedRoom)
                {
                    Console.WriteLine($"Room {r.Key} has {r.Count()} {(r.Count() == 1 ? "booking" : "bookings")}.");
                }
            }
        }
        private void GetAvailabilityPerWeek(int week)
        {
            using (var context = new Context())
            {
                var availableRooms = context.Reservations.Include(x => x.Conferenceroom)
                  .Where(x => x.Date.StartsWith($"{week}")).ToList().OrderBy(x => x.Date).GroupBy(x => x.Conferenceroom);
                Console.WriteLine();
                if (availableRooms.Any() == true)
                {
                    Console.WriteLine($"Booked rooms week {week}");
                    foreach (var item in availableRooms)
                    {
                        string date = "";
                        foreach (var value in item)
                        {
                            string[] weekday = Enum.GetNames(typeof(Weekday));
                            for (int i = 0; i < weekday.Length; i++)
                            {
                                if (value.Date.EndsWith($"{i + 1}"))
                                {
                                    date += weekday[i] + " ";
                                }
                            }
                        }
                        Console.WriteLine($"Room [{item.Key.Name}] is booked on {date} ");
                    }
                }
            }
        }
        private void GetMostFrequentGuest()
        {
            using (var context = new Context())
            {
                var mostFrequentGuest = from reservation in context.Reservations
                                        join person in context.Persons on reservation.PersonId equals person.Id
                                        select new
                                        {
                                            Name = person.Name,
                                            Company = person.Company,
                                            Id = reservation.Id,
                                        };
                var numberOfBookings = mostFrequentGuest.Where(guest => guest.Name != null).Count();
                var reservedBy = (from bookedPerson in mostFrequentGuest.ToList()
                                  where bookedPerson.Name != null
                                  orderby bookedPerson.Name.Count() descending
                                  group bookedPerson by bookedPerson.Name).Take(1);

                Console.WriteLine();
                foreach (var name in reservedBy)
                {
                    Console.WriteLine($"Guest {name.Key} has booked {name.Count()} {(name.Count() == 1 ? "room" : "rooms")}.");
                }
            }
        }
    }
}
