﻿using System.ComponentModel;
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
        [Description("List of items and their connected modifiers. Define new modifiers below, examples are given")]
        public Dictionary<ItemType, string> ModifiedItems { get; set; } = new Dictionary<ItemType, string>()
        {
            [ItemType.Painkillers] = "ModifiedPainkillers",
            [ItemType.Flashlight] = "ModifiedFlashlight",
            [ItemType.MicroHID] = "BigItem",
            [ItemType.KeycardContainmentEngineer] = "ModifiedContainmentEngineer",
            [ItemType.ArmorHeavy] = "ModifiedHeavyArmor",
            [ItemType.Coin] = "BigItem",
            [ItemType.GunCrossvec] = "ModifiedCrossvec",
            [ItemType.GrenadeFlash] = "ModifiedExplosive",
            [ItemType.Radio] = "BetterRadio"
        };

        //blame yaml strict types not me
        [Description("Modifier descriptions for items")]
        public Dictionary<string, object> ItemModifiers { get; set; } = new Dictionary<string, object>()
        {
            ["BetterRadio"] = new DepletableModifier
            {
                StartingEnergyMulti = 1.2f,
                ExcludedRoles = new List<RoleType>()
                {
                    RoleType.ChaosConscript,
                    RoleType.ChaosRifleman,
                    RoleType.ChaosRepressor,
                    RoleType.ChaosMarauder,
                },
            },

            ["BigItem"] = new ItemModifier
            {
                Scale = new Vector3(2, 2, 2),
                PickUpTimeMulti = 3f,
            },
            
            ["ModifiedFlashlight"] = new ItemModifier
            {
                Scale = new Vector3(1, 1, 0.75f)
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
                NeedsReloading = false,
            },
            
            ["ModifiedExplosive"] = new GrenadeModifier
            {
                FuseTimeMulti = 0.8f,
                ThrowTimeMulti = 1.0f,
                ScpDamageMulti = 1.1f,
            },
        };

        [Description("Indicates whether the plugin will show debug logs")]
        public bool DebugMode { get; set; } = false;
    }
}
