using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zi_labs
{
    internal class lab1
    {
        public lab1() { }

        public ulong fast_exp(ulong val, ulong p)
        {
            if (p < 0)
            {
                ulong temp = (ulong)Math.Pow(val, p);
                return temp;

            }
            if (p == 0)
            {
                return 1;
            }
            if (p % 2 == 0)
            {
                return fast_exp(val * val, p / 2);
            }
            else
            {
                return val*fast_exp(val*val, (p-1)/2);
            }
            // https://habr.com/ru/companies/otus/articles/779396/
        }
        public List<ulong> eucl(ulong a, ulong b) 
        { 
        
            
            return new List<ulong>(); 
        
        }
    }
}
