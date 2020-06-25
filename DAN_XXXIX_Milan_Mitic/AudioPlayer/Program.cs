using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\tAUDIO PLAYER\n");

            bool menu = true;
            while (menu == true)
            {
                Console.WriteLine("\n1. Add a song");
                Console.WriteLine("2. View all songs");
                Console.WriteLine("3. Audio player");
                Console.WriteLine("4. Exit");

                string options = Console.ReadLine();

                switch (options)
                {
                    case "1":
                        break;

                    case "2":

                        break;

                    case "3":
                        break;

                    case "4":
                        menu = false;
                        break;

                    default:
                        Console.WriteLine("\nPlease choose one of the following options.");
                        break;
                }
            }
        }
    }
}
