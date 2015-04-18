using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_Server
{
    class ServerHandler
    {
        int sum = 0;
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        public ServerHandler(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();


            while (true)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(AcceptClient, listener);
                tcpClientConnected.WaitOne();
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            var listener = ar.AsyncState as TcpListener;
            var client = listener.EndAcceptTcpClient(ar);
            tcpClientConnected.Set();
            HandleRequest(client);
        }

        public void HandleRequest(TcpClient client)
        {
            int[] mass = new int[3];
            byte[] buffer = new byte[100];

            try
            {
                client.GetStream().Read(buffer, 0, buffer.Length);
            }
            catch
            {
                client.Close();
            }

            for (int i = 0; i < 3; i++)
            {
                mass[i] = BitConverter.ToInt32(buffer, i * 4);
                sum = sum + mass[i];
            }
            Console.WriteLine("Total: {0}", sum);
            client.Close();
        }
    }
}
