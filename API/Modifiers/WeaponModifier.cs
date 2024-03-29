﻿using Exiled.API.Features.DamageHandlers;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;


namespace ItemUtils.API.Modifiers
{
    public class WeaponModifier : ItemModifier
    {        
        //Needs testing
        public float ScpDamageMulti { get; set; } = 1;
        public float HumanDamageMulti { get; set; } = 1;
        public override void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnregisterEvents();
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            ItemType damagingItem;


            if (ev.Handler.BaseIs(out FirearmDamageHandler handler)) 
                damagingItem = handler.Item.Type;
            else
            {
                switch(ev.Handler.Type)
                {
                    case DamageType.Scp018:
                        damagingItem = ItemType.SCP018;
                        break;
                    case DamageType.Explosion:
                        damagingItem = ItemType.GrenadeHE;
                        break;
                    default:
                        return;
                }
            }

            if (AffectedItems.Contains(damagingItem) && !IgnoredRoles.Contains(ev.Attacker.Role.Type) && ev.Target != null)
            {
                if (ev.Target.IsHuman)
                    ev.Amount *= HumanDamageMulti;
                else 
                    ev.Amount *= ScpDamageMulti;
            }
        }
    }
}
