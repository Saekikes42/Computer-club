using designclub.Models;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace designclub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Устанавливаем приложение в полноэкранный режим
            //this.WindowStyle = WindowStyle.None; // Без рамки и кнопок управления
            //this.WindowState = WindowState.Maximized; // Максимальный размер окна
            //this.Topmost = true; // Поверх всех окон
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            // Получаем позицию мыши относительно окна
            var mousePosition = e.GetPosition(this);

            // Рассчитываем центр окна
            double centerX = this.ActualWidth / 2;
            double centerY = this.ActualHeight / 2;

            // Рассчитываем смещение текста относительно центра, чтобы текст "двигался" при движении мыши
            double offsetX = (mousePosition.X - centerX) * -0.05; // Умножаем на коэффициент для уменьшения эффекта
            double offsetY = (mousePosition.Y - centerY) * -0.05;

            // Ограничиваем смещение
            offsetX = Math.Clamp(offsetX, -30, 30); // Ограничиваем по X в пределах -30 до 30 пикселей
            offsetY = Math.Clamp(offsetY, -30, 30); // Ограничиваем по Y в пределах -30 до 30 пикселей

            // Применяем смещение к тексту
            TextTransform.X = offsetX;
            TextTransform.Y = offsetY;
        }
        // Метод для открытия окна бронирования

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAuthWindow();
        }



        // Метод для открытия окна авторизации
        private void ShowAuthWindow()
        {
            DimLayer.Visibility = Visibility.Visible; // Включаем затемняющий слой
            AuthPopup.IsOpen = true; // Открываем окно авторизации
        }

        // Обработчик нажатия на кнопку "Войти" внутри окна авторизации
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            App.IsDemoMode = false;


            //string email = EmailTextBox.Text;

            //if (!IsValidEmail(email))
            //{
            //    // Если email некорректен, показываем сообщение об ошибке
            //    ShowError("Неправильный Email");
            //}
            //else
            //{
            //    // Логика успешной авторизации
            //    MessageBox.Show("Авторизация успешна!");
            //    CloseAuthWindow();
            //}
            if (string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                AuthPopup.IsOpen = false;
                MessageBox.Show("Логин и пароль не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return; // Прекращаем выполнение, если поля пустые
            }


            // Проверяем среди сотрудников
            var user = App.context.Users
                .FirstOrDefault(u => u.Login == Login.Text && u.Password == Password.Text);

            if (user != null)
            {
                AuthPopup.IsOpen = false;

                // Проверка роли пользователя

                MessageBox.Show("Добро пожаловать, " + user.FullName);
                AdminPanel adminPanel = new AdminPanel(user.FullName, user.Role); // Открываем панель администратора или сотрудника
                adminPanel.Show();


                this.Close();
                return;
            }

            // Проверяем среди клиентов
            var client = App.context.Clients
                .FirstOrDefault(c => c.UserClient == Login.Text && c.Pass == Password.Text);

            if (client != null)
            {
                AuthPopup.IsOpen = false;
                MessageBox.Show("Добро пожаловать, " + client.FullName);

                // Переходим на клиентский интерфейс
                ClientGame clientGame = new ClientGame(client.FullName, client.BonusBalance);
                clientGame.Show();
                this.Close();
            }
            else
            {
                AuthPopup.IsOpen = false;
                // Неверный логин или пароль
                MessageBox.Show("Неверный логин или пароль.");
            }




        }

        private void DemoAdminButton_Click(object sender, RoutedEventArgs e)
        {
            App.IsDemoMode = true;
            CloseAuthWindow();
            new AdminPanel("Демо-пользователь", "Администратор").Show();
            Close();
        }

        private void DemoClientButton_Click(object sender, RoutedEventArgs e)
        {
            App.IsDemoMode = true;
            CloseAuthWindow();
            new ClientGame(DemoData.Client.FullName, DemoData.Client.BonusBalance).Show();
            Close();
        }
        // Метод для проверки корректности email с помощью регулярного выражения
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Показ уведомления об ошибке с анимацией


        // Закрытие окна авторизации и затемняющего слоя
        private void CloseAuthWindow()
        {
            DimLayer.Visibility = Visibility.Collapsed; // Скрываем затемняющий слой
            AuthPopup.IsOpen = false; // Закрываем окно авторизации
        }

        // Закрытие всплывающего окна уведомления об ошибке
        private void CloseErrorPopup_Click(object sender, RoutedEventArgs e)
        {

        }

        // Закрытие окна авторизации при клике на затемняющий слой
        private void DimLayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseAuthWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Показываем окно с подтверждением
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Если пользователь нажал "Да"
            if (result == MessageBoxResult.Yes)
            {
                // Закрываем приложение
                Application.Current.Shutdown();
            }
        }
        //private void AnimatedButton_Click(object sender, RoutedEventArgs e)
        //{
        //    DoubleAnimation animation = new DoubleAnimation
        //    {
        //        From = 220,               // Начальная позиция по Y
        //        To = 300,                // Конечная позиция по Y
        //        Duration = TimeSpan.FromSeconds(0.4) // Длительность анимации
        //    };

        //    // Применяем анимацию к свойству Canvas.Top
        //    LoginButton.BeginAnimation(Canvas.TopProperty, animation);
        //    RegistorButton.BeginAnimation(Canvas.TopProperty, animation);

        //    DoubleAnimation animationtext = new DoubleAnimation
        //    {
        //        From = 120,               // Начальная позиция по Y
        //        To = 30,                // Конечная позиция по Y
        //        Duration = TimeSpan.FromSeconds(0.4) // Длительность анимации
        //    };

        //    // Применяем анимацию к свойству Canvas.Top
        //    AnimatedTextBlock.BeginAnimation(Canvas.TopProperty, animationtext);
        //}



    }
}
