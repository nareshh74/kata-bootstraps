﻿using System;

namespace DotnetStarter.Logic.ElephantCarpaccio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.WelcomeUser();
            int itemCount = Program.GetItemCount();
            Program.OutputItemCount(itemCount);
            decimal itemPrice = Program.GetItemPrice();
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

        public static void OutputItemCount(int itemCount)
        {
            Console.WriteLine($"You have entered {itemCount} items.");
        }

        public static decimal GetItemPrice()
        {
            Console.WriteLine("Enter the price of the item:");
            return decimal.Parse(Console.ReadLine());
        }
    }
}
