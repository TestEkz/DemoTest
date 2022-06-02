using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZadaniePM01
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentProd = new Service();
        public string Fill;
        public AddEditPage(Service _selProd)
        {
            InitializeComponent();
            if(_selProd != null)
            {
                _currentProd = _selProd;
                Header.Text = "Редактирование услуги";
            }
            DataContext = _currentProd;
            
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentProd = Manager.GetContext().Service.ToList();
            currentProd = currentProd.Where(p => p.Title.ToLower().Contains(_currentProd.Title.ToLower())).ToList();

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentProd.Title))
                errors.AppendLine("Введите название!");
            if (_currentProd.Cost <= 0)
                errors.AppendLine("Введите корректную цену!");
            if (_currentProd.DurationInSeconds <=0)
                errors.AppendLine("Введите длительность услуги!");
            if (_currentProd.DurationInSeconds >= 240)
                errors.AppendLine("Длительность услуги не может быть больше 4 часов");
            if (_currentProd.Discount > 100 || _currentProd.Discount <= 0)
                errors.AppendLine("Скидка должна быть меньше 100 или больше 0");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if(_currentProd.ID == 0)
            {
                if (currentProd.Count() >= 1)
                {
                    MessageBox.Show("Такая услуги уже есть!");
                    return;
                }
                _currentProd.MainImagePath = Fill;
                Manager.GetContext().Service.Add(_currentProd);
            }
            try
            {
                    _currentProd.MainImagePath = Fill;
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Готово!");
                    Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG*.GIF;*.PNG|All files (*.*)|*.*";
            if(openDialog.ShowDialog() == true)
            {
                try
                {
                    Img.Source = new BitmapImage(new Uri(openDialog.FileName));
                    Fill = openDialog.FileName.ToString();
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытии!");
                }
            }
        }
    }
}
