using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Numerics;


namespace zi_labs
{
    internal class lab1
    {
        public lab1() { }

        public static BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max) // этот костыль не может генерировать числа больше 10^18
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            if (min >= max)
            {
                throw new ArgumentException("Минимальное значение должно быть меньше максимального.");
            }

            BigInteger range = max - min;
            byte[] buffer = new byte[sizeof(ulong)];
            rnd.NextBytes(buffer);
            BigInteger randomValue = BitConverter.ToUInt64(buffer, 0);

            return (randomValue % range) + min;
        }

        public static BigInteger pow_module(BigInteger bigA, BigInteger bigX, BigInteger bigP)
        {

            BigInteger result = BigInteger.One;
            bigA = bigA % bigP;

            if (bigA == 0)
            {
                return 0;
            }

            while (bigX > 0)
            {
                if ((bigX & 1) == 1)
                {
                    result = (result * bigA) % bigP;
                }

                bigA = (bigA * bigA) % bigP;
                bigX >>= 1;
            }

            return (BigInteger)result;
        }

        public static List<BigInteger> gcd_mod(BigInteger a, BigInteger b)
        {
            List<BigInteger> U = new List<BigInteger> { a, 1, 0 };
            List<BigInteger> V = new List<BigInteger> { b, 0, 1 };
            while (V[0] != 0)
            {
                BigInteger q = U[0] / V[0];
                List<BigInteger> T = new List<BigInteger>{ U[0] % V[0], U[1] - q * V[1], U[2] - q * V[2]};
                U = V; 
                V = T;
            }

            return U; 
        }
        public static bool check_prime(BigInteger p)
        {
            Random rnd = new Random();

            if (p<=1) return false;
            else if (p==2) return true;
            BigInteger a = GenerateRandomBigInteger(2, p-1);
            if(pow_module(a, (p-1), p) != 1 || gcd(p, a) > 1){
                return false;
            } 
            return true;
        }

        public static BigInteger gcd(BigInteger a, BigInteger b)
        {
            while(b!=0)
            {
                BigInteger r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        public static BigInteger generate_prime(BigInteger left, BigInteger right)
        {
            while (true)
            {
                BigInteger p = GenerateRandomBigInteger(left, right);
                if(check_prime(p)) return p;
            }
        }

        public static List<BigInteger> diffie_hellman_algorithm()
        {
            Random rnd = new Random();
            BigInteger q = 0;
            BigInteger p = 0;
            BigInteger Xa = 0;
            BigInteger Xb = 0;
            BigInteger Ya = 0;
            BigInteger Yb = 0;
            BigInteger Zab = 0;
            BigInteger Zba = 0;
            Console.WriteLine("Общие параметры: ");
            while(true)
            {
                q = generate_prime(0, 1000000000);
                p = 2 * q + 1;
                if (check_prime(p))
                {
                    break;
                } else
                {
                }

            }
            BigInteger g = (BigInteger)rnd.Next(1, (int)p-1);

            Console.WriteLine("q = " + q);
            Console.WriteLine("p = " + p);
            while (pow_module(g, q, p) == 1)
            {
                g = (BigInteger)rnd.Next(1, (int)(p -1));

            }
            Console.WriteLine("g = " + g);
            Xa = (BigInteger)rnd.Next(1, (int)(p));
            Xb = (BigInteger)rnd.Next(1, (int)(p));
            Console.WriteLine("Закрытые ключи: ");
            Console.WriteLine("Xa = " + Xa);
            Console.WriteLine("Xb = " + Xb);

            Ya = pow_module(g, Xa, p);
            Yb = pow_module(g, Xb, p);
            Console.WriteLine("Открытые ключи: ");
            Console.WriteLine("Ya = " + Ya);
            Console.WriteLine("Yb = " + Yb);

            Zab = pow_module(Yb, Xa, p);
            Zba = pow_module(Ya, Xb, p);

            return new List<BigInteger> { Xa, Xb, Ya, Yb, Zab, Zba };
        }

        public static BigInteger? giant_baby_step(BigInteger a, BigInteger p, BigInteger y)
        {
            BigInteger k = (BigInteger)Math.Ceiling(Math.Sqrt((double)p));
            BigInteger m = (BigInteger)Math.Ceiling(Math.Sqrt((double)p));

            Dictionary<BigInteger, BigInteger> baby = new Dictionary<BigInteger, BigInteger>();
            for (BigInteger j = 0; j < m; j++)
            {
                BigInteger value = (pow_module(a, j, p) * y) % p;
                baby[value] = j;
            }
            /*Console.WriteLine(string.Join(", ", baby));*/

            List<BigInteger> giant = new List<BigInteger>();
            for (BigInteger i = 1; i <= k; i++)
            {
                giant.Add(pow_module(a, m * i, p));
            }
            /*Console.WriteLine(string.Join(", ", giant));*/

            for (BigInteger i = 1; i <= k; i++)
            {
                if (baby.TryGetValue(giant[(int)i - 1], out BigInteger j))
                {
                    return i * m - j;
                }
            }

            return null;
        }

    }
}
