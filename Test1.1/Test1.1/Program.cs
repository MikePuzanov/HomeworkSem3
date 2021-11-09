using System;
using System.Threading.Tasks;

namespace Test1._1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {
                var server = new Server(Int32.Parse(args[0]));
                await server.Working();
            }
            else
            {
                var client = new Client(args[0], Int32.Parse(args[1]));
                client.Working();
            }
        }
    }
}