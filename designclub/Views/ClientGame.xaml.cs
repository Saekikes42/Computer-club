using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Diagnostics;
using designclub.Models;


namespace designclub
{
    /// <summary>
    /// Логика взаимодействия для ClientGame.xaml
    /// </summary>
    public partial class ClientGame : Window
    {
        public ClientGame(string name, decimal balance)
        {
            InitializeComponent();


            NameWindow.Text = name;
            BalanceWindow.Text = balance.ToString();
            this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            this.WindowState = WindowState.Maximized; // Максимальный размер окна
            this.Topmost = true; // Поверх всех окон
            StartClock();
        }
        private void StartClock()
        {
            var timer = new DispatcherTimer(); // Инициализация timer
            timer.Interval = TimeSpan.FromSeconds(1); // Интервал обновления - 1 секунда
            timer.Tick += Timer_Tick; // Подписка на событие Tick
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ClockTextBlock.Text = DateTime.Now.ToString("HH:mm"); // Отображение времени
            datatime.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var balance = BalanceWindow.Text;
            var name = NameWindow.Text;
            ServiceClientGame serviceClientGame = new ServiceClientGame(name, balance);
            serviceClientGame.Show();
            this.Close();
        }
    }
}
