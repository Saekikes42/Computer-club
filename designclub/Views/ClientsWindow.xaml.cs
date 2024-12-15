using designclub.Models;
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
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        private readonly MyDbContext _dbContext;
        public ClientsWindow(string fullName, string role)
        {
            InitializeComponent();
            _dbContext = new MyDbContext();
            if (App.IsDemoMode)
                ClientsDataGrid.ItemsSource = DemoData.Clients;
            else
                LoadClients();

            this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            this.WindowState = WindowState.Maximized; // Максимальный размер окна
            this.Topmost = true; // Поверх всех окон
            if (App.IsDemoMode)
            {
                AddClientButton.IsEnabled = false;
                EditClientButton.IsEnabled = false;
                DeleteClientButton.IsEnabled = false;
            }
            Rolex.Text = role;
            Namex.Text = fullName;
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





        private void LoadClients()
        {
            // Загружаем список клиентов из базы данных
            var clients = _dbContext.Clients.ToList();
            ClientsDataGrid.ItemsSource = clients;
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {

            var addClientWindow = new AddClientWindow(_dbContext);
            addClientWindow.Owner = this;
            if (addClientWindow.ShowDialog() == true)
            {
                LoadClients(); // Обновляем таблицу после добавления клиента
            }

        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient)
            {
                // Передаем клиента и контекст базы данных в окно редактирования
                var editClientWindow = new EditClientWindow(selectedClient, _dbContext);  // Передаем MyDbContext
                editClientWindow.Owner = this;

                // Если окно редактирования закрывается с успешным результатом
                if (editClientWindow.ShowDialog() == true)
                {
                    LoadClients(); // Обновляем таблицу после сохранения изменений
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования.");
            }
        }



        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {selectedClient.FullName}?",
                                             "Удаление клиента", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Clients.Remove(selectedClient);
                    _dbContext.SaveChanges();
                    LoadClients(); // Обновляем таблицу
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var namex = Namex.Text;
            var rolex = Rolex.Text;


            ServicesWindow servicesWindow = new ServicesWindow(namex, rolex);
            servicesWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var namex = Namex.Text;
            var rolex = Rolex.Text;



            ProvidedServicesWindow providedServicesWindow = new ProvidedServicesWindow(namex, rolex);
            providedServicesWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
