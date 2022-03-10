﻿using System.Collections.Generic;
using System.Linq;

using Interactables.Interobjects.DoorUtils;

using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

using PlayerHandler = Exiled.Events.Handlers.Player;


namespace ItemUtils.API.Modifiers
{
    public class KeycardModifier : ItemModifier
    {
        //Only need to test remote cards
        
        public bool CanBeUsedRemotely { get; set; } = false;
        public List<KeycardPermissions> AddedPermissions { get; set; } = new List<KeycardPermissions>();
        public List<KeycardPermissions> RemovedPermissions { get; set; } = new List<KeycardPermissions>();

        public override void RegisterEvents()
        {
            PlayerHandler.InteractingDoor += OnInteractingDoor;
            PlayerHandler.UnlockingGenerator += OnUnlockingGenerator;
            PlayerHandler.InteractingLocker += OnInteractingLocker;
            PlayerHandler.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.InteractingDoor -= OnInteractingDoor;
            PlayerHandler.UnlockingGenerator -= OnUnlockingGenerator;
            PlayerHandler.InteractingLocker -= OnInteractingLocker;
            PlayerHandler.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            base.UnregisterEvents();
        }
        
        public void OnInteractingDoor(InteractingDoorEventArgs ev) => ev.IsAllowed = CheckPermissions(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions);
        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev) => ev.IsAllowed = CheckPermissions(ev.Player, KeycardPermissions.ArmoryLevelTwo);
        public void OnInteractingLocker(InteractingLockerEventArgs ev) => ev.IsAllowed = CheckPermissions(ev.Player, ev.Chamber.RequiredPermissions);
        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev) => ev.IsAllowed = CheckPermissions(ev.Player, KeycardPermissions.AlphaWarhead); 
        
        private bool CheckPermissions(Player plyr, KeycardPermissions perms)
        {
            Log.Debug($"Starting check of permissions {perms}", PluginMain.Instance.Config.DebugMode);

            if (perms == KeycardPermissions.None)
                return true;

            if (perms.HasFlagFast(KeycardPermissions.ScpOverride))
            {
                if (plyr.IsScp)
                    return true;
                perms -= KeycardPermissions.ScpOverride;
            }

            if (CanBeUsedRemotely)
            {
                foreach (Item item in plyr.Items)
                {
                    if (item is Keycard _card && CanModify(_card.Type) && CheckPermissions(_card, perms))
                        return true;
                }
            }

            return plyr.CurrentItem is Keycard card && CheckPermissions(card, perms);
        }
        private bool CheckPermissions(Keycard card, KeycardPermissions perms)
        {
            KeycardPermissions newPerms = card.Base.Permissions;

            if (CanModify(card, card.Owner))
            {
                newPerms += (ushort)AddedPermissions.Sum((perm) => (ushort)perm);
                newPerms -= (ushort)RemovedPermissions.Sum((perm) => (ushort)perm);
            }

            Log.Debug($"Checking permission {perms} against card with perms {newPerms}", PluginMain.Instance.Config.DebugMode);

            return newPerms.HasFlagFast(perms);
        }
        
    }
}
