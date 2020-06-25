using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AudioPlayer
{
    class Program
    {
        static Dictionary<int, string> songs = new Dictionary<int, string>();
        static AutoResetEvent are = new AutoResetEvent(false);
        static bool playerRunning = false;

        static void Main(string[] args)
        {
            Console.WriteLine("\tAUDIO PLAYER\n");

            bool menu = true;
            while (menu == true)
            {
                if (playerRunning == true)
                {
                    are.WaitOne();
                }

                //start menu
                Console.WriteLine("\n1. Add a song");
                Console.WriteLine("2. View all songs");
                Console.WriteLine("3. Audio player");
                Console.WriteLine("4. Exit");

                string options = Console.ReadLine();

                switch (options)
                {
                    case "1":
                        // insert song's author, name and duration.
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
                        ViewAllSongs();
                        break;

                    case "3":
                        ViewAllSongs();
                        string songNumber = Console.ReadLine();
                        Thread player = new Thread(() => AudioPlayer(songNumber));
                        player.Start();
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

        public static void AudioPlayer(string songNumber)
        {
            try
            {
                string song = songs[Convert.ToInt32(songNumber)];
                Console.WriteLine("\nThe time of song starts playing is " + DateTime.Now.ToString());
                Console.WriteLine("Song name is: " + song);
            }
            catch
            {
                Console.WriteLine("\nSong with number " + songNumber + " does not exist.");
            }
            are.Set();
        }

        /// <summary>
        /// Appends the string to a txt file.
        /// </summary>
        /// <param name="song"></param>
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

        /// <summary>
        /// Asks to enter time in 00 format as long as the insert is not correct and returns an entered string. 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Views all lines from a txt files and adds them to a dictionary.
        /// </summary>
        public static void ViewAllSongs()
        {
            playerRunning = true;
            songs.Clear();
            int i = songs.Keys.Count;
            try
            {
                using (StreamReader sr = new StreamReader("../../Music.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {

                        while (true)
                        {
                            try
                            {
                                i++;
                                songs.Add(i, line);
                                break;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        Console.WriteLine(i + ". " + line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
