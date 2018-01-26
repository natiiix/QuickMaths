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
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments!");
                return;
            }

            string sourcePath = args[0];

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist!");
                return;
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                new Engine(File.ReadAllLines(sourcePath)).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}