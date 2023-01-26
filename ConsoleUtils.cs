using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReservation
{
    static class ConsoleUtils
    {
        public static void WaitForKeyPress()
        {
            Console.WriteLine("(Press any key to continue.)");
            Console.ReadKey(true);
        }

        public static void QuitConsole()
        {
            Console.WriteLine("(Press any key to exit.)");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        public static string GetStringFromUser(string prompt)
        {
            Console.Write(prompt);
            string? result = Console.ReadLine();
            if (result == null)
            {
                result = "";
            }
            return result;
        }
        public static int GetIntFromUser(string prompt)
        {
            bool isValid = true;
            int result;
            do
            {
                if (!isValid)
                {
                    Console.WriteLine("Please enter a number.");
                }
                else isValid = false;

                Console.Write(prompt);
            } while (!int.TryParse(Console.ReadLine(), out result));

            return result;
        }
        public static void PrintList<T>(IEnumerable<T> list)
        {
            foreach (var value in list)
            {
                if (value.GetType() == typeof(int) || value.GetType() == typeof(string))
                {
                    Console.Write($"    {value.ToString().PadRight(10)}");
                }
                else
                    Console.Write($"    {value.ToString().PadRight(10)}");

            }
            Console.WriteLine();
        }
    }
}
