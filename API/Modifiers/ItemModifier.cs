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

namespace ItemUtils.API.Modifiers
{
    public class ItemModifier
    {
        //Only pick up time needs testing

        // The type of item the modifier will affect
        internal ItemType Type;
        //for keeping track of one-time modifications
        internal List<ushort> RegisteredSerials = new List<ushort>();
        public List<RoleType> ExcludedRoles { get; set; } = new List<RoleType>();
        public Vector3 Scale { get; set; } = Vector3.one;
        public float PickUpTimeMulti { get; set; } = 1;

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
                ev.Pickup.Weight *= PickUpTimeMulti;
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
                pickup.Weight *= PickUpTimeMulti; // Needs testing
            });
        }
        // For null checks
        public bool CanModify(Item item, Player plyr) => 
            plyr != null && item != null 
            && CanModify(item.Type, plyr.Role);
        //For enums only
        public bool CanModify(ItemType iType, RoleType rType) => 
            iType != ItemType.None && (Type == iType || Type == ItemType.None)
            && rType != RoleType.None && !ExcludedRoles.Contains(rType);
    }
}
