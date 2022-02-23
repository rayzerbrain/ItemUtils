using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using PlayerHandler = Exiled.Events.Handlers.Player;
using ItemUtils.Events;
using ItemUtils.Events.EventArgs;

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
        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev) 
        {
            Log.Debug("EVENT CALLEDDEDD!");
            Log.Debug($"IS allowed? {ev.IsAllowed}");
            //throw new Exception();
            ev.IsAllowed = CheckPermissions(ev.Player, KeycardPermissions.AlphaWarhead); 
        }

        public bool CheckPermissions(Player plyr, KeycardPermissions perms)
        {
            if (perms == KeycardPermissions.None) return true;

            if (CanBeUsedRemotely)
            {
                foreach (Keycard _card in plyr.Items)
                {
                    if (CheckPermissions(_card, perms))
                        return true;
                }
                return false;
            }
            
            return plyr.CurrentItem is Keycard card && CheckPermissions(card, perms);
        }
        public bool CheckPermissions(Keycard card, KeycardPermissions perms)
        {
            KeycardPermissions newPerms = card.Base.Permissions;

            if (CanModify(card.Type, card.Owner.Role.Type))
            {
                foreach (KeycardPermissions perm in AddedPermissions)
                {
                    Log.Debug("ADDING PERM " + perm);
                    newPerms += (ushort)perm;
                }
                foreach (KeycardPermissions perm in RemovedPermissions)
                {
                    newPerms -= (ushort)perm;
                }
            }

            Log.Debug($"Checking permission {perms} against card with perms {newPerms}", PluginMain.Instance.Config.DebugMode);

            return newPerms.HasFlagFast(perms);
        }
        
    }
}
