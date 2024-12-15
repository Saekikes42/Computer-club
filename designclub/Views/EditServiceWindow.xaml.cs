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
    /// Логика взаимодействия для EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : Window
    {
        private readonly MyDbContext _dbContext;
        private readonly Minutepackage? _serviceToEdit;

        public EditServiceWindow(Minutepackage? serviceToEdit, MyDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _serviceToEdit = serviceToEdit;

            // Инициализация полей с текущими данными услуги
            if (_serviceToEdit != null)
            {
                ServiceNameTextBox.Text = _serviceToEdit.Name;
                ServiceDescriptionTextBox.Text = _serviceToEdit.Description;
                ServiceMinutesTextBox.Text = _serviceToEdit.Minutes.ToString();
                ServiceCostTextBox.Text = _serviceToEdit.Cost.ToString();

                // Устанавливаем тип услуги в ComboBox
                ServiceTypeComboBox.SelectedItem = ServiceTypeComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == _serviceToEdit.Type);
            }
        }

        // Обработчик кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_serviceToEdit == null)
            {
                MessageBox.Show("Ошибка: Услуга для редактирования не найдена.");
                return;
            }

            try
            {
                // Проверка на пустое имя услуги
                if (string.IsNullOrWhiteSpace(ServiceNameTextBox.Text))
                {
                    MessageBox.Show("Ошибка: Имя услуги не может быть пустым.");
                    return;
                }

                // Проверка на пустое описание услуги
                if (string.IsNullOrWhiteSpace(ServiceDescriptionTextBox.Text))
                {
                    MessageBox.Show("Ошибка: Описание услуги не может быть пустым.");
                    return;
                }

                // Проверка на корректность минут (целое положительное число)
                if (!int.TryParse(ServiceMinutesTextBox.Text, out int minutes) || minutes <= 0)
                {
                    MessageBox.Show("Ошибка: Время услуги должно быть положительным числом.");
                    return;
                }

                // Проверка на корректность стоимости (декемальная сумма)
                if (!decimal.TryParse(ServiceCostTextBox.Text, out decimal cost) || cost <= 0)
                {
                    MessageBox.Show("Ошибка: Стоимость услуги должна быть положительным числом.");
                    return;
                }

                // Проверка на выбор типа услуги
                if (ServiceTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Ошибка: Не выбран тип услуги.");
                    return;
                }

                _serviceToEdit.Name = ServiceNameTextBox.Text;
                _serviceToEdit.Description = ServiceDescriptionTextBox.Text;
                _serviceToEdit.Minutes = minutes;
                _serviceToEdit.Cost = cost;
                _serviceToEdit.Type = ((ComboBoxItem)ServiceTypeComboBox.SelectedItem).Content.ToString();

                _dbContext.SaveChanges();

                MessageBox.Show("Изменения успешно сохранены!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
