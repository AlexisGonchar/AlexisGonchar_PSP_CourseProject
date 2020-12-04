using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GameObjects;

namespace SeaBattleGame
{
    /// <summary>
    /// Логика взаимодействия для GameSettingsWindow.xaml
    /// </summary>
    public partial class GameSettingsWindow : Window
    {
        String[] ships = { "Аврора", "Летучий Голандец", "Форвард", "Призрак", "Пилигрим" };
        Dictionary<string, int> dc;
        public GameSettingsWindow(Dictionary<string, int> dc)
        {
            InitializeComponent();
            initializeShipParameters();
            Ship1ComboBox.ItemsSource = Ship2ComboBox.ItemsSource = ships;
            this.dc = dc;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            dc.Add("Ship1", Ship1ComboBox.SelectedIndex);
            dc.Add("Ship2", Ship2ComboBox.SelectedIndex);
            Close();
        }

        private void initializeShipParameters()
        {
            int[] parameters;
            for(int i = 0; i < 5; i++)
            {
                parameters = Features.getShipParameters(i);
                for (int j = 0; j < 5; j++)
                    ((Label)this.FindName("labelPar" + ((i * 5) + (j + 1)))).Content = parameters[j].ToString();
            }
        }
    }
}
