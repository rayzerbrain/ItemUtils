using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemUtils.API
{
    public struct ConfigurableEffect
    {
        public EffectType Type { get; set; }
        public int Chance { get; set; }
        public int Duration { get; set; }
    }
}
