namespace CoreApi
{
    using System;
    using Microsoft.Owin.Hosting;

    public class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);
            Console.ReadLine();
        }
    }
}