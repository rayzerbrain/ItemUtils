using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using PlayerHandler = Exiled.Events.Handlers.Player;
using InventorySystem.Items.ThrowableProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomPlayerEffects;
using UnityEngine;
using Exiled.API.Features;

namespace ItemUtils.API.Modifiers
{
    public class GrenadeModifier : WeaponModifier
    {
        // Everything needs testing
        public decimal EffectDurationMulti { get; set; } = 1;
        public decimal ThrowTimeMulti { get; set; } = 1;
        public decimal FuseTimeMulti { get; set; } = 1; //works
        public decimal AoeMulti { get; set; } = 1;

        public override void RegisterEvents()
        {
            PlayerHandler.ThrowingItem += OnThrowingItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.ThrowingItem -= OnThrowingItem;
            base.UnregisterEvents();
        }
        public void OnThrowingItem(ThrowingItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            Log.Debug("Item being throen!", PluginMain.Instance.Config.DebugMode);

            Throwable item = ev.Item;
            item.PinPullTime *= (float)ThrowTimeMulti;

            if(ev.Item is ExplosiveGrenade gren)
            {
                gren.BurnDuration *= (float)EffectDurationMulti;
                gren.ConcussDuration *= (float)EffectDurationMulti;
                gren.DeafenDuration *= (float)EffectDurationMulti;
                gren.FuseTime *= (float)FuseTimeMulti;
                gren.MaxRadius *= (float)AoeMulti;
            }
            else if (ev.Item is FlashGrenade flash)
            {
                flash.FuseTime *= (float)FuseTimeMulti;
                flash.SurfaceDistanceIntensifier *= (float)AoeMulti;
            }
        }
        
    }
}
