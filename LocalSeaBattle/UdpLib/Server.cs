using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpLib
{
    public class Server
    {
        UdpClient server;
        private int localPort; // local port
        private bool appClose;
        List<IPEndPoint> clients;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;

        public Server(int localPort)
        {
            try
            {
                this.localPort = localPort;
                appClose = false;
                server = new UdpClient(localPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendData(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                Notify($"Sending1");
                server.Send(data, data.Length, clients[0]);
                Notify($"Sending2");
                server.Send(data, data.Length, clients[1]);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void RunServer()
        {
            RunRecieve();
        }

        private void RunRecieve()
        {
            Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.Start();
        }

        public void ReceiveData()
        {
            clients = new List<IPEndPoint>();
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (!appClose && clients.Count < 2)
                {
                    byte[] data = server.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Status status = (Status)int.Parse(message);
                    switch (status)
                    {
                        case Status.ReadyToConnect:
                            message = status.ToString();
                            clients.Add(remoteIp);
                            break;
                    }

                    Notify($"{remoteIp.Address}:{remoteIp.Port} - {message}");
                }
                Notify($"Play!");
                SendData("Connect");
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void Close()
        {
            if (server != null)
                server.Close();
            appClose = true;
        }
    }
}
