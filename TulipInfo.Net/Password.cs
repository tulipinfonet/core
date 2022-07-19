using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public static class Password
    {
        public static string Generate(int size=8,
            bool mustContainCapital = true, 
            bool mustContainLowercase = true,
            bool mustContainNum=true, 
            bool mustContainSymbol=true)
        {
            StringBuilder sb = new StringBuilder();
            if (mustContainCapital)
            {
                sb.Append(CreateBigAbc());
            }
            if(mustContainLowercase)
            {
                sb.Append(CreateSmallAbc());
            }
            if (mustContainNum)
            {
                sb.Append(CreateNum());
            }
            if (mustContainSymbol)
            {
                sb.Append(CreateSymbol());
            }
            int currentLength = sb.Length;
            const int minvalue = 0;
            const int maxvalue = 4;
            int tmp=0, tmp1 = 0;
            Random random = new Random();
            for (int i = 0; i < size- currentLength; i++)
            {
                if (i != 0)
                {
                    tmp = GetNum(tmp, tmp1, minvalue, maxvalue, random);
                }
                else
                {
                    tmp = random.Next(minvalue, maxvalue);
                }
                switch (tmp)
                {
                    case 0:
                        sb.Append(CreateNum());
                        break;
                    case 1:
                        sb.Append(CreateBigAbc());
                        break;
                    case 2:
                        sb.Append(CreateSmallAbc());
                        break;
                    case 3:
                        sb.Append(CreateSymbol());
                        break;
                    default:
                        break;
                }
                tmp1 = tmp;
            }
            return sb.ToString();
        }

        private static int CreateNum()
        {
            Random random = new Random();
            int num = random.Next(0, 10);
            return num;
        }
        private static char CreateBigAbc()
        {
            Random random = new Random();
            int num = random.Next(65, 91);
            char ABC = Convert.ToChar(num);
            return ABC;
        }
        private static char CreateSmallAbc()
        {
            Random random = new Random();
            int num = random.Next(97, 123);
            char abc = Convert.ToChar(num);
            return abc;
        }
        private static char CreateSymbol()
        {
            Random random = new Random();
            int num = random.Next(33, 48);
            char symbol = Convert.ToChar(num);
            return symbol;
        }
        private static int GetNum(int tmp, int tmp1, int minValue, int maxValue, Random ra)
        {
            while (tmp == tmp1)
            {
                tmp = ra.Next(minValue, maxValue);
            }
            return tmp;
        }


    }
}
