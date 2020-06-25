using System;
using System.Collections.Generic;
using System.IO;
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
                        string songAuthor = "";
                        while (string.IsNullOrEmpty(songAuthor))
                        {
                            Console.WriteLine("\nAuthor's name:");
                            songAuthor = Console.ReadLine();
                        }
                        string songName = "";
                        while (string.IsNullOrEmpty(songName))
                        {
                            Console.WriteLine("\nSong name:");
                            songName = Console.ReadLine();
                        }
                        Console.WriteLine("\nSong duration in hours:");
                        string songDurationHours = EnterDuration();

                        Console.WriteLine("\nSong duration in minutes:");
                        string songDurationMinutes = EnterDuration();

                        Console.WriteLine("\nSong duration in seconds:");
                        string songDurationSeconds = EnterDuration();
                       
                        AddSong(songAuthor + ": " + songName + " " + songDurationHours + ":" + songDurationMinutes + ":" + songDurationSeconds);

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

        public static void AddSong(string song)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"..\..\Music.txt", true))
                {
                    sw.WriteLine(song);
                }
            }
            catch
            {
            }
        }

        public static string EnterDuration()
        {
            string songDuration = "aa";
            while (!char.IsDigit(songDuration.ElementAt(0)) || !char.IsDigit(songDuration.ElementAt(1))
                         || Convert.ToInt32(songDuration) > 60 || songDuration.Length != 2)
            {
                Console.WriteLine("Time:");
                songDuration = Console.ReadLine();
                if (string.IsNullOrEmpty(songDuration))
                {
                    songDuration = "00";
                }
                else if (songDuration.Length == 1)
                {
                    songDuration = "0" + songDuration;
                }
            }
            return songDuration;
        }
    }
}
