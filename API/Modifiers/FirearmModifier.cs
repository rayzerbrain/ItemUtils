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
        //gonna include support for modifiable attachments when nao gets his attachment api approved (never)
        //nvm
        public bool NeedsAmmo { get; set; } = true;
        public bool NeedsReloading { get; set; } = true;
        public AmmoType AmmoUsed { get; set; } = AmmoType.None;
        

        public override void RegisterEvents()
        {
            CustomHandler.ObtainingItem += OnObtainingItem;
            PlayerHandler.ReloadingWeapon += OnReloading;
            PlayerHandler.Shooting += OnShooting;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            CustomHandler.ObtainingItem -= OnObtainingItem;
            PlayerHandler.ReloadingWeapon -= OnReloading;
            PlayerHandler.Shooting -= OnShooting;
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
                ev.Shooter.Ammo[AmmoUsed.GetItemType()]--;
            }
        }
        public void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (!ev.IsAllowed || !CanModify(ev.Firearm, ev.Player) || AmmoUsed == AmmoType.None) 
                return;

            ushort bulletsToLoad = (ushort)(ev.Firearm.MaxAmmo - ev.Firearm.Ammo);
            ushort bulletsAvailable = ev.Player.GetAmmo(AmmoUsed);
            if (bulletsAvailable < bulletsToLoad)
                bulletsToLoad = bulletsAvailable;

            ev.Firearm.Ammo += (byte)bulletsToLoad;
            ev.Player.Ammo[AmmoUsed.GetItemType()] -= bulletsToLoad;
        }
        public void OnObtainingItem(ObtainingItemEventArgs ev)
        {
            Log.Debug("PATCHING SUCCESS!");
        }
    }
}
