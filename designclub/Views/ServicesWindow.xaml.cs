using designclub.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Window
    {
        private readonly MyDbContext _dbContext;

        public ServicesWindow(string namex, string rolex)
        {
            InitializeComponent();
            _dbContext = new MyDbContext();

            this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            this.WindowState = WindowState.Maximized; // Максимальный размер окна
            this.Topmost = true; // Поверх всех окон
            if (App.IsDemoMode)
            {
                AddServiceButton.IsEnabled = false;
                EditServiceButton.IsEnabled = false;
                ArchiveServiceButton.IsEnabled = false;
                UnarchiveServiceButton.IsEnabled = false;
            }
            Rolex.Text = namex;
            Namex.Text = rolex;
            StartClock();
            if (App.IsDemoMode)
                ServicesDataGrid.ItemsSource = DemoData.Services;
            else
                LoadServices(); // Загружаем услуги при открытии окна
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

        // Метод для загрузки услуг из базы данных
        private void LoadServices()
        {


            ServicesDataGrid.ItemsSource = _dbContext.Minutepackages.ToList();
        }


        // Метод добавления новой услуги
        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditServiceWindow(null, _dbContext);
            addWindow.Owner = this;
            addWindow.ShowDialog();

            LoadServices(); // Перезагружаем список услуг
        }

        // Метод редактирования выбранной услуги
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesDataGrid.SelectedItem != null)
            {
                MessageBox.Show($"Тип выбранного элемента: {ServicesDataGrid.SelectedItem.GetType()}");

                if (ServicesDataGrid.SelectedItem is Minutepackage selectedService)
                {
                    var editWindow = new EditServiceWindow(selectedService, _dbContext);

                    editWindow.Owner = this;
                    bool? result = editWindow.ShowDialog();

                    if (result == true)
                    {
                        MessageBox.Show("Услуга успешно отредактирована!");
                        LoadServices(); // Перезагрузка списка услуг
                    }
                }
                else
                {
                    MessageBox.Show("Выбранный элемент не является типом Minutepackage.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для редактирования.");
            }
        }

        // Метод для перемещения услуги в архив
        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            var service = ServicesDataGrid.SelectedItem as Minutepackage;
            if (service == null)
            {
                MessageBox.Show("Выберите услугу для архивации.");
                return;
            }

            service.IsArchived = true; // Перемещаем услугу в архив
            _dbContext.SaveChanges();

            MessageBox.Show("Услуга перемещена в архив.");
            LoadServices(); // Перезагружаем список услуг
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var namex = Namex.Text;
            var rolex = Rolex.Text;


            ClientsWindow clientsWindow = new ClientsWindow(namex, rolex);
            clientsWindow.Show();
            this.Close();
        }

        private void NoArchiveButton_Click(object sender, RoutedEventArgs e)
        {

            var service = ServicesDataGrid.SelectedItem as Minutepackage;
            if (service == null)
            {
                MessageBox.Show("Выберите услугу.");
                return;
            }

            service.IsArchived = false; // Перемещаем услугу в архив
            _dbContext.SaveChanges();

            MessageBox.Show("Услуга перемещена в актив.");
            LoadServices(); // Перезагружаем список услуг
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
