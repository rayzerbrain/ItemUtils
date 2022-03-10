using System.Collections.Generic;
using System.Linq;

using Exiled.Events.EventArgs;

using PlayerHandler = Exiled.Events.Handlers.Player;


namespace ItemUtils.API.Modifiers
{
    public class ConsumableModifier : ItemModifier
    {
        public float UseTimeMulti { get; set; } = 1;
        public float HpAdded { get; set; } = 0;
        public float AhpAdded { get; set; } = 0;
        public List<ConfigurableEffect> Effects { get; set; } = new List<ConfigurableEffect>();

        public override void RegisterEvents()
        {
            PlayerHandler.UsingItem += OnUsingItem;
            PlayerHandler.UsedItem += OnUsedItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.UsingItem -= OnUsingItem;
            PlayerHandler.UsedItem -= OnUsedItem;
            base.UnregisterEvents();
        }
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (CanModify(ev.Item, ev.Player))
                ev.Item.UseTime *= UseTimeMulti;
        }
        public void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            if (HpAdded >= 0) 
                ev.Player.Heal(HpAdded);
            else 
                ev.Player.Hurt(HpAdded);

            if (AhpAdded > 0 || ev.Player.ActiveArtificialHealthProcesses.Any())
                ev.Player.ArtificialHealth += AhpAdded;

            int rand = UnityEngine.Random.Range(0, 99);

            foreach (ConfigurableEffect effect in Effects)
            {
                if (rand < effect.Chance) 
                    ev.Player.EnableEffect(effect.Type, effect.Duration);
            }
        }
        
    }
}
