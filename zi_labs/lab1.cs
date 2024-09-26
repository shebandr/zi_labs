using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace zi_labs
{
    internal class lab1
    {
        public lab1() { }

        public static ulong fast_exp(ulong a, ulong x, ulong p)
        {
            ulong result = 1;
            a = a % p;

            if (a == 0)
            {
                return 0;
            }

            while (x > 0)
            {
                if ((x & 1) == 1)  // если крайний правый бит степени равен 1
                {
                    result = (result * a) % p;
                }

                a = (a * a) % p;
                x >>= 1;  // побитово смещаем степень
            }

            return result;
        }
        public static List<ulong> eucl(ulong a, ulong b)
        {
            List<ulong> U = new List<ulong> { a, 1, 0 };
            List<ulong> V = new List<ulong> { b, 0, 1 };
            while (V[0] != 0)
            {
                ulong q = U[0] / V[0];
                List<ulong> T = new List<ulong>{ U[0] % V[0], U[1] - q * V[1], U[2] - q * V[2]};
                U = V; 
                V = T;
            }

            return U; 
        }
        public static bool check_prime(ulong p)
        {
            Random rnd = new Random();

            if (p<=1) return false;
            else if (p==2) return true;
            ulong a = (ulong)rnd.Next(2, Convert.ToInt32(p) - 1);
            if(fast_exp(a, (p-1), p) != 1 || gcd(p, a) > 1){
                return false;
            } 
            return true;
        }

        public static ulong gcd(ulong a, ulong b)
        {
            while(b!=0)
            {
                ulong r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        public static ulong generate_prime(ulong left, ulong right)
        {
            Random rnd = new Random();
            while (true)
            {
                ulong p = (ulong)rnd.Next((int)left, (int)right);
                if(check_prime(p)) return p;
            }
        }

        public static List<ulong> diffie_hellman_algorithm()
        {
            Random rnd = new Random();
            ulong q = 0;
            ulong p = 0;
            ulong g = 0;
            ulong Xa = 0;
            ulong Xb = 0;
            ulong Ya = 0;
            ulong Yb = 0;
            ulong Zab = 0;
            ulong Zba = 0;
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
            Console.WriteLine("q = " + q);
            Console.WriteLine("p = " + p);
            while (fast_exp(g, q, p) != 1)
            {
                g = (ulong)rnd.Next(1, (int)(p -1));

            }
            Xa = (ulong)rnd.Next(1, (int)(p));
            Xb = (ulong)rnd.Next(1, (int)(p));
            Console.WriteLine("Закрытые ключи: ");
            Console.WriteLine("Xa = " + Xa);
            Console.WriteLine("Xb = " + Xb);

            Ya = fast_exp(g, Xa, p);
            Yb = fast_exp(g, Xb, p);
            Console.WriteLine("Открытые ключи: ");
            Console.WriteLine("Ya = " + Ya);
            Console.WriteLine("Yb = " + Yb);

            Zab = fast_exp(Yb, Xa, p);
            Zba = fast_exp(Ya, Xb, p);
            Console.WriteLine("Секретные ключи: ");
            Console.WriteLine("Zab = " + Zab);
            Console.WriteLine("Zba = " + Zba);
            return new List<ulong> { Xa, Xb, Ya, Yb, Zab, Zba };
        }

        public static ulong? giant_baby_step(ulong a, ulong p, ulong y)
        {
            ulong k = (ulong)Math.Ceiling(Math.Sqrt(p));
            ulong m = (ulong)Math.Ceiling(Math.Sqrt(p));

            Dictionary<ulong, ulong> baby = new Dictionary<ulong, ulong>();
            for (ulong j = 0; j < m; j++)
            {
                ulong value = (fast_exp(a, j, p) * y) % p;
                baby[value] = j;
            }
            Console.WriteLine(string.Join(", ", baby));

            List<ulong> giant = new List<ulong>();
            for (ulong i = 1; i <= k; i++)
            {
                giant.Add(fast_exp(a, m * i, p));
            }
            Console.WriteLine(string.Join(", ", giant));

            for (ulong i = 1; i <= k; i++)
            {
                if (baby.TryGetValue(giant[(int)i - 1], out ulong j))
                {
                    return i * m - j;
                }
            }

            return null;
        }

    }
}
