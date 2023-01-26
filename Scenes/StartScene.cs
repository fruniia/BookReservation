using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookReservation.Enums;
using BookReservation.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BookReservation.Scenes
{
    public class StartScene : Scene
    {
        public StartScene(Booking reservation) : base(reservation)
        {

        }
        public override void Run()
        {
            string prompt = "Welcome to the Conferencereservation \nWhat would you like to do?\n";
            string[] options = Enum.GetNames(typeof(StartMenu));

            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();
            int[] userinput = new int[3];
            switch (selectedIndex)
            {
                case 0:
                    string weekPrompt = "Choose the week you want to check availability for.\n";
                    string[] weekOptions = Enum.GetNames(typeof(Week));
                    Menu weekMenu = new Menu(weekPrompt, weekOptions);
                    int week = weekMenu.Run();
                    switch (week)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            GetAvailabilityPerRoomPerWeek(week);
                            ConsoleUtils.WaitForKeyPress();
                            break;
                        case 4:
                            Run();
                            break;
                    }
                    Run();
                    break;
                case 1:
                    Show_Availability();
                    break;
                case 2:
                    HandleBooking();
                    Run();
                    break;
                case 3:
                    MyReservation.MyAdminScene.Run();
                    break;
                case 4:
                    ConsoleUtils.QuitConsole();
                    break;
            }
        }
        private void Show_Availability()
        {
            Console.WriteLine();
            ConsoleUtils.PrintList<string>(GetAllNamesOfConferenceRooms());
            //string name = AddPersonDetails(); //Gäst kan mata in Namn och Företag
            string name = GetUserName(1); //Hårdkodat -> Default är 1 => Niia
            int userid = GetUserId(name);
            int[] input = GetSelectedChoiceFromUser();
            int[] booking = CheckAvailabilityAndBookConferenceroom(userid, input);
            ConsoleUtils.WaitForKeyPress();
            Run();
        }
        private void HandleBooking()
        {
            Console.WriteLine();
            ConsoleUtils.PrintList(GetAllNamesOfConferenceRooms());
            int roomId = GetWeekChoiceFromUser();
            ConsoleUtils.PrintList(GetMoreInfoAboutConfereceRooms(roomId));
            ConsoleUtils.WaitForKeyPress();
            Run();
        }
        private int GetWeekChoiceFromUser()
        {
            int roomId = ConsoleUtils.GetIntFromUser("\nWhich conferenceroom will you see more details of?\n" +
                                                    "Please enter roomId: ");
            return roomId;
        }
        private int GetAvailabilityPerRoomPerWeek(int week)
        {
            Console.WriteLine();
            int numberOfRooms = GetAllNamesOfConferenceRooms().Count();
            string[] weekdays = Enum.GetNames(typeof(Weekday));
            int numberOfWeekdays = weekdays.Length;
            string[,] roomMatrix = new string[numberOfRooms, numberOfWeekdays];
            string[] roomArray = new string[numberOfRooms * numberOfWeekdays];

            int k = 0;
            for (int i = 0; i < numberOfWeekdays; i++)
            {
                for (int j = 0; j < numberOfRooms; j++)
                {
                    string name = GetReservations((i + 1), (j + 1), (week + 1));
                    if (name == null)
                    {
                        roomArray[k] = "";
                    }
                    else
                    {
                        roomArray[k] = name;
                    }
                    string nameOrFree = roomArray[k];
                    roomMatrix[j, i] = nameOrFree;
                    k++;
                }
            }
            Console.WriteLine("------------------------------------");
            for (int i = 0; i < numberOfWeekdays; i++)
            {
                if (i == 0)
                {
                    Console.Write($"Week: [{week + 1}]");
                    ConsoleUtils.PrintList(GetAllNamesOfConferenceRooms());
                }
                for (int j = 0; j < numberOfRooms; j++)
                {
                    if (j == 0)
                    {
                        Console.Write($"{weekdays[i].PadRight(13)}[ {roomMatrix[j, i].PadRight(10)}] ");
                    }
                    else
                    {
                        Console.Write($"[ {roomMatrix[j, i].PadRight(10)}] ");
                    }
                }
                Console.WriteLine();
            }
            return week;
        }
        private string AddPersonDetails()
        {
            string name = ConsoleUtils.GetStringFromUser("Please enter lastname: ");
            string company = ConsoleUtils.GetStringFromUser("Please enter company: ");
            using (var _context = new Context())
            {
                var person = new Person
                {
                    Name = name,
                    Company = company,
                };
                if (person != null)
                {
                    _context.Add(person);
                    _context.SaveChanges();
                }
            }
            return name;
        }
        private int[] GetSelectedChoiceFromUser()
        {
            int conferenceRoomNumber = ConsoleUtils.GetIntFromUser("Please enter the number of the conferenceroom: ");
            int week = ConsoleUtils.GetIntFromUser("Which week would you like to book? [1-4] ");
            GetAvailabilityPerRoomPerWeek((week - 1));
            Console.WriteLine();
            int day = ConsoleUtils.GetIntFromUser("Which day would you like to book? [1-5] ");
            int[] userInput = { conferenceRoomNumber, week, day };
            return userInput;
        }
        private int GetUserId(string name)
        {
            int id = 0;
            using (var _context = new Context())
            {
                id = (from p in _context.Persons where p.Name == name select p.Id).SingleOrDefault();
            };
            return id;
        }
        private string GetUserName(int userId = 1)
        {
            string name = "";
            using (var _context = new Context())
            {
                name = (from p in _context.Persons where p.Id == userId select p.Name).SingleOrDefault();
            };
            return name;
        }
        private string GetReservations(int day, int roomid, int week)
        {
            string date = week + "" + day;

            using (var _context = new Context())
            {
                var reservations = _context.Reservations.Include(x => x.Person).Include(x => x.Conferenceroom)
                    .Where(x => x.Date == date && x.ConferenceroomId == roomid).Select(x => x.Person.Name).SingleOrDefault();
                return reservations;
            }
        }
        private int[] CheckAvailabilityAndBookConferenceroom(int userId, int[] userChoice)
        {
            int roomId = userChoice[0];
            string date = userChoice[1] + "" + userChoice[2];
            int[] bookingInformation = { roomId, Convert.ToInt32(date), userId };

            //Kolla om konferensrum är ledigt valt datum
            using (var context = new Context())
            {
                var checkReservation = context.Reservations.
                    Where(x => x.ConferenceroomId == roomId && x.Date == date).ToList();
                if (checkReservation.Any() == false)
                {
                    var newReservation = new Reservation
                    {
                        Date = date,
                        PersonId = userId,
                        ConferenceroomId = roomId,
                    };
                    context.Add(newReservation);
                    context.SaveChanges();
                    return bookingInformation;
                }
                else
                {
                    RoomNotAvailable();
                    return bookingInformation;
                }
            }
        }
        private void RoomNotAvailable()
        {
            Console.WriteLine($"The room is occupied. Please choose another date.");
            ConsoleUtils.WaitForKeyPress();
            Run();
        }
        private List<string> GetAllNamesOfConferenceRooms()
        {
            using (var context = new Context())
            {
                var rooms = context.Conferencerooms.Select(x => x.Name).ToList();
                for (int i = 0; i < rooms.Count; i++)
                {
                    rooms[i] = "[" + (1 + i) + "]" + rooms[i];
                }
                return rooms;
            }
        }
        private List<string> GetMoreInfoAboutConfereceRooms(int roomId)
        {
            using (var context = new Context())
            {
                List<string> roomdetails = new List<string>();

                var roomdetail = context.Conferencerooms.Where(x => x.Id == roomId).ToList();
                if (roomdetail.Any())
                {
                    foreach (var room in roomdetail)
                    {
                        string details = $"\n[{room.Id}] {room.Name}\n" +
                            $"Size: \n\t{room.Size} sqm\n" +
                            $"Number of Seats: \n\t{room.NumberOfSeats}\n" +
                            $"Details:{(room.WhiteBoard == true ? "\n\tWhiteboard" : "")}" +
                            $"{(room.Water == true ? "\n\tWater" : "")}" +
                            $"{(room.LCDScreen == true ? "\n\tLCD-Screen" : "")}" +
                            $"{(room.MicroPhone == true ? "\n\tMicrophone" : "")}";

                        roomdetails.Add(details);
                    }
                    return roomdetails;
                }
                else
                {
                    Console.WriteLine("The roomnumber doesn't exist.");
                    return roomdetails;
                    Run();
                }

            }

        }
    }
}

