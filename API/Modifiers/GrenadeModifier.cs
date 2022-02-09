﻿using Exiled.API.Features.Items;
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

namespace ItemUtils.API.Modifiers
{
    public class GrenadeModifier : WeaponModifier
    {
        public float EffectDurationMulti { get; set; } = 1;
        public float ThrowTimeMulti { get; set; } = 1;
        public float FuseTimeMulti { get; set; } = 1;
        public float AoeMulti { get; set; } = 1;

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
            Throwable item = ev.Item;
            item.PinPullTime *= ThrowTimeMulti;

            if(ev.Item is ExplosiveGrenade gren)
            {
                gren.BurnDuration *= EffectDurationMulti;
                gren.ConcussDuration *= EffectDurationMulti;
                gren.DeafenDuration *= EffectDurationMulti;
                gren.FuseTime *= FuseTimeMulti;
                gren.MaxRadius *= AoeMulti;
            }
            else if (ev.Item is FlashGrenade flash)
            {
                flash.FuseTime *= FuseTimeMulti;
                flash.SurfaceDistanceIntensifier *= AoeMulti;
            }
        }
        
    }
}