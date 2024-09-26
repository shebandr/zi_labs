﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Функция быстрого возведения числа в степень по модулю:");
            Console.WriteLine(lab1.fast_exp(4, 2, 3));
            Console.WriteLine("обобщённый алгоритм Евклида:");
            List<ulong> t = lab1.eucl(10, 200);
            Console.WriteLine(t[0].ToString() + " " + t[1].ToString() + " " + t[2].ToString());
            Console.WriteLine("Диффи-Хеллман:");
            lab1.diffie_hellman_algorithm();
            Console.WriteLine("Шаг младенца Шаг великана");
            Console.WriteLine(lab1.giant_baby_step(88, 107, 47));
        }

        private void button_click_l11(object sender, RoutedEventArgs e)
        {
            ulong a;
            try
            {
                UInt64.Parse(l11a.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;
            }
            a = UInt64.Parse(l11a.Text);
            ulong x;
            try
            {
                UInt64.Parse(l11x.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;

            }
            x = UInt64.Parse(l11x.Text);

            ulong p;
            try
            {
                UInt64.Parse(l11p.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;

            }
            p = UInt64.Parse(l11p.Text);
            ulong l11o = lab1.fast_exp(a, x, p);
            l11out.Text = l11o.ToString();


        }

        private void button_click_l12(object sender, RoutedEventArgs e)
        {
            ulong a;
            try
            {
                UInt64.Parse(l12a.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l12out.Text = "некорректные данные";
                return;
            }
            a = UInt64.Parse(l12a.Text);
            ulong x;
            try
            {
                UInt64.Parse(l12x.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l12out.Text = "некорректные данные";
                return;

            }
            x = UInt64.Parse(l12x.Text);


            List<ulong> l12o = lab1.eucl(a, x);
            l12out.Text = "= " + l12o[0].ToString() + "  k1: " + l12o[1].ToString() + "  l2: " + l12o[2].ToString();


        }
        private void button_click_l13(object sender, RoutedEventArgs e)
        {
            List<ulong> L = lab1.diffie_hellman_algorithm();
            l13out1.Text = "Закрытые ключи a: " + L[0] + "  b: " + L[1];
            l13out2.Text = "Открытые ключи Ya: " + L[2] + "  Yb: " + L[3];
            l13out3.Text = "Секретные ключи Zab: " + L[4] + "  Zba: " + L[5];


        }


        private void button_click_l14(object sender, RoutedEventArgs e)
        {
            ulong a;
            try
            {
                UInt64.Parse(l14a.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;
            }
            a = UInt64.Parse(l14a.Text);
            ulong x;
            try
            {
                UInt64.Parse(l14x.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;

            }
            x = UInt64.Parse(l14x.Text);

            ulong p;
            try
            {
                UInt64.Parse(l14p.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("невозможно перевести");
                l11out.Text = "некорректные данные";
                return;

            }
            p = UInt64.Parse(l14p.Text);
                if (lab1.giant_baby_step(a, x, p) != null) 
            {
                ulong l14o = (ulong)lab1.giant_baby_step(a, x, p);
                l14out.Text = l14o.ToString();

            }
            else
            {
                l14out.Text = "Нет решения";
            }


        }

    }
}
