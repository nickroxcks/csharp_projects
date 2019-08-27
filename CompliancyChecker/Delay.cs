using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delay
{
    class Delay
    {
        public Delay(int time)
        {
            DateTime start = DateTime.Now;
            while (DateTime.Now.Subtract(start).Seconds < 1)
            {
            }
        }
    }
}
