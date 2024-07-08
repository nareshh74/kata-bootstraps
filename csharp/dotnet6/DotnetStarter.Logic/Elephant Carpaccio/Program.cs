using System;

namespace DotnetStarter.Logic.ElephantCarpaccio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.WelcomeUser();
            int itemCount = Program.GetItemCount();
        }

        public static void WelcomeUser()
        {
            Console.WriteLine("Welcome User!");
        }

        public static int GetItemCount()
        {
            Console.WriteLine("Enter the number of items:");
            return int.Parse(Console.ReadLine());
        }
    }
}
