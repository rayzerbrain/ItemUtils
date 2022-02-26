using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using ItemUtils.Events;
using ItemUtils.Events.EventArgs;
using PlayerHandler = Exiled.Events.Handlers.Player;
using ItemHandler = Exiled.Events.Handlers.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items.Firearms.Modules;
using Exiled.API.Extensions;

namespace ItemUtils.API.Modifiers
{
    public class FirearmModifier : WeaponModifier
    {
        //attachement modification support soon (tm)
        //all needs testing
        public bool NeedsAmmo { get; set; } = true;
        public bool CanDisarm { get; set; } = true;
        
        public override void RegisterEvents()
        {
            PlayerHandler.Handcuffing += OnHandcuffing;
            ItemHandler.ChangingDurability += OnUsingAmmo;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.Handcuffing -= OnHandcuffing;
            ItemHandler.ChangingDurability -= OnUsingAmmo;
            base.UnregisterEvents();
        }
        public void OnUsingAmmo(ChangingDurabilityEventArgs ev)
        {
            if (!CanModify(ev.Firearm, ev.Player))
                return;

            ev.IsAllowed = NeedsAmmo;
        }
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!CanModify(ev.Cuffer.CurrentItem, ev.Cuffer))
                return;

            ev.IsAllowed = CanDisarm;
        }
    }
}
