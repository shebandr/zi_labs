using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace zi_labs
{
    internal class lab4
    {
        public static Dictionary<string, string> Types = new Dictionary<string, string> { { "P", "♠" }, { "K", "♣" }, { "C", "♥" }, { "B", "♦" } };
        private static List<string> CardTypes = new List<string> { "P", "K", "C", "B" }; // "♠" = P, "♣" = K, "♥" = C, "♦" = B
        private static List<string> CardNumbers = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10", "V", "D", "K", "T" };
        private static List<string> CardsDeck = new List<string>();

        public static void CardsDeckGen()
        {
            foreach(string CT in CardTypes)
            {
                foreach(string CN in CardNumbers)
                {
                    CardsDeck.Add(CN+CT);
                }
            }
        }

        //а теперь время ехать кукухой
        public static List<List<string>> MentalPoker(int playersNum)
        {
            BigInteger q = 0;
            BigInteger p = 0;

            while (true)
            {
                q = lab1.generate_prime(0, 1000000000);
                p = 2 * q + 1;
                if (lab1.check_prime(p))
                {
                    break;
                }
            }

            List<BigInteger> C = new List<BigInteger>();
            List<BigInteger> D = new List<BigInteger>();

            for (int i = 0; i < playersNum; i++)
            {
                BigInteger CTemp = lab2.GenerateCoprime(p - 1);
                BigInteger DTemp = lab1.gcd_mod(CTemp, p - 1)[1];
                if (DTemp < 0)
                {
                    DTemp += p - 1;
                }
                C.Add(CTemp);
                D.Add(DTemp);
                Console.WriteLine("C = " + CTemp + "   D = " + DTemp);
            }

            Console.WriteLine("C:");
            foreach (BigInteger c in C)
            {
                Console.Write(c + " ");
            }
            Console.Write("\n");


            Console.WriteLine("D:");
            foreach (BigInteger c in D)
            {
                Console.Write(c + " ");
            }
            Console.Write("\n");



            CardsDeckGen();
            Console.Write("Список карт: ");
            foreach (string c in CardsDeck)
            {
                Console.Write(c + " ");
            }
            Console.Write("\n");

            int w = 0;
            List<BigInteger> deckKeys = new List<BigInteger>();
            Console.Write("Список индексов: ");
            foreach (string c in CardsDeck)
            {
                deckKeys.Add(w);
                Console.Write(w + " ");
                w++;
            }
            Console.Write("\n");


            List<BigInteger> TempDeck = new List<BigInteger>(deckKeys);
            deckKeys = Shuffle(TempDeck);

            foreach (int c in deckKeys)
            {

                Console.Write(c + " ");
            }
            Console.Write("\n");



            for (int i = 0; i < playersNum; i++)
            {
                List<BigInteger> newDeckKeys = new List<BigInteger>();
                for (BigInteger z = 0; z < deckKeys.Count; z++)
                {
                    BigInteger result = lab1.pow_module(deckKeys[(int)z], C[(int)i], p);
                    newDeckKeys.Add(result);
                }
                deckKeys = new List<BigInteger>(newDeckKeys);
                deckKeys = Shuffle(deckKeys);
                Console.WriteLine("Рандом заширофанных: ");
                foreach (int c in deckKeys)
                {

                    Console.Write(c + " ");
                }
            }
            List<List<BigInteger>> hands = new List<List<BigInteger>>();

            for(int qq = 0; qq < playersNum; qq++)
            {
                hands.Add(new List<BigInteger>());
                for (int cc = 0; cc< 2; cc++)
                {
                    BigInteger card = deckKeys[cc];
                    deckKeys.Remove(card);
                    hands[qq].Add(card);
                    Console.WriteLine("Игрок номер " + qq + " получил карту " + card);
                }
            }
            List<BigInteger> table = new List<BigInteger>();
            for(int qq = 0;qq < 5; qq++)
            {
                table.Add(deckKeys[deckKeys.Count-1]);
                deckKeys.Remove(table[qq]);
            }

            for(int qq = 0;qq < playersNum; qq++)
            {
                for (int j = 0; j < table.Count; j++)
                {
                    table[j] = lab1.pow_module(table[j], D[qq], p);
                }
            }
            Console.WriteLine(table.Count);
            Console.WriteLine("Декодированный стол: ");
            for(int qq = 0; qq<table.Count;qq++)
            {
                Console.Write(CardsDeck[(int)table[qq]] + " " );
            }

            int v = 0;
            for(int qq = 0; qq<playersNum; qq++)
            {
                for(int j = 0; j < playersNum; j++)
                {
                    if (qq != j)
                    {
                        v = 0;
                        while(v < hands[qq].Count)
                        {
                            hands[qq][v] = lab1.pow_module(hands[qq][v], D[j], p);
                            v += 1;
                        }
                    }
                }
                v = 0;
                while( v < hands[0].Count)
                {
                    hands[qq][v] = lab1.pow_module(hands[qq][v], D[qq], p);
                     v += 1;
                }
            }



            for (int qq = 0; qq < playersNum; qq++)
            {
                Console.WriteLine("\nИгрок номер " + qq);
                for(int j = 0; j < 2; j++){
                    Console.Write(CardsDeck[(int)hands[qq][j]] + " " );
                }
            }






            hands.Add(table);
            List<List<string>> allData = new List<List<string>>();
            foreach(var t in hands)
            {
                allData.Add(new List<string>());
                foreach(var d in t)
                {
                    allData[allData.Count - 1].Add(CardsDeck[(int)d]);
                }
            }
            return allData;


        }
        public static List<BigInteger> Shuffle(List<BigInteger> list)
        {
            list = new List<BigInteger> (list);
            List<BigInteger> deckKeys = new List<BigInteger>();

            while (true)
            {
                BigInteger i = lab1.GenerateRandomBigInteger(0, list.Count);
                deckKeys.Add(list[(int)i]);
                list.RemoveAt((int)i);
                if (list.Count == 0)
                {
                    break;
                }
            }
            return deckKeys;
        }
    }
}
