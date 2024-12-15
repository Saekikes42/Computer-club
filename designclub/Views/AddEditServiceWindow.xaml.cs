using designclub.Models;
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
    /// Логика взаимодействия для AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        private readonly MyDbContext _dbContext;
        private readonly Minutepackage? _serviceToEdit;

        // Конструктор для добавления или редактирования услуги
        public AddEditServiceWindow(Minutepackage? serviceToEdit, MyDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _serviceToEdit = serviceToEdit;

            if (_serviceToEdit != null) // Если редактируем услугу
            {
                ServiceNameTextBox.Text = _serviceToEdit.Name;
                ServiceDescriptionTextBox.Text = _serviceToEdit.Description;
                ServiceMinutesTextBox.Text = _serviceToEdit.Minutes.ToString();
                ServiceCostTextBox.Text = _serviceToEdit.Cost.ToString();
                ServiceTypeComboBox.SelectedItem = ServiceTypeComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content?.ToString() == _serviceToEdit.Type?.ToString());
            }
        }

        // Обработчик для кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Считывание данных из полей ввода
                string name = ServiceNameTextBox.Text.Trim();
                string description = ServiceDescriptionTextBox.Text.Trim();
                int minutes;
                decimal cost;

                // Проверка: Поля не должны быть пустыми
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Название услуги не может быть пустым.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("Описание услуги не может быть пустым.");
                    return;
                }

                // Проверка: Минуты должны быть целым числом больше 0
                if (!int.TryParse(ServiceMinutesTextBox.Text, out minutes) || minutes <= 0)
                {
                    MessageBox.Show("Введите корректное количество минут (больше 0).");
                    return;
                }

                // Проверка: Стоимость должна быть положительным числом
                if (!decimal.TryParse(ServiceCostTextBox.Text, out cost) || cost <= 0)
                {
                    MessageBox.Show("Введите корректную стоимость (больше 0).");
                    return;
                }

                // Проверка: Выбран ли тип услуги
                if (ServiceTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите тип услуги.");
                    return;
                }

                // Получение выбранного значения типа услуги как строки
                string type = ((ComboBoxItem)ServiceTypeComboBox.SelectedItem).Content?.ToString() ?? string.Empty;

                // Сохранение услуги
                if (_serviceToEdit == null) // Если это новая услуга
                {
                    var newService = new Minutepackage
                    {
                        Name = name,
                        Description = description,
                        Minutes = minutes,
                        Cost = cost,
                        Type = type // Сохраняем строку вместо перечисления
                    };

                    _dbContext.Minutepackages.Add(newService);
                }
                else // Если это редактирование существующей услуги
                {
                    _serviceToEdit.Name = name;
                    _serviceToEdit.Description = description;
                    _serviceToEdit.Minutes = minutes;
                    _serviceToEdit.Cost = cost;
                    _serviceToEdit.Type = type; // Сохраняем строку вместо перечисления
                }

                // Сохранение изменений в базе
                _dbContext.SaveChanges();
                MessageBox.Show("Услуга сохранена!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Обработка непредвиденных ошибок
                MessageBox.Show($"Произошла ошибка при сохранении услуги: {ex.Message}");
            }
        }



        // Обработчик для кнопки "Отменить"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает окно без сохранения
        }
    }
}
