﻿namespace ExamWebApp
{
    using SIS.MvcFramework;

    public class Program
    {
        public static void Main()
        {
            WebHost.Start(new Startup());
        }
    }
}
