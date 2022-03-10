using System.ComponentModel;
using System.Collections.Generic;

using UnityEngine;

using Exiled.API.Interfaces;
using Exiled.API.Enums;

using ItemUtils.API;
using ItemUtils.API.Modifiers;

using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;


namespace ItemUtils
{
    public class Config : IConfig
    {
        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("A list of the names of custom items of which the modifications should not affect")]
        public List<string> IgnoredCustomItems = new List<string>()
        {
            "Example",
        };

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
                Scale = new Vector3(1.5f, 1.5f, 1.5f),
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
                AffectedItems =
                {
                    ItemType.ArmorHeavy,
                },
                Scale = new Vector3(0.9f, 0.9f, 0.9f),
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
                    new ConfigurableEffect
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
