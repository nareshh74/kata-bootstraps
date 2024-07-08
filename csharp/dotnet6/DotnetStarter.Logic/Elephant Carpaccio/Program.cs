using System;

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
            string stateCode = Program.GetStateCode();
            Program.OutputItemPrice(itemPrice);
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

        public static string GetStateCode()
        {
            Console.WriteLine("Enter the state code:");
            return Console.ReadLine();
        }

        public static void OutputStateCode(string stateCode)
        {
            Console.WriteLine($"The state code is {stateCode}.");
        }

        public static void OutputItemPrice(decimal itemPrice)
        {
            Console.WriteLine($"The price of the item is {itemPrice}.");
        }

        public static void OutputTotalPrice(int itemCount, decimal itemPrice)
        {
            decimal totalPrice = PriceCalculator.Create().GetTotalPrice(itemCount, itemPrice);
            Console.WriteLine($"The total price is {totalPrice}.");
        }
    }
}
