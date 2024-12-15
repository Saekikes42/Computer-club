using Microsoft.EntityFrameworkCore;
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

namespace designclub
{
    /// <summary>
    /// Логика взаимодействия для ProvidedServicesWindow.xaml
    /// </summary>
    public partial class ProvidedServicesWindow : Window
    {

        private readonly MyDbContext _dbContext;
        public ProvidedServicesWindow(string fullName, string role)
        {


            InitializeComponent();
            Namex.Text = fullName;
            Rolex.Text = role;
            _dbContext = new MyDbContext();
            this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            this.WindowState = WindowState.Maximized; // Максимальный размер окна
            this.Topmost = true; // Поверх всех окон
            var providedServices = App.IsDemoMode
                ? DemoData.ProvidedServices
                : _dbContext.Providedminutepackages
                    .Include(ps => ps.Client)
                    .Include(ps => ps.Package)
                    .ToList();
            GetStatisticsButton.IsEnabled = !App.IsDemoMode;
            StartClock();
            // Привязка данных к DataGrid
            ProvidedServicesDataGrid.ItemsSource = providedServices;
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
            var namex = Namex.Text;
            var rolex = Rolex.Text;


            ClientsWindow clientsWindow = new ClientsWindow(namex, rolex);
            clientsWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var namex = Namex.Text;
            var rolex = Rolex.Text;


            ServicesWindow servicesWindow = new ServicesWindow(namex, rolex);
            servicesWindow.Show();
            this.Close();
        }

        // Метод для получения статистики по оказанным услугам
        private void GetStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var statistics = _dbContext.Providedminutepackages
                .GroupBy(ps => ps.Package.Name)
                .Select(g => new
                {
                    ServiceName = g.Key,
                    TotalServicesProvided = g.Count(),
                    TotalRevenue = g.Sum(ps => ps.Cost),
                    TotalBonusGiven = g.Sum(ps => ps.Cost * 0.30m) // 30% от стоимости услуги - бонусы
                }).ToList();

            // Создание нового окна для отображения статистики
            var statisticsWindow = new Window
            {
                Title = "Статистика по оказанным услугам",
                Width = 500,
                Height = 300,
                Content = new DataGrid
                {
                    AutoGenerateColumns = true,
                    ItemsSource = statistics
                }
            };

            statisticsWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
