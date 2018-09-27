namespace _1.Even_Numbers_Thread
{
    using System;
    using System.Threading;

    public class Startup
    {
        public static void Main()
        {
            Console.Write("Enter start and end number separated by space: ");
            var input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var start = int.Parse(input[0]);
            var end = int.Parse(input[1]);

            Thread thread = new Thread(() => PrintEvenNumbers(start, end));
            thread.Start();
            thread.Join();
            Console.WriteLine("Thread finished work!");
        }

        private static void PrintEvenNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if(i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
