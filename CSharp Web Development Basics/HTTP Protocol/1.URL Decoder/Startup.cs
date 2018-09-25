namespace _1.URL_Decoder
{
    using System;
    using System.Net;

    public class Startup
    {
        public static void Main()
        {
            Console.Write("Enter encoded URL:  ");
            var input = Console.ReadLine();
            var output = WebUtility.UrlDecode(input);
            Console.Write("Decoded URL:  ");
            Console.WriteLine(output);
        }
    }
}
