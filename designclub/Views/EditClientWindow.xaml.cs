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

namespace designclub
{
    /// <summary>
    /// Логика взаимодействия для EditClientWindow.xaml
    /// </summary>
    public partial class EditClientWindow : Window
    {
        private readonly MyDbContext _dbContext;  // Добавляем поле для контекста базы данных
        private readonly Client _clientToEdit;   // Для хранения клиента, который редактируется

        // Конструктор, принимающий клиента и контекст базы данных
        public EditClientWindow(Client client, MyDbContext dbContext)
        {
            InitializeComponent();

            _dbContext = dbContext;  // Инициализация контекста базы данных
            _clientToEdit = client;  // Инициализация клиента

            // Инициализация полей формы
            ClientNameTextBox.Text = client.FullName;
            ClientPhoneTextBox.Text = client.Phone;
            ClientBonusBalanceTextBox.Text = client.BonusBalance.ToString();


            // Инициализация логина и пароля
            ClientLoginTextBox.Text = client.UserClient;
            ClientPasswordBox.Text = client.Pass;
        }

        // Обработчик для кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string clientName = ClientNameTextBox.Text;
            string clientPhone = ClientPhoneTextBox.Text;
            string clientLogin = ClientLoginTextBox.Text.Trim();
            string clientPassword = ClientPasswordBox.Text.Trim();


            // Проверка уникальности логина среди клиентов и пользователей
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Login == clientLogin);
            var existingClient = _dbContext.Clients.FirstOrDefault(c => c.UserClient == clientLogin);

            if (existingUser != null || existingClient != null)
            {
                MessageBox.Show("Этот логин уже занят. Пожалуйста, выберите другой.");
                return; // Останавливаем сохранение, если логин уже существует
            }
            // Обновляем данные клиента
            _clientToEdit.FullName = clientName;
            _clientToEdit.Phone = clientPhone;

            _clientToEdit.BonusBalance = Convert.ToDecimal(ClientBonusBalanceTextBox.Text);
            _clientToEdit.UserClient = clientLogin;
            _clientToEdit.Pass = clientPassword;

            // Сохраняем изменения в базе данных
            _dbContext.SaveChanges();

            MessageBox.Show("Данные клиента сохранены!");
            this.DialogResult = true; // Закрываем окно и подтверждаем, что изменения были сохранены
            this.Close();
        }

        // Обработчик для кнопки "Отменить"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает окно без сохранения
        }


    }
}