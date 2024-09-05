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

namespace zi_labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        lab1 Lab1 = new lab1();
        public MainWindow()
        {
            InitializeComponent();



        }

        private void button_click_l11(object sender, RoutedEventArgs e)
        {
            ulong val;
            try
            {
                UInt64.Parse(l11val.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Content = "некорректные данные";
                return;
                //   MessageBox.Show("Неправильный идентификатор Лифтового блока");
            }
            val = UInt64.Parse(l11val.Text);
            ulong p;
            try
            {
                UInt64.Parse(l11p.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Content = "некорректные данные";
                return;
                //   MessageBox.Show("Неправильный идентификатор Лифтового блока");
            }
            p = UInt64.Parse(l11p.Text);

            l11out.Content = Lab1.fast_exp(val, p).ToString();


        }
    }
}
