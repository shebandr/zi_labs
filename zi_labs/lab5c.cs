using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace zi_labs
{
    internal class lab5c
    {
        private lab5s Lab5S = new lab5s();
        private string Name = "";
        public lab5c(lab5s l5, string name)
        {
            Lab5S = l5;
            Name = name;
        }

        public int Vote(lab5s.VoteOption vote)
        {
            BigInteger rnd = lab1.generate_prime(lab1.PowBigInteger(2, 511), lab1.PowBigInteger(2, 512) - 1);
			BigInteger shiftedRnd = rnd << 512;

			// Объединяем сдвинутое значение rnd с vote.value
			BigInteger n = shiftedRnd | (int)vote;
            BigInteger r = lab2.GenerateCoprime(Lab5S.N);
		    
            BigInteger hash_10 = lab5s.MySha(n);
            BigInteger hh = hash_10 * lab1.pow_module(r, Lab5S.D, Lab5S.N) % Lab5S.N;

            BigInteger ss = Lab5S.GetBlank(Name, hh);
            if(ss != 0)
            {
                BigInteger s = ss * lab5s.Inverse(r, Lab5S.N) % Lab5S.N;
                if(Lab5S.SetBlank(n, s))
                {
                    Console.WriteLine("Бюллетень принят");
                    return 1;
                } else
                {
                    Console.WriteLine("Бюллетень не принят");
                    return 2;
				}
            } else
            {
                Console.WriteLine("Вы уже проголосовали");
                return 3;

			}


		}
    }
}
