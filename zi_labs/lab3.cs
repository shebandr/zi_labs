using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows;
using System.Security.AccessControl;

namespace zi_labs
{
    internal class lab3
    {
        public static List<BigInteger> GamalKeys;
        public static List<BigInteger> RSAKeys;
        public static List<BigInteger> GOSTKeys;
        public static List<BigInteger> ElGamalSing(byte[] data)
        {
            BigInteger q, p, g = 0;
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

            // Генерация примитивного корня g
            while (true)
            {
                g = lab1.GenerateRandomBigInteger(2, p - 1);
                if (lab1.pow_module(g, q, p) == 1)
                {
                    break;
                }
            }
            Console.WriteLine("g = " + g);

            BigInteger x = lab1.generate_prime(0, p - 1);
            Console.WriteLine("x = " + x);

            BigInteger y = lab1.pow_module(g, x, p);
            Console.WriteLine("y = " + y);

            BigInteger k = lab2.GenerateCoprime(p - 1);
            Console.WriteLine("k = " + k);

            BigInteger r = lab1.pow_module(g, k, p);
            Console.WriteLine("r = " + r);

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }

            string hashString = BitConverter.ToString(hash);
            Console.WriteLine("MD5 hash: " + hashString);

            Console.WriteLine("Цифровое представление хэша:");
            foreach (byte b in hash)
            {
                Console.WriteLine(b);
            }

            List<BigInteger> sign = new List<BigInteger>();
            foreach (byte b in hash)
            {
                BigInteger i = new BigInteger(b);
                List<BigInteger> gcdResult = lab1.gcd_mod(i, p - 1);
                BigInteger value = ((gcdResult[1] * i) % (p - 1));

                // Приведение результата по модулю к положительному значению
                if (value < 0)
                {
                    value += (p - 1);
                }

                sign.Add(value);
                Console.WriteLine(value);
            }

            GamalKeys = new List<BigInteger> { p, g, y, r };
            return sign;
        }

        public static bool ElGamalSingCheck(byte[] data, List<BigInteger> sign)
        {
            BigInteger p = GamalKeys[0];
            BigInteger g = GamalKeys[1];
            BigInteger y = GamalKeys[2];
            BigInteger r = GamalKeys[3];

            byte[] hash;
            List<BigInteger> signCalculated = new List<BigInteger>();

            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }
            foreach (byte b in hash)
            {
                signCalculated.Add(lab1.pow_module(g, b, p));
            }
            string hashString = BitConverter.ToString(hash);
            Console.WriteLine("MD5 hash: " + hashString);

            List<BigInteger> res = new List<BigInteger>();
            foreach (BigInteger i in sign)
            {
                BigInteger value = (lab1.pow_module(y, r, p) * lab1.pow_module(r, i, p)) % p;

                if (value < 0)
                {
                    value += p;
                }

                res.Add(value);
                Console.WriteLine(value + " " + res.Count);
            }

            Console.WriteLine("Проверка длин подписей: " + signCalculated.Count + " " + res.Count);
            for (int i = 0; i < sign.LongCount(); i++)
            {
                Console.WriteLine(res[i] + " == " + signCalculated[i]);
                if (res[i] != signCalculated[i])
                {
                    Console.WriteLine("Проблемы на " + i + " числе");
                    return false;
                }
            }

            return true;
        }

        public static List<BigInteger> RSASign(byte[] data)
        {
            BigInteger P = lab1.generate_prime(0, 1000000000);
            BigInteger Q = lab1.generate_prime(0, 1000000000);
            BigInteger N = P * Q;
            BigInteger Phi = (P-1) * (Q-1);
            Console.WriteLine("P = " + P + " Q = " + Q + " N = " + N + " Phi = " + Phi);

            BigInteger d = lab2.GenerateCoprime(Phi);
            BigInteger c = lab1.gcd_mod(d, Phi)[1];
            if(c<0)
            {
                c += Phi;
            }
            Console.WriteLine("d = " + d + " c = " + c);

            RSAKeys = new List<BigInteger> { N, d };
            Console.WriteLine("проверка длины исходных данных: " + data.Length);
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }
            Console.WriteLine("проверка длины хэша: " + hash.Length);
            Console.WriteLine("Цифровое представление хэша:");
            foreach (byte b in hash)
            {
                Console.Write(b + " ");
            }
            List<BigInteger> result = new List<BigInteger>();
            foreach(BigInteger i in hash)
            {
                result.Add(lab1.pow_module(i, c, N));
            }
            return result;
        }

        public static bool RSASignCheck(byte[] data, List<BigInteger> sign)
        {
            BigInteger N = RSAKeys[0];
            BigInteger d = RSAKeys[1];
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }
            List<BigInteger> calculatedSign = new List<BigInteger>();
            foreach (BigInteger i in sign)
            {
                calculatedSign.Add(lab1.pow_module(i, d, N));
            }

            for(int i =0;i<sign.Count; i++)
            {
                Console.WriteLine(hash[i] + " = " + calculatedSign[i]);
                if (hash[i] != calculatedSign[i])
                {
                    return false;
                }
            }
            return true;
        }


        public static BigInteger GOSTSign(byte[] data)
        {
            BigInteger q = lab1.GenerateRandomBigInteger(0, 1000000000);
            BigInteger b = lab1.GenerateRandomBigInteger(0, 1000000000);
            BigInteger p = 1; 
            while (!lab1.check_prime(q * b + 1))
            {
                b = lab1.GenerateRandomBigInteger(0, 1000000000);
                p = q * b + 1;
            }
            Console.WriteLine(" p = " + p + " q = " + q + " b = " + b);

            BigInteger g = lab1.GenerateRandomBigInteger(0, p - 1);
            BigInteger a = lab1.pow_module(g, b, p);
            while (a <= 1)
            {
                g = lab1.GenerateRandomBigInteger(1, p-1);
                a = lab1.pow_module(g, b, p);
            }
            BigInteger x = lab1.GenerateRandomBigInteger(1, q-1);
            BigInteger y = lab1.pow_module(a, x, p);
            Console.WriteLine("x = " + x + " y = " + y);
            BigInteger hash;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashArr = md5.ComputeHash(data);
                string hexHash = BitConverter.ToString(hashArr).Replace("-", "").ToLower();
                hash = BigInteger.Parse("0" + hexHash, System.Globalization.NumberStyles.HexNumber);
            }
            Console.WriteLine("хеш = " + hash);

            BigInteger r = 0, s = 0, k = 0;
            while (s == 0)
            {
                while (r == 0)
                {
                    k = lab1.GenerateRandomBigInteger(1, q - 1);
                    r = lab1.pow_module(a, k, p) % q;
                }
                s = (k * hash + x * r) % q;
            }
            GOSTKeys = new List<BigInteger> { q, a, y, p, r };
            return s;


        }
        public static bool GOSTSignCheck(byte[] data, BigInteger sign)
        {
            BigInteger q = GOSTKeys[0];
            BigInteger a = GOSTKeys[1];
            BigInteger y = GOSTKeys[2];
            BigInteger p = GOSTKeys[3];
            BigInteger r = GOSTKeys[4];

            BigInteger hash;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashArr = md5.ComputeHash(data);
                string hexHash = BitConverter.ToString(hashArr).Replace("-", "").ToLower();
                hash = BigInteger.Parse("0" + hexHash, System.Globalization.NumberStyles.HexNumber);
            }

            BigInteger temp = lab1.gcd_mod(hash, q)[1];

            if(temp < 1)
            {
                temp += q;
            }
            BigInteger u1 = (sign * temp) % q;
            BigInteger u2 = (-r * temp) % q;
            BigInteger v = ((lab1.pow_module(a, u1, p) * lab1.pow_module(y, u2, p)) % p) % q;
            Console.WriteLine(v + " == " + r);
            if(v != r)
            {
                return false;
            }
            return true;
        }
    }
}
