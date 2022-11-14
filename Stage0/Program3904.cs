// See https://aka.ms/new-console-template for more information
using System;
namespace Targil0
{
    partial class Program
    {
        static void Main(String[]args)
        {
            Welcome3904();
            Console.ReadKey();
        }

        static partial void Welcome0000();
        private static void Welcome3904()
        {
            Console.WriteLine("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }

    }
}
