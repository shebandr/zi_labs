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
using System.Numerics;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;


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
        private lab5s lab5S = new lab5s();
		public MainWindow()
        {
            InitializeComponent();
		}

		private void lab5Calc(object sender, RoutedEventArgs e)
		{

			lab5c lab5C = new lab5c(lab5S, lab5Name.Text);
			if (Vote1Option.IsChecked == true)
			{
				lab5SetError(lab5C.Vote(lab5s.VoteOption.v1));
			}
			if (Vote2Option.IsChecked == true)
			{
				lab5SetError(lab5C.Vote(lab5s.VoteOption.v2));
			}
			if (Vote3Option.IsChecked == true)
			{
				lab5SetError(lab5C.Vote(lab5s.VoteOption.v3));
			}



            List<int> VoteResult = lab5S.VotingResults();
			voteResult1Label.Content = VoteResult[0];
			voteResult2Label.Content = VoteResult[1];
			voteResult3Label.Content = VoteResult[2];
		}

		private void lab5SetError(int data)
        {
            switch(data)
			{
				case 0:
					lab5ErrorLabel.Content = "Бюллетень принят";
					return;
				case 1:
					lab5ErrorLabel.Content = "Бюллетень не принят";
					return;
				case 2:
					lab5ErrorLabel.Content = "Вы уже проголосовали";
					return;

			}
        }

		private void lab4Calc(object sender, RoutedEventArgs e)
        {
            int playerNum = Int32.Parse( lab4PlayersNum.Text);
            List<List<string>> cards = lab4.MentalPoker(playerNum);
            Console.Write("\n");
            for(int i = 0; i<cards.Count;i++)
            {
                Console.WriteLine("x");
                for(int j = 0; j < cards[i].Count; j++)
                {
                    Console.Write(cards[i][j] + " ");
                    char TempType = cards[i][j][cards[i][j].Length - 1];
                    cards[i][j] = cards[i][j].Remove(cards[i][j].Length - 1);
                    cards[i][j] += lab4.Types[TempType.ToString()];
                }
            }



            StackPanel lab4Table = (StackPanel)FindName("lab4Table");
            lab4Table.Children.Clear();

            foreach(var c in cards[cards.Count-1])
            {
                Border border = new Border
                {
                    Width = 40,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black
                };

                StackPanel stackPanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label label1 = new Label();
                Label label2 = new Label();
                if (c[c.Length - 1] == '♥' || c[c.Length - 1] == '♦')
                {
                     label1 = new Label
                    {
                        Content = c[c.Length - 1],
                        Foreground = Brushes.Red
                    };
                     label2 = new Label
                    {
                        Content = c.Substring(0, c.Length - 1),
                        Foreground = Brushes.Red
                    };
                } else
                {
                     label1 = new Label
                    {
                        Content = c[c.Length - 1],
                        Foreground = Brushes.Black
                    };
                     label2 = new Label
                    {
                        Content = c.Substring(0, c.Length - 1),
                        Foreground = Brushes.Black
                    };
                }
                stackPanel.Children.Add(label1);
                stackPanel.Children.Add(label2);
                border.Child = stackPanel;
                lab4Table.Children.Add(border);

            }



            StackPanel lab4Hands = (StackPanel)FindName("lab4Hands");
            lab4Hands.Children.Clear();

            foreach (var hand in cards)
            {
                if (hand.Count != 2)
                {
                    break;
                }
                Border borderHand = new Border
                {
                    Width = 90,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(3),
                    BorderBrush = Brushes.Black
                };
                StackPanel stackPanelCard = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Orientation = Orientation.Horizontal
                };
                foreach (var c in hand)
                {


                    Border border = new Border
                    {
                        Width = 40,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Background = Brushes.LightGray,
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black
                    };

                    StackPanel stackPanel = new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Orientation = Orientation.Vertical
                    };

                    Label label1 = new Label();
                    Label label2 = new Label();
                    if (c[c.Length - 1] == '♥' || c[c.Length - 1] == '♦')
                    {
                        label1 = new Label
                        {
                            Content = c[c.Length - 1],
                            Foreground = Brushes.Red
                        };
                        label2 = new Label
                        {
                            Content = c.Substring(0, c.Length - 1),
                            Foreground = Brushes.Red
                        };
                    }
                    else
                    {
                        label1 = new Label
                        {
                            Content = c[c.Length - 1],
                            Foreground = Brushes.Black
                        };
                        label2 = new Label
                        {
                            Content = c.Substring(0, c.Length - 1),
                            Foreground = Brushes.Black
                        };
                    }
                    stackPanel.Children.Add(label1);
                    stackPanel.Children.Add(label2);
                    border.Child = stackPanel;

                    stackPanelCard.Children.Add(border);

                }
                borderHand.Child = stackPanelCard;
                lab4Hands.Children.Add(borderHand);
            }

        }

        private void lab1Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 800;
            lab2Grid.Height = 0;
            lab3Grid.Height = 0;
			lab4Grid.Height = 0;
			lab5Grid.Height = 0;

		}
        private void lab2Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 800;
            lab3Grid.Height = 0;
            lab4Grid.Height = 0;
			lab5Grid.Height = 0;

		}
        private void lab3Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 0;
            lab3Grid.Height = 800;
            lab4Grid.Height = 0;
			lab5Grid.Height = 0;

		}
        private void lab4Start(object sender, RoutedEventArgs e)
        {
            lab1Grid.Height = 0;
            lab2Grid.Height = 0;
            lab3Grid.Height = 0;
			lab4Grid.Height = 800;
			lab5Grid.Height = 0;

        }
		private void lab5Start(object sender, RoutedEventArgs e)
		{
			lab1Grid.Height = 0;
			lab2Grid.Height = 0;
			lab3Grid.Height = 0;
			lab4Grid.Height = 0;
			lab5Grid.Height = 800;
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}


	}
}
