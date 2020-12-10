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
        Status status = Status.None;

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
                RunRecieve(Status.ReadyToConnect);
                RunConnect(Status.ReadyToConnect);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunConnect(Status statusloc)
        {
            Thread connectThread = new Thread(new ParameterizedThreadStart(Connect));
            connectThread.Start(statusloc);
        }

        public void RunRecieve(Status statusloc)
        {
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            receiveThread.Start(statusloc);
        }

        public void Connect(Object statusloc)
        {
            Status stat = (Status) statusloc;
            status = stat;
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                Thread.Sleep(300);
                int sleepTime = 25;
                int waitingTime = 10000;
                while (status != (stat + 1) && !appQuit)
                {
                    byte[] data = Encoding.Unicode.GetBytes(((int)stat).ToString());
                    sender.Send(data, data.Length, remoteAddress, remotePort);
                    Thread.Sleep(sleepTime);
                    waitingTime -= sleepTime;
                    if (waitingTime <= 0)
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
                sender.Send(data, data.Length, remoteAddress, remotePort);
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

        public void ReceiveData(Object statusloc)
        {
            Status stat = (Status)statusloc;
            receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (status != (stat + 1) && !appQuit)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    if(int.Parse(message) == (int)stat)
                    {
                        SendData(((int)Status.Connect).ToString());
                        Notify("Connect");
                        status = stat + 1;
                    }
                    else if (int.Parse(message) == (int)stat + 1)
                    {
                        Notify("Connect");
                        status = stat + 1;
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

        public void ClearNotify()
        {
            Notify = null;
        }
    }
}
