using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation.Enums
{
    public enum StartMenu
    {
        Show_available_room,
        Book_a_conferenceroom,
        Get_more_details_about_Conferenceroom,
        Log_in_as_admin,
        Quit
    }
    public enum AdminMenu
    {
        Add_new_conferenceroom,
        Statistics,
        Back_to_main_menu,
        Quit
    }
    public enum StatisticMenu
    {
        Total_number_of_bookings_per_room,
        Booked_room_per_week,
        Most_frequent_guest,
        Back_to_admin,
        Back_to_main_menu,
        Quit
    }
    public enum RoomDetails
    { 
        LCDScreen,
        MicroPhone,
        WhiteBoard,
        Water
    }
    public enum Weekday
    { 
        Monday = 1,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
    }
    public enum Week
    { 
        Week_1 = 1,
        Week_2,
        Week_3,
        Week_4,
        Back_to_main_menu
    }
    public class Enums
    {
    }
}
