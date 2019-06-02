using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerForRPG
{
    public interface IMonotonicClock
    {
        float GetNow();
    }
}
