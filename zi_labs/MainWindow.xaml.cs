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
using System.Numerics;
using Microsoft.Win32;


namespace zi_labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string lab2FilePath;
        private List<BigInteger> currentSign;
        private BigInteger currentSignBigInteger;
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
            /*            Console.WriteLine("Проверка Вернама: ");
                        byte[] array1 = { 0x55, 0x33, 0xAA, 0x00, 0x01, 0x02, 0xFF };
                        byte[] array2 = lab2.VernamEncode(array1);
                        byte[] array3 = lab2.VernamDecode(array2);
                        for( int i = 0; i < array1.Length; i++ )
                        {
                            Console.WriteLine(array1[i] + " " + array3[i]);
                        }




                        Console.WriteLine("Проверка RSA: ");
                        BigInteger[] array5 = lab2.RSAEncode(array1);
                        array3 = lab2.RSADecode(array5);
                        for (int i = 0; i < array1.Length; i++)
                        {
                            Console.WriteLine(array1[i] + " " + array5[i] +  " " + array3[i]);
                        }

                        Console.WriteLine("Проверка степени: " + lab1.pow_module(85, 82056381197643673, 471863054612645153));*/

            /*            Console.WriteLine("Проверка Эл Гамала: ");
                        byte[] array1 = { 0x55, 0x33, 0xAA, 0x00, 0x01, 0x02, 0xFF };
                        BigInteger[] array2 = lab2.ElGamalEncode(array1);
                        byte[] array3 = lab2.ElGamalDecode(array2);
                        for (int i = 0; i < array1.Length; i++)
                        {
                            Console.WriteLine(array1[i] + "  " + array2[i] + " " + array3[i]);
                        }*/


/*            Console.WriteLine("Проверка Шамира: ");
            byte[] array1 = { 0x55, 0x33, 0xAA, 0x00, 0x01, 0x02, 0xFF };
            BigInteger[] array2 = lab2.ShamirEncode(array1);
            byte[] array3 = lab2.ShamirDecode(array2);
            for (int i = 0; i < array1.Length; i++)
            {
                Console.WriteLine(array1[i] + "  " + array2[i] + " " + array3[i]);
            }*/

        }


        private void lab1Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 800;
            lab2Grid.Height = 0;
            lab3Grid.Height = 0;
            lab4Grid.Height = 0;

        }
        private void lab2Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 800;
            lab3Grid.Height = 0;
            lab4Grid.Height = 0;

        }
        private void lab3Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 0;
            lab3Grid.Height = 800;
            lab4Grid.Height = 0;

        }
        private void lab4Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 0;
            lab3Grid.Height = 0;
            lab4Grid.Height = 800;

        }

        private void fileOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            

            if (openFileDialog.ShowDialog() == true)
            {
                lab2FilePath = openFileDialog.FileName;
            }
            Console.WriteLine(lab2FilePath);
        }

        private void Sign(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            Console.WriteLine(senderButton.Tag);

            byte[] fileBuffer = lab2.ReadFile(lab2FilePath);
            Console.WriteLine(fileBuffer.Length);


            switch (senderButton.Tag)
            {
                case "0":
                    currentSign = lab3.ElGamalSing(fileBuffer);
                    break;
                case "1":
                    currentSign = lab3.RSASign(fileBuffer);
                    break;
                case "2":
                    currentSignBigInteger = lab3.GOSTSign(fileBuffer);
                    break;

            }
        }


        private void Unsign(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            Console.WriteLine(senderButton.Tag);

            byte[] fileBuffer = lab2.ReadFile(lab2FilePath);
            Console.WriteLine(fileBuffer.Length);
            bool signCheck = false;


            switch (senderButton.Tag)
            {
                case "0":
                    signCheck = lab3.ElGamalSingCheck(fileBuffer, currentSign);
                    l3Output.Content = signCheck;
                    break;
                case "1":
                    signCheck = lab3.RSASignCheck(fileBuffer, currentSign);
                    l3Output.Content = signCheck;
                    break;
                case "2":
                    signCheck = lab3.GOSTSignCheck(fileBuffer, currentSignBigInteger);
                    l3Output.Content = signCheck;
                    break;


            }
            Console.WriteLine("итоговая проверка подписи: " + signCheck);
        }


        private void Encode(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            Console.WriteLine(senderButton.Tag);

            byte[] fileBuffer = lab2.ReadFile(lab2FilePath);
            Console.WriteLine(fileBuffer.Length);
            BigInteger[] encodedFileBuffer = { };
            byte[] outputFileBuffer = { };
            byte[] encodedFileBufferByte = { };

            string[] outputNameArr = lab2FilePath.Split('.');
            switch (senderButton.Tag)
            {
                case "0":
                    encodedFileBuffer = lab2.ShamirEncode(fileBuffer);
                    //вызов Шамира
                    break;
                case "1":
                    encodedFileBuffer = lab2.ElGamalEncode(fileBuffer);
                    //вызов Эль-Гамаля
                    break;
                case "2":
                    encodedFileBufferByte = lab2.VernamEncode(fileBuffer);
                    //вызов Вернама
                    break;
                case "3":
                    encodedFileBuffer = lab2.RSAEncode(fileBuffer);
                    //вызов RSA
                    break;
            }



            string lab2FilePathOutput = outputNameArr[0] + "2." + outputNameArr[1];
            if(senderButton.Tag.ToString() != "2")
            {
                lab2.WriteBigIntegersToFile(outputNameArr[0] + ".Encoded", encodedFileBuffer);

            } else
            {

                Console.WriteLine("Проверка гойды: " + encodedFileBufferByte.Length + " " + outputNameArr[0]);
                lab2.WriteBytesToFile(outputNameArr[0] + ".Encoded", encodedFileBufferByte);
            }
            encodedFileBuffer = null;

            encodedFileBuffer = lab2.ReadBigIntegersFromFile(outputNameArr[0] + ".Encoded");

            switch (senderButton.Tag)
            {
                case "0":
                    outputFileBuffer = lab2.ShamirDecode(encodedFileBuffer);

                    //вызов Шамира
                    break;
                case "1":
                    outputFileBuffer = lab2.ElGamalDecode(encodedFileBuffer);
                    //вызов Эль-Гамаля
                    break;
                case "2":
                    outputFileBuffer = lab2.VernamDecode(encodedFileBufferByte);
                    //вызов Вернама
                    break;
                case "3":
                    outputFileBuffer = lab2.RSADecode(encodedFileBuffer);
                    //вызов RSA
                    break;
            }
/*            Console.WriteLine("Длина: " + fileBuffer.Length + " " + encodedFileBuffer.Length + " " + outputFileBuffer.Length);*/
            
            lab2.WriteBytesToFile(lab2FilePathOutput, outputFileBuffer);
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
            BigInteger l11o = lab1.pow_module(a, x, p);
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


            List<BigInteger> l12o = lab1.gcd_mod(a, x);
            l12out.Text = "= " + l12o[0].ToString() + "  k1: " + l12o[1].ToString() + "  l2: " + l12o[2].ToString();


        }
        private void button_click_l13(object sender, RoutedEventArgs e)
        {
            List<BigInteger> L = lab1.diffie_hellman_algorithm();
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
