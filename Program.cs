namespace BookReservation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Conferencebooking";
            Booking reservation = new Booking();
            reservation.Start();
        }
    }
}