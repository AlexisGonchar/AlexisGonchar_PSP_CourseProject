using System;
using System.Windows;
using System.Windows.Media;
using UdpLib;

namespace SeaBattleUI
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Action<string> action;
        Client client;
        public MainWindow()
        {
            InitializeComponent();
            action = AddMessage;
        }

        private void buttonCreateClient_Click(object sender, RoutedEventArgs e)
        {
            string ip = textBoxIpAddress.Text;
            int localPort = int.Parse(textBoxLocalPort.Text);
            int remotePort = int.Parse(textBoxRemotePort.Text);
            client = new Client(ip, remotePort, localPort);
            client.Notify += ShowMessage;
            labelReceive.Content = "Waiting for connection...";
            buttonCancel.Visibility = Visibility.Visible;
            buttonCreateClient.Visibility = Visibility.Hidden;
        }

        private void AddMessage(string message)
        {
            labelReceive.Content = message;
        }

        private void ShowMessage(string message)
        {
            Dispatcher.Invoke(action, message);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
            {
                client.CloseClient();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            buttonCancel.Visibility = Visibility.Hidden;
            buttonCreateClient.Visibility = Visibility.Visible;
            client.CloseClient();
            client = null;
            labelReceive.Content = "";
        }
    }
}
