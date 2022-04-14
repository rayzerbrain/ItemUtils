using System.ComponentModel;
using System.Collections.Generic;

using UnityEngine;

using Exiled.API.Interfaces;
using Exiled.API.Enums;

using ItemUtils.API;
using ItemUtils.API.Modifiers;

using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;
using InventorySystem.Items.Firearms.Attachments;

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
            ["Infinite Radio"] = new DepletableModifier
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
            
            ["Small Flashlight"] = new ItemModifier
            {
                AffectedItems = 
                {
                    ItemType.Flashlight,
                },
                Scale = new Vector3(1, 1, 0.75f),
            },

            ["Modified Painkillers"] = new ConsumableModifier 
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
            
            ["Modified Containment Engineer"] = new KeycardModifier
            {
                AffectedItems =
                {
                    ItemType.KeycardContainmentEngineer,
                },
                AddedPermissions = { KeycardPermissions.AlphaWarhead, KeycardPermissions.Intercom }
            },
            
            ["Modified Logicer"] = new FirearmModifier
            {
                AffectedItems =
                {
                    ItemType.GunLogicer,
                },
                ScpDamageMulti = 1.2f,
                ModifiedAttachments =
                {
                    [AttachmentName.Foregrip] = new Dictionary<AttachmentParam, float>()
                    {
                        [AttachmentParam.HipInaccuracyMultiplier] = 0.9f,
                    },
                },
            },
        };

        [Description("Indicates whether the plugin will show debug logs")]
        public bool DebugMode { get; set; } = false;
    }
}
