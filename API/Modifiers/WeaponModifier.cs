using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.API.Extensions;
using Exiled.API.Enums;

namespace ItemUtils.API.Modifiers
{
    public class WeaponModifier : ItemModifier
    {
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
            ItemType damagingItem = ItemType.None;

            if (ev.Handler.Item != null) 
                damagingItem = ev.Handler.Item.Type;
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
                }
            }

            if (CanModify(damagingItem, ev.Attacker.Role) && ev.Target != null)
            {
                if (ev.Target.IsHuman)
                    ev.Amount *= HumanDamageMulti;
                else 
                    ev.Amount *= ScpDamageMulti;
            }
        }
    }
}
