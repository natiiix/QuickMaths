using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace QuickMaths
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            foreach (string path in args)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"Source file does not exist! ({path})");
                    return;
                }

                Console.WriteLine($"---- {path} ----");

                try
                {
                    new Engine(File.ReadAllLines(path)).Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }
    }
}