using BookReservation.Enums;
using BookReservation.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation.Scenes
{
    public class AdminScene : Scene
    {
        public AdminScene(Booking reservation) : base(reservation)
        {

        }
        public override void Run()
        {
            string prompt = "Welcome admin! \nWhat would you like to do?\n";
            string[] options = Enum.GetNames(typeof(AdminMenu));

            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    AddNewConferenceRoom();
                    break;
                case 1:
                    MyReservation.MyStatisticScene.Run();
                    break;
                case 2:
                    MyReservation.MyStartScene.Run();
                    break;
                case 3:
                    ConsoleUtils.QuitConsole();
                    break;
            }
        }
        private void AddNewConferenceRoom()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nPlease enter details for the new Conferenceroom.\n");
                string? name = ConsoleUtils.GetStringFromUser("Name: ");
                int numberOfSeats = ConsoleUtils.GetIntFromUser("Number of seats: ");
                int size = ConsoleUtils.GetIntFromUser("Size (sqm): ");
                string[] roomDetails = Enum.GetNames(typeof(RoomDetails));

                bool[] roomDetailExistOrNot = SetRoomDetails(roomDetails);
                string result = GetRoomDetails(roomDetails, roomDetailExistOrNot);

                Console.WriteLine($"Name: {name} Size: {size} Seats: {numberOfSeats} Details:{result} ");

                string answer = ConsoleUtils.GetStringFromUser($"Is above information correct? <j/n> ");
                running = (answer.Trim().ToLower().StartsWith("j") ? false : true);
                if (running == false)
                {
                    UpdateDatabaseWithNewConferenceRoom(name, numberOfSeats, size, roomDetailExistOrNot);
                }
                else
                {
                    Console.WriteLine("The information wasn't saved");
                }
                answer = ConsoleUtils.GetStringFromUser($"Do you want to add another conferenceroom? <j/n> ");
                running = (answer.Trim().ToLower().StartsWith("j") ? true : false);
                if (running == false)
                {
                    Run();
                }
            }
        }
        private bool[] SetRoomDetails(string[] roomdetails)
        {
            bool[] boolRoomdetails = new bool[roomdetails.Length];
            for (int i = 0; i < roomdetails.Length; i++)
            {
                string answer = ConsoleUtils.GetStringFromUser($"Does the room have {roomdetails[i]}? j/n: ");
                boolRoomdetails[i] = (answer.Trim().ToLower().StartsWith("j") ? true : false);
            }
            return boolRoomdetails;
        }
        private string GetRoomDetails(string[] roomdetails, bool[] roomdetailExist)
        {
            string result = "";

            for (int i = 0; i < roomdetails.Length; i++)
            {
                string detail = roomdetails[i];
                string detailExistOrNot = roomdetailExist[i] ? "yes" : "no";

                result += " " + detail + " : " + detailExistOrNot;
            }
            return result;
        }
        private void UpdateDatabaseWithNewConferenceRoom(string name, int numberOfSeats, int size, bool[] existsOrNot)
        {
            using (var _context = new Context())
            {
                var conferenceroom =
                  new Conferenceroom
                  {
                      Name = name,
                      NumberOfSeats = numberOfSeats,
                      Size = size,
                      LCDScreen = existsOrNot[0],
                      MicroPhone = existsOrNot[1],
                      WhiteBoard = existsOrNot[2],
                      Water = existsOrNot[3],
                  };
                if (conferenceroom != null) 
                {
                    _context.Add(conferenceroom);
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Input wasn't saved");
                    Run();
                }
            }
        }
    }
}

