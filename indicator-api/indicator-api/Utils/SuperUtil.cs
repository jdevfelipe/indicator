using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Utils
{
    interface SuperUtil
    {
        public Boolean ObjectIsNull (Object obj)
        {
            if (obj is null) {
                return true;
            }
            return false;
        }

        public Boolean StringIsNull (string field)
        {
            if (field == "" || field == null)
            {
                return true;
            }
            return false;
        }

        public long GenerateRamdom() 
        {
            Random rnd = new Random();
            int numberOne = rnd.Next(1, 1000);
            int numberTwo = rnd.Next(1, 2000);
            long result = numberOne + numberTwo;

            return result;
        } 
    }
}
