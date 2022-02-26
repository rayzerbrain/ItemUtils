using Exiled.API.Features.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using ItemUtils.Events.EventArgs;
using ItemUtils.Events;
using static InventorySystem.Items.Armor.BodyArmor;

namespace ItemUtils.API.Modifiers
{
    public class ArmorModifier : ItemModifier
    {
        //ammo limits work
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

            Armor armor = ev.Item as Armor;

            //List<ArmorAmmoLimit> newLimits = new List<ArmorAmmoLimit>(armor.AmmoLimits);
            for (int i=0; i<armor.Base.AmmoLimits.Length; i++)
            {
                ArmorAmmoLimit limit = armor.Base.AmmoLimits[i];
                AmmoType aType = limit.AmmoType.GetAmmoType();

                if(AmmoLimitMultis.ContainsKey(aType))
                {
                    ArmorAmmoLimit newLim = limit;
                    newLim.Limit = (ushort)(limit.Limit * AmmoLimitMultis[aType]);
                    armor.Base.AmmoLimits[i] = newLim;
                }
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
            else Log.Warn("Armor stamina use multiplier is an invalid number. Please change it to be between 1 and 2 for it to work correctly.");
        }
        
    }
}
