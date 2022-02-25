using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.API.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features.DamageHandlers;

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
            if(ev.Handler.BaseIs(out FirearmDamageHandler fdh))
            {
                fdh.ite
            }


            /*ItemType damagingItem = ItemType.None;


            //Test if hadnler.Item uses grenades, if it doesn't then make it
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
                }
            }

            if (Type == damagingItem && !IgnoredRoles.Contains(ev.Attacker.Role.Type) && ev.Target != null)
            {
                if (ev.Target.IsHuman)
                    ev.Amount *= HumanDamageMulti;
                else 
                    ev.Amount *= ScpDamageMulti;
            }*/
        }
    }
}
