using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpLib
{
    public class Server
    {
        UdpClient receiver;
        private string remoteAddress; // the addres to which we connect
        private int remotePort; // the port to which we connect
        private int localPort; // local port
        private bool appClose;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;

        public Server(string ip, int remotePort, int localPort)
        {
            try
            {
                this.localPort = localPort;
                remoteAddress = ip;
                this.remotePort = remotePort;
                appClose = false;
                RunRecieve();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunRecieve()
        {
            Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
        }

        public void ReceiveData()
        {
            receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (!appClose)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Notify(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }

        public void CloseClient()
        {
            if (receiver != null)
                receiver.Close();
            appClose = true;
        }
    }
}
