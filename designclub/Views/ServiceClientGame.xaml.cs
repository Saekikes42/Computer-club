using designclub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
using System.Xml.Linq;

namespace designclub
{
    /// <summary>
    /// Логика взаимодействия для ServiceClientGame.xaml
    /// </summary>
    public partial class ServiceClientGame : Window
    {
        private readonly MyDbContext _dbContext; // Контекст базы данных
        private readonly Client _client = null!; // Клиент, которому начисляются бонусы

        // Конструктор
        public ServiceClientGame(string name, string balance)
        {


            InitializeComponent();

            balancewin.Text = balance;
            namewin.Text = name;

            this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            this.WindowState = WindowState.Maximized; // Максимальный размер окна
            this.Topmost = true; // Поверх всех окон
            StartClock();
            // Инициализация контекста базы данных
            _dbContext = new MyDbContext();

            if (App.IsDemoMode)
            {
                _client = DemoData.Client;
                ServicesDataGrid.ItemsSource = DemoData.Services;
                PayByCardButton.IsEnabled = false;
                PayWithBonusesButton.IsEnabled = false;
            }
            else
            {
                var client = _dbContext.Clients.FirstOrDefault(c =>
                    c.FullName.ToLower().Trim() == name.ToLower().Trim());

                if (client == null)
                {
                    MessageBox.Show("Клиент с таким именем не найден в базе данных!");
                    Close();
                    return;
                }

                _client = client;
                ServicesDataGrid.ItemsSource = _dbContext.Minutepackages
                    .Where(service => !service.IsArchived)
                    .ToList();
            }
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

        // Метод для обработки нажатия на кнопку "Оплатить"
        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную услугу из DataGrid
            var selectedService = ServicesDataGrid.SelectedItem as Minutepackage;

            if (selectedService != null)
            {
                // Считаем стоимость
                decimal totalCost = selectedService.Cost;

                // Пример логики для оплаты картой
                if (PayWithCard()) // Если оплата прошла успешно
                {
                    // Начисляем бонусы (30% от суммы)
                    decimal bonusAmount = totalCost * 0.30m;
                    _client.BonusBalance += bonusAmount;

                    // Создаем запись в статистике
                    var providedService = new Providedminutepackage
                    {
                        PackageId = selectedService.Id,
                        ClientId = _client.Id,
                        Date = DateTime.Now,
                        Cost = totalCost,

                        Status = "Completed",

                    };

                    // Добавляем запись в статистику
                    _dbContext.Providedminutepackages.Add(providedService);

                    // Сохраняем изменения в базе данных
                    _dbContext.SaveChanges();

                    // Обновляем баланс на UI
                    balancewin.Text = _client.BonusBalance.ToString("F2");

                    MessageBox.Show($"Оплата прошла успешно! Вам начислено {bonusAmount} бонусов.");
                }
                else
                {
                    MessageBox.Show("Ошибка при оплате картой.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу.");
            }
        }


        private void PayWithBonusesButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную услугу из DataGrid
            var selectedService = ServicesDataGrid.SelectedItem as Minutepackage;

            if (selectedService != null)
            {
                // Считаем стоимость
                decimal totalCost = selectedService.Cost;

                // Проверяем, есть ли у клиента достаточно бонусов для оплаты
                if (_client.BonusBalance >= totalCost)
                {
                    // Списываем бонусы с баланса клиента
                    _client.BonusBalance -= totalCost;

                    // Создаем запись в статистике
                    var providedService = new Providedminutepackage
                    {
                        PackageId = selectedService.Id,
                        ClientId = _client.Id,
                        Date = DateTime.Now,
                        Cost = totalCost,

                        Status = "Completed",

                    };

                    // Добавляем запись в статистику
                    _dbContext.Providedminutepackages.Add(providedService);

                    // Сохраняем изменения в базе данных
                    _dbContext.SaveChanges();

                    // Обновляем баланс на UI
                    balancewin.Text = _client.BonusBalance.ToString("F2");

                    MessageBox.Show($"Оплата прошла успешно! Вы заплатили {totalCost} бонусами.");
                }
                else
                {
                    MessageBox.Show("У вас недостаточно бонусов для оплаты этой услуги.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу.");
            }
        }


        // Метод для имитации оплаты картой
        private bool PayWithCard()
        {
            // Здесь будет логика для реальной оплаты картой.
            // Для примера, считаем, что оплата всегда проходит успешно.
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

}
