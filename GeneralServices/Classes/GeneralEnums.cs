using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Classes
{
    public enum SevenPointLikertScale : byte
    {
        NoAcceptable = 1,
        VeryWeak = 2,
        Weak = 3,
        NotBad = 4,
        Good = 5,
        VeryGood = 6,
        Excellent = 7
    }

    public enum FivePointLikertScale : byte
    {
        VeryWeak = 1,
        Weak = 2,
        NotBad = 3,
        Good = 4,
        VeryGood = 5
    }
}
