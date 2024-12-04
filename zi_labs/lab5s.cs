using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Org.BouncyCastle.Crypto.Digests;

namespace zi_labs
{
    internal class lab5s
    {

		public enum VoteOption
		{
			v1,
			v2,
			v3,
			MaxValue
		}

		private BigInteger P, Q, Phi, C;
        public BigInteger N, D;
        private HashSet<string> NamesVoted = new HashSet<string>();
        private List<List<BigInteger>> VotesVoted = new List<List<BigInteger>>();
        public lab5s()
        {
            P = 0;
            while (lab1.check_prime(P) != true)
            {
                P = lab1.GenerateRandomBigInteger(lab1.PowBigInteger(2, 511), lab1.PowBigInteger(2, 512)-1, 64);
                Console.WriteLine(P);
            }
            Q = 0;
            while (lab1.check_prime(Q) != true)
            {
                Q = lab1.GenerateRandomBigInteger(lab1.PowBigInteger(2, 511), lab1.PowBigInteger(2, 512) - 1, 64);
                Console.WriteLine(Q);
            }
            Console.WriteLine(P + " " + Q);
            N = P * Q;
            Phi = (P - 1) * (Q - 1);
            Console.WriteLine(Phi);
            D = lab2.GenerateCoprime(Phi);
            C = Inverse(D, Phi);

        }

        public BigInteger GetBlank(string name, BigInteger hh)
        {
            if (NamesVoted.Contains(name))
            {
                Console.WriteLine("такой пользователь уже получил бюллетень");
                return new BigInteger(0);
            } else
            {
                Console.WriteLine("Пользователь " + name + " получил бюллетень");
				NamesVoted.Add(name);

			}
            return lab1.pow_module(hh, C, N);
        }

        public bool SetBlank(BigInteger n, BigInteger s)
        {
            BigInteger hash_10 = MySha(n);
            if (hash_10 == lab1.pow_module(s, D, N))
            {
                VotesVoted.Add(new List<BigInteger> { n, s });
                Console.WriteLine("Бюллетень принят");
                return true;
			} else
            {
                Console.WriteLine("бюллетень не бьыл принят");
                Console.WriteLine(lab1.pow_module(s, D, n) + " != " + hash_10);
                return false;
            }
		}

		public List<int> VotingResults()
		{
			// Создаем словарь для подсчета голосов
			Dictionary<VoteOption, int> votesDict = new Dictionary<VoteOption, int>();
			foreach (VoteOption option in Enum.GetValues(typeof(VoteOption)))
			{
				votesDict[option] = 0;
			}

			// Подсчитываем голоса
			foreach (var vote in VotesVoted)
			{
				BigInteger n = vote[0]; // Предположим, что первый элемент списка - это n
				int maskedN = (int)(n & (~((~0) << (int)Math.Log((int)VoteOption.MaxValue + 1))));
				VoteOption voteOption = (VoteOption)maskedN;
				votesDict[voteOption]++;
			}

			// Выводим результаты
            List<int> result = new List<int>();
			Console.WriteLine("[SERVER] Текущие итоги голосования:");
			foreach (var kvp in votesDict)
			{
				Console.WriteLine($"\t{kvp.Key} = {kvp.Value}");
                result.Add(kvp.Value);
			}
            return result;
		}
	


	public static BigInteger MySha(BigInteger n)
		{
			byte[] byteArray = n.ToByteArray();

			Sha3Digest sha3_512 = new Sha3Digest(512);

			byte[] hash = new byte[sha3_512.GetDigestSize()];
			sha3_512.BlockUpdate(byteArray, 0, byteArray.Length);
			sha3_512.DoFinal(hash, 0);

			string hash_16 = BitConverter.ToString(hash).Replace("-", "").ToLower();

			BigInteger hash_10 = BigInteger.Parse(hash_16, System.Globalization.NumberStyles.HexNumber);

			return hash_10;
		}

		public static BigInteger Inverse(BigInteger n, BigInteger p)
        {
            BigInteger inv = lab1.gcd_mod(n, p)[1];
            if (inv < 0)
            {
                inv += p;
            }
            return inv;
        }
        

    }
}
