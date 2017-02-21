using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    public class Randomer
    {

        public static long Long(int bit)
        {
            if (bit > 16)
            {
                throw new Exception("bit must <= 16");
            }
            if (bit < 6)
            {
                throw new Exception("bit must >= 6");
            }
            string midStr = "";
            byte[] bytes = Guid.NewGuid().ToByteArray();
            for (int i = 0; i < bit; i++)
            {
                midStr += bytes[i].ToString().Last<char>();
            }
            if (midStr[0] == '0')
            {
                midStr = new Random().Next(1, 10).ToString() + midStr.Substring(1);
            }
            return long.Parse(midStr);
        }
    }
}
