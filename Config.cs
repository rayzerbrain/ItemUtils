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
        [Description("List of items and their connected modifiers. Define new modifiers below, examples are given")]
        public Dictionary<ItemType, string> ModifiedItems { get; set; } = new Dictionary<ItemType, string>()
        {
            [ItemType.Painkillers] = "ModifiedPainkillers",
            [ItemType.Flashlight] = "SmallFlashlight",
            [ItemType.MicroHID] = "BigItem",
            [ItemType.KeycardContainmentEngineer] = "ModifiedContainmentEngineer",
            [ItemType.ArmorHeavy] = "ModifiedHeavyArmor",
            [ItemType.Coin] = "BigItem",
            [ItemType.ArmorHeavy] = "BigItem",
            [ItemType.GunCrossvec] = "ModifiedCrossvec",
            [ItemType.GrenadeHE] = "ModifiedExplosive",
            [ItemType.Radio] = "InfiniteRadio",
        };

        //blame yaml strict types not me
        [Description("Modifier descriptions for items")]
        public Dictionary<string, object> ItemModifiers { get; set; } = new Dictionary<string, object>()
        {
            ["InfiniteRadio"] = new DepletableModifier
            {
                IgnoredRoles = new List<RoleType>()
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
                Scale = new Vector3(2, 2, 2),
            },
            
            ["SmallFlashlight"] = new ItemModifier
            {
                Scale = new Vector3(1, 1, 0.75f),
            },
            
            ["ModifiedHeavyArmor"] = new ArmorModifier
            {
                Scale = new Vector3(1.2f, 1.2f, 1.2f),
                AmmoLimitMultis = new Dictionary<AmmoType, float>()
                {
                    {
                        AmmoType.Nato9, 1.2f
                    }
                },
                BodyProtectionMulti = 1.2f,
            },

            ["ModifiedPainkillers"] = new ConsumableModifier 
            {
                Scale = new Vector3(1, 3, 1), 
                HpAdded = 5, 
                AhpAdded = 0,
                Effects = new List<ConfigurableEffect>()
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
                AddedPermissions = { KeycardPermissions.AlphaWarhead, KeycardPermissions.Intercom }
            },
            
            ["ModifiedLogicer"] = new FirearmModifier
            {
                ScpDamageMulti = 1.2f,
            },
            
            ["ModifiedExplosive"] = new GrenadeModifier
            {
                FuseTimeMulti = 1.1f,
                ScpDamageMulti = 1.2f,
            },
        };

        [Description("Indicates whether the plugin will show debug logs")]
        public bool DebugMode { get; set; } = false;
    }
}
