using System;
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
            //защита 1 лабы
            /*            Console.WriteLine("Функция быстрого возведения числа в степень по модулю:");
                        Random rnd = new Random();
                        ulong p = lab1.generate_prime(100000000, 1000000000);
                        ulong a = (ulong)rnd.Next(10000, 100000);
                        ulong x = (ulong)rnd.Next(0, (int)p);

                        ulong y = lab1.pow_module(a, x, p);
                        Console.WriteLine("a = " + a + " x = " + x + " p = " + p + "  Ответ: " + y);

                        var X = lab1.giant_baby_step(a, p, y);
                        Console.WriteLine(" X = " + X);

                        if(X != x)
                        {
                            ulong Y = lab1.pow_module(a, (ulong)X, p);
                            Console.WriteLine("Новое решение: " + Y);
                        }

                        ulong prime1 = lab1.generate_prime(1000000, 1000000000);
                        System.Threading.Thread.Sleep(50);
                        ulong prime2 = lab1.generate_prime(1000000, 1000000000);
                        System.Threading.Thread.Sleep(50);
                        ulong prime3 = lab1.generate_prime(1000000, 1000000000) + 1;
                        System.Threading.Thread.Sleep(50);
                        ulong prime4 = lab1.generate_prime(1000000, 1000000000) + 1;
                        System.Threading.Thread.Sleep(50);
                        List<ulong> primelist1 = lab1.gcd_mod(prime1, prime2);
                        List<ulong> primelist2 = lab1.gcd_mod(prime3, prime4);
                        Console.WriteLine("Евклид:");
                        Console.WriteLine("простые числа: " + primelist1[0]);
                        Console.WriteLine("четные числа: " + primelist2[0]);*/

            //подготовка второй лабы
            Console.WriteLine("Проверка Вернама: ");
            byte[] array1 = { 0x55, 0x33, 0xAA, 0x00, 0x01, 0x02, 0xFF };
            byte[] array2 = lab2.VernamEncode(array1);
            byte[] array3 = lab2.VernamDecode(array2);
            for( int i = 0; i < array1.Length; i++ )
            {
                Console.WriteLine(array1[i] + " " + array3[i]);
            }




            Console.WriteLine("Проверка RSA: ");
            ulong[] array5 = lab2.RSAEncode(array1);
            array3 = lab2.RSADecode(array5);
            for (int i = 0; i < array1.Length; i++)
            {
                Console.WriteLine(array1[i] + " " + array5[i] +  " " + array3[i]);
            }
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
            ulong l11o = lab1.pow_module(a, x, p);
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


            List<ulong> l12o = lab1.gcd_mod(a, x);
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
