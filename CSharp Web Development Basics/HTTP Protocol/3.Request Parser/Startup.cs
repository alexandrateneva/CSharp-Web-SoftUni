namespace _3.Request_Parser
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class Startup
    {
        public static void Main()
        {
            var methodsByUrls = new Dictionary<string, HashSet<string>>();

            // Read methods and paths
            var input = Console.ReadLine();

            while (input != "END")
            {
                var partsOfInput = input.Split("/", StringSplitOptions.RemoveEmptyEntries);
                var path = partsOfInput[0].ToLower();
                var method = partsOfInput[1].ToLower();

                if (!methodsByUrls.ContainsKey(method))
                {
                    methodsByUrls.Add(method, new HashSet<string>());
                }
                methodsByUrls[method].Add(path);

                input = Console.ReadLine();
            }

            // Process Http Request
            var request = Console.ReadLine();
            var partsOfRequest = request.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var requestMethod = partsOfRequest[0].ToLower();
            var requestPath = partsOfRequest[1].Substring(1).ToLower();
            var requestProtocol = partsOfRequest[2];

            var statusCode = "404 Not Found";
            var responseText = "NotFound";          

            var paths = methodsByUrls[requestMethod];
            if (paths != null && paths.Contains(requestPath))
            {
                statusCode = "200 OK";
                responseText = "OK";
            }

            var output = new StringBuilder();
            output
                .AppendLine($"HTTP/1.1 {statusCode}")
                .AppendLine($"Content-Length: {responseText.Length}")
                .AppendLine("Content-Type: text/plain")
                .AppendLine()
                .AppendLine($"{responseText}");

            Console.WriteLine(output);
        }
    }
}
