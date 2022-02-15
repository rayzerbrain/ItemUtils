using Exiled.API.Features.Items;
using Exiled.API.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using ItemUtils.Events.EventArgs;
using ItemUtils.Events;

namespace ItemUtils.API.Modifiers
{
    public class ArmorModifier : ItemModifier
    {
        //all needs testing
        public Dictionary<AmmoType, float> AmmoLimitMultis { get; set; } = new Dictionary<AmmoType, float>();
        public float HelmetProtectionMulti { get; set; } = 1;
        public float BodyProtectionMulti { get; set; } = 1;
        public float StaminaUseMulti { get; set; } = 1;

        public override void RegisterEvents()
        {
            CustomHandler.ObtainingItem += OnObtainingItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            CustomHandler.ObtainingItem -= OnObtainingItem;
            base.UnregisterEvents();
        }
        public void OnObtainingItem(ObtainingItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            Log.Debug($"Successful modification of item {ev.Item.Type}");

            Armor armor = ev.Item as Armor;

            //List<ArmorAmmoLimit> newLimits = new List<ArmorAmmoLimit>(armor.AmmoLimits);
            for (int i=0; i<armor.AmmoLimits.Count(); i++)
            {
                ArmorAmmoLimit limit = armor.AmmoLimits.ElementAt(i);
                AmmoType aType = limit.AmmoType;
                if(AmmoLimitMultis.ContainsKey(aType))
                {
                    ArmorAmmoLimit newLim = armor.AmmoLimits.ElementAt(i);
                    newLim.Limit = (ushort)(limit.Limit * AmmoLimitMultis[aType]);
                    armor.AmmoLimits.ToList()[i] = newLim;
                }
                /*if (AmmoLimitMultis.ContainsKey(armor.AmmoType))
                {
                    ushort newMax = (ushort)(ammoLimit.Limit * AmmoLimitMultis[ammoLimit.AmmoType]);
                    newLimits.Add(new ArmorAmmoLimit(ammoLimit.AmmoType, newMax));
                }*/
            }
            //armor.AmmoLimits = newLimits;

            int newHelmetEfficacy = (int)(armor.HelmetEfficacy * HelmetProtectionMulti);
            if (newHelmetEfficacy > 100) 
               armor.HelmetEfficacy = 100;
            else 
                armor.HelmetEfficacy = newHelmetEfficacy;

            int newBodyEfficacy = (int)(armor.VestEfficacy * BodyProtectionMulti);
            if (newBodyEfficacy > 100) 
                armor.VestEfficacy = 100;
            else 
                armor.VestEfficacy = newBodyEfficacy;

            if (StaminaUseMulti >= 1 && StaminaUseMulti <= 2) 
                armor.StaminaUseMultiplier = StaminaUseMulti;
            else Log.Warn("Armor stamina use multiplier is an invalid number. Please change it to be between 1 and 2 for it to work");
        }
        
    }
}
