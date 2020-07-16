using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.Attributes
{
    public class Wallet
    {
        public static Wallet Instance
        {
            get;
            set;
        }

        public static readonly string DataPath = "Stuff";

    }
}
