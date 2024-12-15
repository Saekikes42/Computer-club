using designclub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace designclub
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private MyDbContext _dbContext;

        public AddClientWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        // Метод сохранения данных клиента
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Считывание значений из полей ввода
            string clientName = NameTextBox.Text;
            string clientPhone = PhoneTextBox.Text;
            string clientEmail = EmailTextBox.Text;
            DateOnly? clientBirthDate = BirthDatePicker.SelectedDate.HasValue
    ? DateOnly.FromDateTime(BirthDatePicker.SelectedDate.Value)
    : (DateOnly?)null;

            string clientBonusBalance = BonusBalanceTextBox.Text;
            string userClient = UserClientTextBox.Text.Trim();
            string pass = PassPasswordBox.Password.Trim();

            // Проверка на корректность ФИО
            if (!Regex.IsMatch(clientName, @"^[А-Яа-яЁё\s-]+$"))
            {
                MessageBox.Show("ФИО не должно содержать цифры, символы (кроме '-') и латинские буквы.");
                return;
            }

            // Проверка на корректность номера телефона (только цифры)
            if (!Regex.IsMatch(clientPhone, @"^\d+$"))
            {
                MessageBox.Show("Номер телефона должен содержать только цифры.");
                return;
            }

            // Проверка на уникальность номера телефона
            using (var dbContext = new MyDbContext())
            {
                var existingClient = dbContext.Clients.FirstOrDefault(c => c.Phone == clientPhone);
                if (existingClient != null)
                {
                    MessageBox.Show("Номер телефона должен быть уникальным.");
                    return;
                }
            }

            // Проверка на корректность email
            if (!Regex.IsMatch(clientEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введите корректный email.");
                return;
            }

            // Проверка на корректность даты рождения
            if (!clientBirthDate.HasValue)
            {
                MessageBox.Show("Дата рождения обязательна.");
                return;
            }

            // Ограничение по возрасту (например, возраст от 18 лет)
            var age = DateTime.Today.Year - clientBirthDate.Value.Year;
            if (clientBirthDate.Value > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            {
                MessageBox.Show("Клиент должен быть старше 18 лет.");
                return;
            }

            // Проверка на корректность баланса бонусов (только числа)
            if (!decimal.TryParse(clientBonusBalance, out decimal parsedBonusBalance))
            {
                MessageBox.Show("Баланс бонусов должен быть числом.");
                return;
            }
            if (_dbContext.Clients.Any(c => c.UserClient == userClient) || _dbContext.Users.Any(u => u.Login == userClient))
            {
                MessageBox.Show("Логин уже используется. Придумайте другой.");
                return;
            }

            // Проверка на пустой пароль
            if (string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return;
            }
            // Создание нового клиента
            var newClient = new Client
            {
                FullName = clientName,
                Phone = clientPhone,
                Email = clientEmail,
                BirthDate = clientBirthDate.Value, // Преобразуем DateOnly в DateTime
                BonusBalance = parsedBonusBalance,
                UserClient = userClient,
                Pass = pass,
                RoleClient = "client"
            };

            // Добавление нового клиента в базу данных
            using (var dbContext = new MyDbContext())
            {
                dbContext.Clients.Add(newClient);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Клиент успешно добавлен!");
            this.DialogResult = true;
            this.Close(); // Закрываем окно после сохранения
        }

        // Проверка корректности email
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Обработчик для кнопки "Отменить"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает окно без сохранения
        }

    }
}
