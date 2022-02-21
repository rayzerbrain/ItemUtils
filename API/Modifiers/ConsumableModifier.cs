using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Linq;
using Radio = Exiled.API.Features.Items.Radio;
using PlayerHandler = Exiled.Events.Handlers.Player;
using System.Collections.Generic;
using Exiled.API.Enums;
using ItemUtils.Events.EventArgs;
using ItemUtils.Events;
using PlayerStatsSystem;

namespace ItemUtils.API.Modifiers
{
    public class ConsumableModifier : ItemModifier
    {
        public float UseTimeMulti { get; set; } = 1;
        public float CooldownMulti { get; set; } = 1;
        public float HpAdded { get; set; } = 0; //tested
        public float AhpAdded { get; set; } = 0;
        public List<ConfigurableEffect> Effects { get; set; } = new List<ConfigurableEffect>();

        public override void RegisterEvents()
        {
            PlayerHandler.UsingItem += OnUsingItem;
            PlayerHandler.UsedItem += OnItemUsed;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.UsingItem -= OnUsingItem;
            PlayerHandler.UsedItem -= OnItemUsed;
            base.UnregisterEvents();
        }
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (CanModify(ev.Item, ev.Player))
            {
                Log.Debug("Modify check passed for UsingItem", PluginMain.Instance.Config.DebugMode);
                ev.Item.UseTime *= UseTimeMulti;
                ev.Item.RemainingCooldown *= CooldownMulti;
            }
        }
        public void OnItemUsed(UsedItemEventArgs ev)
        {
            Log.Debug($"Chcecking Item of type {ev.Item.Type} for player {ev.Player}");

            if (!CanModify(ev.Item, ev.Player))
                return;

            Log.Debug("Healing... ", PluginMain.Instance.Config.DebugMode);

            if (HpAdded >= 0) 
                ev.Player.Heal(HpAdded);
            else 
                ev.Player.Hurt(HpAdded); //Hopefully this reason won't be needed

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
