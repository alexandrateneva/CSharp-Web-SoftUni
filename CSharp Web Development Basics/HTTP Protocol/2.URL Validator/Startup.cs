namespace _2.URL_Validator
{
    using System;
    using System.Net;
    using System.Text;

    public class Startup
    {
        public static void Main()
        {
            Console.Write("Enter encoded URL:  ");
            var input = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(input);
            try
            {
                var url = new Uri(decodedUrl);

                // Required URL components
                if (string.IsNullOrWhiteSpace(url.Scheme) ||
                    string.IsNullOrWhiteSpace(url.Host) ||
                    string.IsNullOrWhiteSpace(url.LocalPath) ||
                    !url.IsDefaultPort)
                {
                    throw new ArgumentException("Invalid URL");
                }

                var output = new StringBuilder();
                output
                    .AppendLine($"Protocol: {url.Scheme}")
                    .AppendLine($"Host: {url.Host}")
                    .AppendLine($"Port: {url.Port}")
                    .AppendLine($"Path: {url.LocalPath}");

                // Optional URL components
                if (!string.IsNullOrWhiteSpace(url.Query))
                {
                    output.AppendLine($"Query: {url.Query.Substring(1)}");
                }

                if (!string.IsNullOrWhiteSpace(url.Fragment))
                {
                    output.AppendLine($"Fragment: {url.Fragment.Substring(1)}");
                }

                Console.WriteLine(output.ToString().Trim());
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid URL");
            }
        }
    }
}
