using System.ComponentModel;
using System.Collections.Generic;

using UnityEngine;

using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;

using Exiled.API.Interfaces;
using Exiled.API.Enums;

using ItemUtils.API;
using ItemUtils.API.Modifiers;

namespace ItemUtils
{
    public class Config : IConfig
    {
        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;
        //Have list of mapped items with types and load in the OnENabled
        [Description("A list of the names of custom items of which the modifications should not affect")]
        public List<string> IgnoredCustomItems = new List<string>()
        {
            "Example",
        };

        //blame yaml strict types not me
        [Description("Modifier descriptions for items")]
        public Dictionary<string, object> ItemModifiers { get; set; } = new Dictionary<string, object>()
        {
            ["InfiniteRadio"] = new DepletableModifier
            {
                AffectedItems = 
                {
                    ItemType.Radio
                },
                IgnoredRoles = 
                {
                    RoleType.ChaosConscript,
                    RoleType.ChaosRifleman,
                    RoleType.ChaosRepressor,
                    RoleType.ChaosMarauder,
                },
                HasInfiniteUse = true,
            },

            ["BigItem"] = new ItemModifier
            {
                AffectedItems = 
                {
                    ItemType.Coin,
                    ItemType.ArmorHeavy,
                },
                Scale = new Vector3(2, 2, 2),
            },
            
            ["SmallFlashlight"] = new ItemModifier
            {
                AffectedItems = 
                {
                    ItemType.Flashlight,
                },
                Scale = new Vector3(1, 1, 0.75f),
            },
            
            ["ModifiedHeavyArmor"] = new ArmorModifier
            {
                Scale = new Vector3(1.2f, 1.2f, 1.2f),
                AmmoLimitMultis = 
                {
                    {
                        AmmoType.Nato9, 1.2f
                    }
                },
                BodyProtectionMulti = 1.2f,
            },

            ["ModifiedPainkillers"] = new ConsumableModifier 
            {
                AffectedItems = 
                {
                    ItemType.Painkillers,
                },
                Scale = new Vector3(1, 3, 1), 
                HpAdded = 5, 
                AhpAdded = 0,
                Effects = 
                {
                    new ConfigurableEffect()
                    {
                        Type = EffectType.Invigorated,
                        Duration = 10,
                        Chance = 10,
                    }
                },
            },
            
            ["ModifiedContainmentEngineer"] = new KeycardModifier
            {
                AffectedItems =
                {
                    ItemType.KeycardContainmentEngineer,
                },
                AddedPermissions = { KeycardPermissions.AlphaWarhead, KeycardPermissions.Intercom }
            },
            
            ["ModifiedLogicer"] = new FirearmModifier
            {
                AffectedItems =
                {
                    ItemType.GunLogicer,
                },
                ScpDamageMulti = 1.2f,
            },
        };

        [Description("Indicates whether the plugin will show debug logs")]
        public bool DebugMode { get; set; } = false;
    }
}
