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
        UdpClient receiver;
        private string remoteAddress; // the addres to which we connect
        private int remotePort; // the port to which we connect
        private int localPort; // local port
        bool connect, appQuit;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;
        public Client(string ip, int remotePort, int localPort)
        {
            try
            {
                this.localPort = localPort;
                remoteAddress = ip;
                this.remotePort = remotePort;
                connect = false;
                appQuit = false;
                Thread connectThread = new Thread(new ThreadStart(Connect));
                Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.Start();
                connectThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Connect()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                int sleepTime = 15;
                int waitingTime = 10000;
                while (!connect && !appQuit)
                {
                    byte[] data = Encoding.Unicode.GetBytes("1");
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                    Thread.Sleep(sleepTime);
                    waitingTime -= sleepTime;
                    if(waitingTime <= 0)
                    {
                        Notify("Timeout");
                        sender.Close();
                        Thread.CurrentThread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        public void SendData(string message)
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        public void ReceiveData()
        {
            receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (!appQuit)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    if(message == "1")
                    {
                        SendData("2");
                    }
                    else if (message == "2")
                    {
                        connect = true;
                        Notify("Connect");
                    }
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
            appQuit = true;
        }
    }
}
