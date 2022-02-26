using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapHandler = Exiled.Events.Handlers.Map;
using PlayerHandler = Exiled.Events.Handlers.Player;

using ServerHandler = Exiled.Events.Handlers.Server;
using MEC;
using Exiled.API.Features.Items;
using Exiled.API.Extensions;
using ItemUtils.Events;
using Exiled.API.Features;
using UnityEngine;
using Exiled.Events.EventArgs;
using Exiled.CustomItems.API.Features;

namespace ItemUtils.API.Modifiers
{
    public class ItemModifier
    {
        //Only pick up time needs testing

        // The type of item the modifier will affect
        internal ItemType Type;
        //for keeping track of one-time modifications
        internal List<ushort> RegisteredSerials = new List<ushort>();
        public List<RoleType> IgnoredRoles { get; set; } = new List<RoleType>();
        public List<string> ExcludedCustomItems { get; set; } = new List<string>();
        public Vector3 Scale { get; set; } = Vector3.one;

        public virtual void RegisterEvents()
        {
            ServerHandler.WaitingForPlayers += OnWaitingForPlayers;
            MapHandler.SpawningItem += OnSpawningItem;
            PlayerHandler.DroppingItem += OnDroppingItem;
        }
        public virtual void UnregisterEvents()
        {
            ServerHandler.WaitingForPlayers -= OnWaitingForPlayers;
            MapHandler.SpawningItem -= OnSpawningItem;
            PlayerHandler.DroppingItem -= OnDroppingItem;
        }
        public void OnWaitingForPlayers() => RegisteredSerials.Clear();
        //Spawning item event is never called, Needs Fixing
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            Timing.CallDelayed(0.1f, () => 
            {
                if (ev.Pickup.Type != Type)
                    return;

                Log.Debug($"Attempting to modify scale of item {ev.Pickup.Type}", PluginMain.Instance.Config.DebugMode);

                ev.Pickup.Scale = Scale;
            });
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            Timing.CallDelayed(0.1f, () =>
            {
                Log.Debug($"Attempting to modify scale of item {ev.Item.Type}", PluginMain.Instance.Config.DebugMode);
                Pickup pickup = Map.Pickups.First((p) => p.Serial == ev.Item.Serial);
                pickup.Scale = Scale;
            });
        }
        // For null checks
        public bool CanModify(Pickup p)
        {
            CustomItem.TryGet(p, out CustomItem ci);
            return CanModify(p?.Type) && CanModify(ci);
        }
        public bool CanModify(Item item, Player plyr)
        {
            CustomItem.TryGet(item, out CustomItem ci);
            return CanModify(item?.Type) && CanModify(ci) && plyr != null && !IgnoredRoles.Contains(plyr.Role.Type);
        }
        public bool CanModify(ItemType? t) =>
            t != null && t != ItemType.None && (Type == t || Type == ItemType.None);
        private bool CanModify(CustomItem ci) =>
            ci == null || !PluginMain.Instance.Config.IgnoredCustomItems.Contains(ci.Name);
    }
}
