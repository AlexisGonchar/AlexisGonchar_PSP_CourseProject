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
        private MainViewModel vm;
        private GameSettingsWindow gsw;
        private Dictionary<string, int> dc;
        private String[] ships = { "Аврора", "Летучий Голандец", "Форвард", "Призрак", "Пилигрим" };
        private Client client;

        public MainWindow(Client client)
        {
            InitializeComponent();
            this.client = client;
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
            client.ClearNotify();
            client.Notify += OpenGameDispatcher;
            client.RunRecieve(Status.ReadyToPlay);
            client.RunConnect(Status.ReadyToPlay);            
        }

        public void OpenGameDispatcher(string message)
        {
            Dispatcher.Invoke(OpenGame);
        }

        public void OpenGame()
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
                        Content = new Renderer(dc)
                    };
                    DataContext = vm;
                }
                catch
                {

                }
            }
        }
    }
}
