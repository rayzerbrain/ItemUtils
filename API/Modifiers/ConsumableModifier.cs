using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using Radio = Exiled.API.Features.Items.Radio;
using PlayerHandler = Exiled.Events.Handlers.Player;
using System.Collections.Generic;
using Exiled.API.Enums;
using ItemUtils.Events.EventArgs;
using ItemUtils.Events;

namespace ItemUtils.API.Modifiers
{
    public class ConsumableModifier : ItemModifier
    {
        public float UseTimeMulti { get; set; } = 1;
        public float CooldownMulti { get; set; } = 1;
        public float HpAdded { get; set; } = 0;
        public float AhpAdded { get; set; } = 0;
        //For micro and Hid only
        public List<ConfigurableEffect> Effects { get; set; } = new List<ConfigurableEffect>();

        public override void RegisterEvents()
        {
            PlayerHandler.UsingItem += OnUsingItem;
            PlayerHandler.ItemUsed += OnItemUsed;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.UsingItem -= OnUsingItem;
            PlayerHandler.ItemUsed -= OnItemUsed;
            base.UnregisterEvents();
        }
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (CanModify(ev.Item, ev.Player))
            {
                ev.Item.UseTime *= UseTimeMulti;
            }
        }
        public void OnItemUsed(UsedItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            Log.Debug("Healing... ", PluginMain.Instance.Config.DebugMode);

            if (HpAdded >= 0) 
                ev.Player.Heal(HpAdded);
            else ev.Player.Hurt("A needle is stuck in his asophagus.", HpAdded); //Hopefully this reason won't be needed

            ev.Player.ArtificialHealth += AhpAdded;
            ev.Item.RemainingCooldown *= CooldownMulti;

            int rand = UnityEngine.Random.Range(0, 99);
            foreach (ConfigurableEffect effect in Effects)
            {
                if (rand < effect.Chance) 
                    ev.Player.EnableEffect(effect.Type, effect.Duration);
            }
        }
        
    }
}
