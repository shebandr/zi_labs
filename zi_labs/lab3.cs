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

            List<BigInteger> sign = new List<BigInteger>();
            foreach (byte b in hash)
            {
                BigInteger hashValue = new BigInteger(b);
                BigInteger s = (ModInverse(k, p - 1) * (hashValue - x * r)) % (p - 1);

                // Приведение результата по модулю к положительному значению
                if (s < 0)
                {
                    s += (p - 1);
                }

                sign.Add(s);
                Console.WriteLine(s);
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

            string hashString = BitConverter.ToString(hash);
            Console.WriteLine("MD5 hash: " + hashString);

            List<BigInteger> res = new List<BigInteger>();
            for (int i = 0; i < hash.Length; i++)
            {
                BigInteger b = new BigInteger(hash[i]);
                BigInteger s = sign[i];

                BigInteger v1 = (lab1.pow_module(y, r, p) * lab1.pow_module(r, s, p)) % p;
                BigInteger v2 = lab1.pow_module(g, b, p);

                if (v1 < 0)
                {
                    v1 += p;
                }

                if (v2 < 0)
                {
                    v2 += p;
                }

                res.Add(v1);
                signCalculated.Add(v2);
                Console.WriteLine(v1 + " == " + v2);
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

        public static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                // q является частным
                BigInteger q = a / m;
                BigInteger t = m;

                // m остаток от деления, процесс как в алгоритме Евклида
                m = a % m;
                a = t;
                t = y;

                // Обновляем x и y
                y = x - q * y;
                x = t;
            }

            // Делаем x положительным
            if (x < 0)
                x += m0;

            return x;
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
            BigInteger q, p, b, g, a, x, y, hash, r, s, k;

            // Генерация простых чисел p и q
            while (true)
            {
                q = lab1.generate_prime(0, 1000000000);
                b = lab1.GenerateRandomBigInteger(0, 1000000000);
                p = q * b + 1;
                if (lab1.check_prime(p))
                {
                    break;
                }
            }
            Console.WriteLine(" p = " + p + " q = " + q + " b = " + b);

            // Генерация a
            while (true)
            {
                g = lab1.GenerateRandomBigInteger(1, p - 1);
                a = lab1.pow_module(g, b, p);
                if (a > 1)
                {
                    break;
                }
            }

            // Генерация секретного ключа x и вычисление открытого ключа y
            x = lab1.GenerateRandomBigInteger(1, q - 1);
            y = lab1.pow_module(a, x, p);
            Console.WriteLine("x = " + x + " y = " + y);

            // Вычисление хеша
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashArr = md5.ComputeHash(data);
                string hexHash = BitConverter.ToString(hashArr).Replace("-", "").ToLower();
                hash = BigInteger.Parse("0" + hexHash, System.Globalization.NumberStyles.HexNumber);
            }
            Console.WriteLine("хеш = " + hash);

            // Генерация подписи
            while (true)
            {
                k = lab1.GenerateRandomBigInteger(1, q - 1);
                r = lab1.pow_module(a, k, p) % q;
                if (r != 0)
                {
                    s = (k * hash + x * r) % q;
                    if (s != 0)
                    {
                        break;
                    }
                }
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

            // Вычисление хеша
            BigInteger hash;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashArr = md5.ComputeHash(data);
                string hexHash = BitConverter.ToString(hashArr).Replace("-", "").ToLower();
                hash = BigInteger.Parse("0" + hexHash, System.Globalization.NumberStyles.HexNumber);
            }

            // Вычисление u1 и u2
            BigInteger hashInverse = ModInverse(hash, q);
            BigInteger u1 = (sign * hashInverse) % q;
            BigInteger u2 = (-r * hashInverse) % q;
            if (u2 < 0)
            {
                u2 += q;
            }

            // Вычисление v
            BigInteger v = ((lab1.pow_module(a, u1, p) * lab1.pow_module(y, u2, p)) % p) % q;
            Console.WriteLine(v + " == " + r);

            return v == r;
        }
    }
}
