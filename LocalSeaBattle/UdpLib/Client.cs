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
        int idShip;
        public ShipData MyShip, EnemyShip;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;
        public Client(string ip, int remotePort, int localPort, int idShip)
        {
            try
            {
                this.localPort = localPort;
                remoteAddress = ip;
                this.remotePort = remotePort;
                appQuit = false;
                client = new UdpClient(localPort);
                this.idShip = idShip;
                RunConnect();
                MyShip = new ShipData();
                EnemyShip = new ShipData();
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
                byte[] data = Encoding.Unicode.GetBytes(idShip.ToString());
                client.Send(data, data.Length, remoteAddress, remotePort);
                ReceiveData();
                ReceiveData();
                RunGameLogic();
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

        private void RunGameLogic()
        {
            Thread receiveThread = new Thread(new ThreadStart(ShipDataRecieve));
            receiveThread.Start();
            Thread sendThread = new Thread(new ThreadStart(ShipDataSend));
            sendThread.Start();
        }

        public void ShipDataRecieve()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения
            
                while (!appQuit)
                {
                    byte[] data = client.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    string[] values = message.Split('|');
                    EnemyShip.x = int.Parse(values[0]);
                    EnemyShip.y = int.Parse(values[1]);
                    EnemyShip.dircetion = int.Parse(values[2]);
                    EnemyShip.bullet = int.Parse(values[3]);
                    EnemyShip.mode = int.Parse(values[4]);
                }
            
        }

        public void ShipDataSend()
        {
            try
            {
                while (!appQuit)
                {
                    byte[] data = Encoding.Unicode.GetBytes(MyShip.x + "|" + MyShip.y + "|" + MyShip.dircetion + "|" + MyShip.bullet + "|" + MyShip.mode);
                    if(MyShip.bullet == 1)
                    {
                        MyShip.bullet = 0;
                    }
                    if (MyShip.mode == 1)
                    {
                        MyShip.mode = 0;
                    }
                    client.Send(data, data.Length, remoteAddress, remotePort);
                    Thread.Sleep(30);
                }
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
