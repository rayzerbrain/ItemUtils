using Exiled.API.Enums;

namespace ItemUtils.API
{
    public struct ConfigurableEffect
    {
        public EffectType Type { get; set; }
        public int Chance { get; set; }
        public int Duration { get; set; }
    }
}
