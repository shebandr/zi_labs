using System;
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
        private static BigInteger P, Q, N, Phi, D, C;
        public static void ClosedDataGen()
        {
            P = 0;
            while (lab1.check_prime(P) != true && P > 0)
            {
                P = lab1.GenerateRandomBigInteger(lab1.PowBigInteger(10, 10), lab1.PowBigInteger(10, 1000));
                Console.WriteLine(P);
            }
            Q = 0;
            while (lab1.check_prime(Q) != true && Q > 0)
            {
                Q = lab1.GenerateRandomBigInteger(lab1.PowBigInteger(10, 10), lab1.PowBigInteger(10, 1000));
                Console.WriteLine(Q);
            }
            N = P * Q;
            Phi = (P - 1) * (Q - 1);
            D = lab2.GenerateCoprime(Phi);
            C = lab1.gcd_mod(D, Phi)[1];
            while (C < 0)
            {
                C += Phi;
            }
        }

        public static BigInteger MySha(BigInteger n)
        {
            byte[] byteArray = n.ToByteArray();

            Sha3Digest sha3_256 = new Sha3Digest(256);

            byte[] hash = new byte[sha3_256.GetDigestSize()];
            sha3_256.BlockUpdate(byteArray, 0, byteArray.Length);
            sha3_256.DoFinal(hash, 0);

            BigInteger result = new BigInteger(hash); 

            return result;
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
