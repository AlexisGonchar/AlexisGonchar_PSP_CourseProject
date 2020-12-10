using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UdpLib;

namespace SeaBattleGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Action<string> action;
        private MainViewModel vm;
        private GameSettingsWindow gsw;
        private Dictionary<string, int> dc;
        private String[] ships = { "Аврора", "Летучий Голандец", "Форвард", "Призрак", "Пилигрим" };
        private Client client;
        int idShip;


        public MainWindow()
        {
            InitializeComponent();
            action = OpenGame;
            dc = new Dictionary<string, int>();
            dc.Add("Ship1", 0);
            dc.Add("Ship2", 0);
        }

        private void openSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            dc = new Dictionary<string, int>();
            gsw = new GameSettingsWindow(dc);
            gsw.Show();
            openSettingsButton.IsEnabled = false;
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = textBoxIpAddress.Text;
            int localPort = int.Parse(textBoxLocalPort.Text);
            int remotePort = int.Parse(textBoxRemotePort.Text);
            client = new Client(ip, remotePort, localPort, dc["Ship1"]);
            client.Notify += OpenGameDispatcher;
            labelReceive.Content = "Waiting for connection...";
        }

        public void OpenGameDispatcher(string message)
        {
            Dispatcher.Invoke(action, message);
        }

        public void OpenGame(string message)
        {
            labelReceive.Content = message;
            if (message == "Connect")
            {
                client.ClearNotify();
                if (App.Current.Windows.OfType<GameSettingsWindow>().Count() == 0 && dc != null)
                {
                    labelShips.Content = ships[dc["Ship1"]] + " и " + ships[dc["Ship2"]];
                    openSettingsButton.IsEnabled = true;
                    try
                    {
                        vm = new MainViewModel
                        {
                            Content = new Renderer(dc, idShip, client)
                        };
                        DataContext = vm;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                idShip = int.Parse(message.Split('|')[1]);
                int typeShip = int.Parse(message.Split('|')[0]);
                dc["Ship2"] = typeShip;
            }
            
        }

        private void buttonLocalPortUp_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textBoxLocalPort.Text);

            if (port < 10000 && port > 0)
                textBoxLocalPort.Text = (port + 1).ToString();
        }

        private void buttonLocalPortDown_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textBoxLocalPort.Text);

            if (port < 10000 && port > 0)
                textBoxLocalPort.Text = (port - 1).ToString();
        }

        private void buttonRemotePortUp_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textBoxRemotePort.Text);

            if (port < 10000 && port > 0)
                textBoxRemotePort.Text = (port + 1).ToString();
        }

        private void buttonRemotePortDown_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textBoxRemotePort.Text);

            if (port < 10000 && port > 0)
                textBoxRemotePort.Text = (port - 1).ToString();
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
            {
                client.CloseClient();
            }
        }
    }
}
