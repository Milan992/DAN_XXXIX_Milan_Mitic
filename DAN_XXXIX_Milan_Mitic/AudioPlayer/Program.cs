using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AudioPlayer
{
    class Program
    {
        static Dictionary<int, string> songs = new Dictionary<int, string>();
        static AutoResetEvent menuView = new AutoResetEvent(false);
        static AutoResetEvent songStart = new AutoResetEvent(false);
        static AutoResetEvent songPlayingE = new AutoResetEvent(false);
        static AutoResetEvent commercialsStart = new AutoResetEvent(false);
        static bool playerRunning = false;
        static bool songPlayingB = false;
        static int hoursRunning;
        static int minutesRunning;
        static int secondsRunning;
        static Random random = new Random();
        static bool playing = true;

        static void Main(string[] args)
        {
            Console.WriteLine("\tAUDIO PLAYER\n");

            //reset to false so program can run multiple times
            playerRunning = false;
            songPlayingB = false;

            bool menu = true;
            while (menu == true)
            {
                if (playerRunning == true)
                {
                    menuView.WaitOne();
                }
                if (songPlayingB == true)
                {
                    songPlayingE.WaitOne();
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
                        ViewAll();
                        break;

                    case "3":
                        playing = true;
                        ViewAllSongs();
                        string songNumber = Console.ReadLine();
                        Thread player = new Thread(() => AudioPlayer(songNumber));
                        Thread songPlaying = new Thread(() => PlaySong());
                        Thread commercials = new Thread(() => PlayCommercials());

                        player.Start();
                        commercials.IsBackground = true;
                        songPlaying.Start();
                        commercials.Start();
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

        private static void PlayCommercials()
        {
            commercialsStart.WaitOne();
            string[] commercials = new string[5];
            try
            {
                using (StreamReader sr = new StreamReader("../../Commercials.txt"))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        commercials[i] = line;
                        i++;
                    }
                }
            }
            catch { }

            while (playing == true)
            {
                Thread.Sleep(200);
                Console.WriteLine(commercials[random.Next(0, 4)]);
                if (Console.KeyAvailable)
                {
                    break;
                }
            }
        }

        public static void PlaySong()
        {
            songStart.WaitOne();

            TimeSpan ts = new TimeSpan(hoursRunning, minutesRunning, secondsRunning);
            Stopwatch s = new Stopwatch();
            s.Start();
            while (ts.TotalMilliseconds > s.ElapsedMilliseconds)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Song is playing. . .");
                if (Console.KeyAvailable)
                {
                    break;
                }
            }
            Console.WriteLine("\nSong ended.");
            playing = false;
            songPlayingE.Set();
        }

        public static void AudioPlayer(string songNumber)
        {
            try
            {
                string song = songs[Convert.ToInt32(songNumber)];
                Console.WriteLine("\nThe time of song starts playing is " + DateTime.Now.ToString());
                Console.WriteLine("Song name is: " + song);
                Console.WriteLine("\n\tPress any key twice to stop song playing.");
                songPlayingB = true;

                // getting song duration from a string
                char[] songLetters = song.ToCharArray();
                string hours = Convert.ToString(songLetters[songLetters.Length - 8]) + Convert.ToString(songLetters[songLetters.Length - 7]);
                string minutes = Convert.ToString(songLetters[songLetters.Length - 5]) + Convert.ToString(songLetters[songLetters.Length - 4]);
                string seconds = Convert.ToString(songLetters[songLetters.Length - 2]) + Convert.ToString(songLetters[songLetters.Length - 1]);

                hoursRunning = Convert.ToInt32(hours);
                minutesRunning = Convert.ToInt32(minutes);
                secondsRunning = Convert.ToInt32(seconds);

                songStart.Set();
                commercialsStart.Set();
            }
            catch
            {
                Console.WriteLine("\nSong with number " + songNumber + " does not exist.");
            }

            menuView.Set();
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
        public static void ViewAll()
        {
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
