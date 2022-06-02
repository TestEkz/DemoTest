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

namespace DemoEkzTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            var allManuf = Classes.Manager.GetContext().Производитель.ToList();
            allManuf.Insert(0, new Производитель
            {
                Название = "Все типы"
            });
            ComboManuf.ItemsSource = allManuf;
            ComboManuf.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;
            Update();
        }
        public void Update()
        {
            var curProd = Classes.Manager.GetContext().Товар.ToList();
            if (ComboManuf.SelectedIndex > 0)
                curProd = curProd.Where(p => p.Производитель.Название.Contains((ComboManuf.SelectedItem as Производитель).Название)).ToList();

            curProd = curProd.Where(p => p.Название.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            switch (ComboSort.SelectedIndex)
            {
                case 0:
                    LViewProd.ItemsSource = curProd;
                    break;
                case 1:
                    LViewProd.ItemsSource = curProd.OrderBy(p => p.Стоимость).ToList();
                    break;
                case 2:
                    LViewProd.ItemsSource = curProd.OrderByDescending(p => p.Стоимость).ToList();
                    break;
            }
            Classes.Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                Classes.Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProd.ItemsSource = Classes.Manager.GetContext().Товар.ToList();
            }
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ComboManuf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
    }
}
