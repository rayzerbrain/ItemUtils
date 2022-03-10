using System.Collections.Generic;

using UnityEngine;

using Exiled.API.Features.Items;
using Exiled.API.Enums;

using ItemUtils.Events;
using ItemUtils.Events.EventArgs;


namespace ItemUtils.API.Modifiers
{
    public class ArmorModifier : ItemModifier
    {
        //ammo limits currently client-sided, leaving them in case that changes (it won't)
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

            /*for (int i=0; i<armor.Base.AmmoLimits.Length; i++)
            {
                ArmorAmmoLimit lim = armor.Base.AmmoLimits[i];
                AmmoType aType = lim.AmmoType.GetAmmoType();

                if(AmmoLimitMultis.ContainsKey(aType))
                {
                    ArmorAmmoLimit newLim = lim;
                    newLim.Limit = (ushort)(lim.Limit * AmmoLimitMultis[aType]);
                    Log.Debug($"Old value for {aType}: {lim.Limit}. New value: {newLim.Limit}", PluginMain.Instance.Config.DebugMode);
                    armor.Base.AmmoLimits[i] = newLim;
                    
                }
            }*/
            
            armor.HelmetEfficacy = (int)Mathf.Clamp(armor.HelmetEfficacy * HelmetProtectionMulti, 0, 100);
            armor.VestEfficacy = (int)Mathf.Clamp(armor.VestEfficacy * BodyProtectionMulti, 0, 100);
            armor.StaminaUseMultiplier = Mathf.Clamp(armor.StaminaUseMultiplier * StaminaUseMulti, 0, 1);
        }
    }
}
