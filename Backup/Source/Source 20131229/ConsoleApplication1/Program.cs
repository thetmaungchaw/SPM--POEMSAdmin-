using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public interface IService
    {
        void Serve();
    } // Interface

    public class Service : IService // Implementation
    {
        public void Serve()
        {
            Console.WriteLine("Service Called");
            //To Do: Some Stuff
        }
    }

    public class Client // Controller
    {
        private IService _service;

        public Client(IService service)
        {
            this._service = service;
        }

        public void Start()
        {
            Console.WriteLine("Service Started");
            this._service.Serve();
            //To Do: Some Stuff
        }
    }

    class Program // Main
    {
        static void Main(string[] args)
        {
            Client client = new Client(new Service());
            client.Start();

            Console.ReadKey();
        }
    }
}