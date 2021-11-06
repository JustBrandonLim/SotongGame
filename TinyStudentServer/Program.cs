using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TinyStudentServer
{
    public class Program
    {
        private const string BASE_ADDRESS = "http://localhost:9000/";

        static void Main(string[] args)
        {
            Console.WriteLine("STARTING TINYSTUDENT SERVER...");
            using (WebApp.Start<Startup>(BASE_ADDRESS))
            {
                Console.WriteLine("STARTED TINYSTUDENT SERVER...");
                Console.ReadLine();
            }
        }
    }
}
