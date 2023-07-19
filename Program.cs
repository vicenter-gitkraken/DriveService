using System;
using System.IO;

namespace DriveService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var driveService = new DriveService();

            foreach (DriveInfo driveInfo in driveService.GetDrives())
            {
                Console.WriteLine($"================ {driveInfo.Name} ================");

                driveService.PrintDiskProperties(driveInfo);
            }

            Console.ReadKey();
        }
    }
}