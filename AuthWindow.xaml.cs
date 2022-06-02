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

namespace DemoEkzTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }
        DispatcherTimer dispatcherTimer;
        TimeSpan time;
        Random rnd;
        string capth = "";
        int i = 0;
        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            var user = Classes.Manager.GetContext().Роль.ToList();
            if(i < 2)
            {
                if(TBoxLogin.Text != "" && TBoxPass.Password != "")
                {
                    var curUser = user.Where(p => p.ID.ToString() == TBoxLogin.Text && p.Название == TBoxPass.Password).ToList();
                    if(curUser.Count > 0)
                    {
                        var userRole = curUser.FirstOrDefault();
                        MainWindow mainWin = new MainWindow();
                        mainWin.Show();
                    }
                    else
                    {
                        i++;
                        MessageBox.Show("Ошибка!");
                    }
                }
                else
                {
                    i++;
                    MessageBox.Show("Ошибка!");
                }
            }
            else
            {
                MessageBox.Show("Ошибка!");
                TitleWin.Height = 280;
                TBoxLogin.IsEnabled = false;
                TBoxPass.IsEnabled = false;
                BtnAuth.IsEnabled = false;
                StackCaptcha.Visibility = Visibility.Visible;
                rnd = new Random();
                capth = rnd.Next(0, 9999).ToString();
                TxtCaptcha.Text = capth;
            }
        }
        public int sec = 10;
        private void BtnCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if(TBoxCaptcha.Text == capth)
            {
                StackCaptcha.Visibility = Visibility.Hidden;
                TitleWin.Height = 200;
                TBoxLogin.IsEnabled = true;
                TBoxPass.IsEnabled = true;
                BtnAuth.IsEnabled = true;
            }
            else
            {
                TitleWin.Height = 250;
                StackTimer.Visibility = Visibility.Visible;
                StackCaptcha.Visibility = Visibility.Hidden;
                time = TimeSpan.FromSeconds(15);
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Start();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (sec == 0)
            {
                dispatcherTimer.Stop();
                i = 0;
                sec = 15;
                StackTimer.Visibility = Visibility.Hidden;
                TitleWin.Height = 200;
                TBoxLogin.IsEnabled = true;
                TBoxPass.IsEnabled = true;
                BtnAuth.IsEnabled = true;
            }
            else
            {
                sec--;
                LabelkTimer.Content = sec.ToString();
            }
        }
    }
}
