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
        private const string BASE_ADDRESS = "http://*:9000/";

        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(BASE_ADDRESS))
            {
                Console.WriteLine("SERVER STARTED");
                Console.ReadLine();
            }
        }
    }
}
