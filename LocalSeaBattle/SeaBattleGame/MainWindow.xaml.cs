using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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

        public MainWindow()
        {
            InitializeComponent();
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
