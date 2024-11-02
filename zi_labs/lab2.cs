using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace zi_labs
{
    internal class lab2
    {

        public static byte[] vernamKeys;
        public static byte[] vernamKeys2;
        public static List<BigInteger> RSAKeys;
        public static byte[] RSAKeys2;
        public static List<BigInteger> GamalKeys;
        public static byte[] GamalKeys2;
        public static List<BigInteger> ShamirKeys;
        public static byte[] ShamirKeys2;

        public static BigInteger GenerateCoprime(BigInteger p)
        {

            Random rnd = new Random();

            BigInteger result = lab1.GenerateRandomBigInteger(2, p);

            while (lab1.gcd(p, result) != 1)
            {
                result = lab1.GenerateRandomBigInteger(2, p);
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


        public static byte[] VernamEncodeUnsafe(byte[] data)
        {
            Random rnd = new Random();
            vernamKeys2 = new byte[data.Length];
            Console.WriteLine("шифрование :");
            for (int i = 0; i < data.Length; i++)
            {
                vernamKeys2[i] = (byte)rnd.Next(0, 255);
            }
            byte[] output = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeys2[i]);
            }
            return output;
        }

        public static byte[] VernamDecodeUnsafe(byte[] data)
        {
            byte[] output = new byte[data.Length];
            Console.WriteLine("дешифровка:");
            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeys2[i]);
            }
            return output;
        }


        public static byte[] VernamEncode(byte[] data)
        {
            Random rnd = new Random();
            byte[] vernamKeysLocal = new byte[data.Length];
            Console.WriteLine("шифрование :");

            for (int i = 0; i < data.Length; i++)
            {
                vernamKeysLocal[i] = (byte)rnd.Next(0, 255);
            }
            
            byte[] output = new byte[data.Length];
           
            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeysLocal[i]);
            }
            vernamKeys = VernamEncodeUnsafe(vernamKeysLocal);
            return output;
        }

        public static byte[] VernamDecode(byte[] data)
        {
            byte[] vernamKeysLocal = VernamDecodeUnsafe(vernamKeys);
            byte[] output = new byte[data.Length];
            Console.WriteLine("дешифровка:");
            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ vernamKeysLocal[i]);
            }
            return output;
        }


        public static BigInteger[] RSAEncode(byte[] data)
        {
            BigInteger[] output = new BigInteger[data.Length];

            BigInteger p = lab1.generate_prime(0, 1000000000);
            Console.WriteLine("P = " + p);
            BigInteger Q = lab1.generate_prime(0, 1000000000);
            Console.WriteLine("Q = " + Q);

            BigInteger N = p * Q;
            Console.WriteLine("N = " + N);

            BigInteger Phi = (p - 1) * (Q - 1);
            Console.WriteLine("Phi = " + Phi);

            BigInteger d = GenerateCoprime(Phi);
            Console.WriteLine("d = " + d);

            BigInteger c = lab1.gcd_mod(d, Phi)[1];
            if (c < 0)
            {
                c += Phi;
            }

            Console.WriteLine("c = " + c);


            RSAKeys = new List<BigInteger> { c, N };

            for(int i = 0; i<data.Length; i++)
            {

                BigInteger e = lab1.pow_module(data[i], d, N);

                output[i] = e;
            }
            return output;
        }

        public static byte[] RSADecode(BigInteger[] data)
        {
            byte[] output = new byte[data.Length];

            BigInteger c = RSAKeys[0];
            BigInteger N = RSAKeys[1];
            Console.WriteLine("c = " + c + " N = " + N);
            for(int i = 0; i < data.Length; i++)
            {
                BigInteger e = lab1.pow_module(data[i], c, N);
                output[i] = (byte)e;
            }


            return output;
        }


        public static BigInteger[] ElGamalEncode(byte[] data)
        {
            BigInteger[] output = new BigInteger[data.Length];
            BigInteger g = 0;
            BigInteger q;
            BigInteger p;
            while (true)
            {
                q = lab1.generate_prime(0, 1000000000);
                p = 2 * q + 1;
                if (lab1.check_prime(p))
                {
                    break;
                }
            }
            Console.WriteLine("p = " + p);
            while(lab1.pow_module(g, q, p) != 1)
            {
                g = lab1.GenerateRandomBigInteger(2, p - 1);
            }
            Console.WriteLine("g = " + g);
            BigInteger x = lab1.generate_prime(0, p - 1);
            Console.WriteLine("x = " + x);
            BigInteger y = lab1.pow_module(g, x, p);
            Console.WriteLine("y = " + y);

            BigInteger k = lab1.generate_prime(0, p - 2);
            Console.WriteLine("k = " + k);
            BigInteger a = lab1.pow_module(g, k, p);
            Console.WriteLine("a = " + a);

            GamalKeys = new List<BigInteger> { p, g, x, y, k, a };

            for(int i = 0; i < data.Length; i++)
            {
                output[i] = (data[i] * lab1.pow_module(y, k, p))%p;
            }


            return output;
        }

        public static byte[] ElGamalDecode(BigInteger[] data)
        {
            byte[] output = new byte[data.Length];

            BigInteger p = GamalKeys[0];
            BigInteger x = GamalKeys[2];
            BigInteger a = GamalKeys[5];

            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)((data[i] * lab1.pow_module(a, p-1-x, p)) % p);
            }

            return output;
        }

        public static BigInteger[] ShamirEncode(byte[] data)
        {
            BigInteger[] output = new BigInteger[data.Length];
            BigInteger p = lab1.generate_prime(0, 1000000000);

            BigInteger Ca = GenerateCoprime(p - 1);

            BigInteger Da = lab1.gcd_mod(p - 1, Ca)[2];
            if( Da < 0 )
            {
                Da += p - 1;
            }

            BigInteger Cb = GenerateCoprime(p-1);
            BigInteger Db = lab1.gcd_mod(p-1, Cb)[2];
            if(Db < 0)
            {
                Db += p - 1;
            }
            ShamirKeys = new List<BigInteger> { p, Ca, Da, Cb, Db };

            for (int i = 0; i < data.Length; i++)
            {
                BigInteger x1 = lab1.pow_module(data[i], Ca, p);
                BigInteger x2 = lab1.pow_module(x1, Cb, p);
                BigInteger x3 = lab1.pow_module(x2, Da, p);
                output[i] = x3;
            }

            return output;
        }

        public static byte[] ShamirDecode(BigInteger[] data)
        {
            byte[] output = new byte[data.Length];

            BigInteger p = ShamirKeys[0];
            BigInteger Db = ShamirKeys[4];



            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(lab1.pow_module(data[i],Db, p));
            }

            return output;
        }
        public static void WriteBytesToFile(string filePath, byte[] byteArray)
        {
            try
            {
                // Записываем массив байтов в файл
                File.WriteAllBytes(filePath, byteArray);
                Console.WriteLine("Файл успешно записан.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}");
            }
        }
        

    public static void WriteBigIntegersToFile(string filePath, BigInteger[] bigIntegers)
    {
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                foreach (BigInteger bigInt in bigIntegers)
                {
                    // Преобразуем BigInteger в массив байтов
                    byte[] byteArray = bigInt.ToByteArray();

                    // Записываем длину массива байтов (4 байта)
                    fs.Write(BitConverter.GetBytes(byteArray.Length), 0, 4);

                    // Записываем сам массив байтов
                    fs.Write(byteArray, 0, byteArray.Length);
                }
            }
            Console.WriteLine("Файл успешно записан.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи файла: {ex.Message}");
        }
    }


public static BigInteger[] ReadBigIntegersFromFile(string filePath)
    {
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                List<BigInteger> bigIntegers = new List<BigInteger>();

                while (fs.Position < fs.Length)
                {
                    // Читаем длину массива байтов (4 байта)
                    byte[] lengthBytes = new byte[4];
                    fs.Read(lengthBytes, 0, 4);
                    int length = BitConverter.ToInt32(lengthBytes, 0);

                    // Читаем сам массив байтов
                    byte[] byteArray = new byte[length];
                    fs.Read(byteArray, 0, length);

                    // Преобразуем массив байтов обратно в BigInteger
                    BigInteger bigInt = new BigInteger(byteArray);
                    bigIntegers.Add(bigInt);
                }

                return bigIntegers.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            return null;
        }
    }

} 
}
