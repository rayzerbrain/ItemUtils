using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using ItemUtils.Events;
using ItemUtils.Events.EventArgs;
using PlayerHandler = Exiled.Events.Handlers.Player;
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
        public bool NeedsReloading { get; set; } = true;
        public bool CanDisarm { get; set; } = true;
        
        public override void RegisterEvents()
        {
            CustomHandler.ObtainingItem += OnObtainingItem;
            PlayerHandler.Shooting += OnShooting;
            PlayerHandler.Handcuffing += OnHandcuffing;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            CustomHandler.ObtainingItem -= OnObtainingItem;
            PlayerHandler.Shooting -= OnShooting;
            PlayerHandler.Handcuffing -= OnHandcuffing;
            base.UnregisterEvents();
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            Firearm gun = ev.Shooter.CurrentItem as Firearm;

            if (!NeedsAmmo) 
                gun.Ammo++;
            else if (!NeedsReloading)
            {
                gun.Ammo++;
                ev.Shooter.Ammo[gun.AmmoType.GetItemType()]--;
            }
        }
        public void OnObtainingItem(ObtainingItemEventArgs ev)
        {
            Log.Debug("PATCHING SUCCESS!");
        }
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!CanModify(ev.Cuffer.CurrentItem, ev.Cuffer))
                return;

            ev.IsAllowed = CanDisarm;
        }
    }
}
