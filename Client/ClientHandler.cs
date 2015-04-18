using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientHandler
    {
       
        public ClientHandler(IPAddress ipAddress)
        {
           
            var massive = new Int32[3];
            int port = 80;            
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            var client = new TcpClient();
            client.Connect(ipEndPoint);
            Console.WriteLine("Enter three numbers");
            byte[] buffer = new byte[12];
           
            int value;
            
            for (int i = 0; i < 3; i++)
            {
                while (!int.TryParse(Console.ReadLine(), out value))
                {
                    Console.WriteLine("Value cannot be letter! Enter the number.");
                }
                massive[i] = value;                    
                
                var data = BitConverter.GetBytes(massive[i]);
                data.CopyTo(buffer, i * 4);
            }
            client.GetStream().Write(buffer, 0, buffer.Length);

            client.Close();
        }

        
    }
}
