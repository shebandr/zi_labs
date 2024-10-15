using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace zi_labs
{
    internal class lab2
    {

        public static byte[] vernamKeys;
        public static List<ulong> RSAKeys;

        public static ulong GenerateCoprime(ulong p)
        {

            Random rnd = new Random();

            ulong result = lab1.GenerateRandomUlong(2, p);

            while (lab1.gcd(p, result) != 1)
            {
                result = lab1.GenerateRandomUlong(2, p);
            }

            return result;
        }

        public static byte[] ReadFile(string fullPath)
        {



            byte[] fileBytes;

            try
            {
                fileBytes = File.ReadAllBytes(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
                return null;
            }

            return fileBytes;
        }

        public static byte[] VernamEncode(byte[] data)
        {
            Random rnd = new Random();
            vernamKeys = new byte[data.Length];
            Console.WriteLine("шифрование :");
            for (int i = 0; i < data.Length; i++)
            {
                vernamKeys[i] = (byte)rnd.Next(0, 255);
            }
            byte[] output = new byte[data.Length];
           
            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeys[i]);
                Console.WriteLine(data[i] + " " + vernamKeys[i] + " " + output[i]);
            }
            return output;
        }

        public static byte[] VernamDecode(byte[] data)
        {
            byte[] output = new byte[data.Length];
            Console.WriteLine("дешифровка:");
            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeys[i]);

                Console.WriteLine(output[i] + " " + vernamKeys[i] + " " + data[i]);
            }
            return output;
        }


        public static ulong[] RSAEncode(byte[] data)
        {
            ulong[] output = new ulong[data.Length];

            ulong p = lab1.generate_prime(0, 100); // все упирается в ulong, надо доставать bigInt
            Console.WriteLine("P = " + p);
            ulong Q = lab1.generate_prime(0, 100);
            Console.WriteLine("Q = " + Q);

            ulong N = p * Q;
            Console.WriteLine("N = " + N);

            ulong Phi = (p - 1) * (Q - 1);
            Console.WriteLine("Phi = " + Phi);

            ulong d = GenerateCoprime(Phi);
            Console.WriteLine("d = " + d);

            ulong c = lab1.gcd_mod(d, Phi)[1];
            if(c<0)
            {
                c += Phi;
            }

            Console.WriteLine("c = " + c);
            RSAKeys = new List<ulong> { c, N };

            for(int i = 0; i<data.Length; i++)
            {
                ulong e = lab1.pow_module(data[i], d, N);

                output[i] = e;
            }
            return output;
        }

        public static byte[] RSADecode(ulong[] data)
        {
            byte[] output = new byte[data.Length];

            ulong c = RSAKeys[0];
            ulong N = RSAKeys[1];
            Console.WriteLine("c = " + c + " N = " + N);
            for(int i = 0; i < data.Length; i++)
            {
                byte e = (byte)lab1.pow_module(data[i], c, N);
                output[i] = e;
            }


            return output;
        }

    } 
}
