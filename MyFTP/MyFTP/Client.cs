using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyFTP
{
    public class Client
    {
        private int _port;
        private string _host;

        Client(int port, string host)
        {
            _port = port;
            _host = host;
        }

        public async Task List(string path, Stream stream)
        {
            
        }
        
        public async Task Get(string path, Stream stream)
        {
            
        }
    }
}
