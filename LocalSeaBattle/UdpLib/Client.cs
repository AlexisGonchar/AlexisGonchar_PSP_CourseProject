using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpLib
{
    /// <summary>
    /// A class for communicating with another computer using the udp protocol.
    /// </summary>
    public class Client
    {
        private string remoteAddress; // the addres to which we connect
        private int remotePort; // the port to which we connect
        private int localPort; // local port
        bool appQuit;
        UdpClient client;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;
        public Client(string ip, int remotePort, int localPort)
        {
            try
            {
                this.localPort = localPort;
                remoteAddress = ip;
                this.remotePort = remotePort;
                appQuit = false;
                client = new UdpClient(localPort);
                RunConnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunConnect()
        {
            Thread receiveThread = new Thread(new ThreadStart(Connect));
            receiveThread.Start();
        }

        public void Connect()
        {
            Thread.Sleep(100);
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(((int)Status.ReadyToConnect).ToString());
                client.Send(data, data.Length, remoteAddress, remotePort);
                ReceiveData();
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void ReceiveData()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                byte[] data = client.Receive(ref remoteIp); // получаем данные
                string message = Encoding.Unicode.GetString(data);

                Notify(message);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void CloseClient()
        {
            if (client != null)
                client.Close();
            appQuit = true;
        }

        public void ClearNotify()
        {
            Notify = null;
        }
    }
}
