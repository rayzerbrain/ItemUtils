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
        //Each needs testing
        public static readonly KeycardPermissions generatorPerm = KeycardPermissions.ArmoryLevelTwo;
        
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
        
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            ev.IsAllowed = CheckPermissions(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions); //ah yes  
        }
        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            ev.IsAllowed = CheckPermissions(ev.Player, generatorPerm);
        }
        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            ev.IsAllowed = CheckPermissions(ev.Player, ev.Chamber.RequiredPermissions);
        }
        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            ev.IsAllowed = CheckPermissions(ev.Player, KeycardPermissions.AlphaWarhead);
        }

        public bool CheckPermissions(Player plyr, KeycardPermissions perms)
        {
            if (ExcludedRoles.Contains(plyr.Role)) return false;
            if (perms == KeycardPermissions.None) return true;

            if (CanBeUsedRemotely)
            {
                foreach (Keycard card in plyr.Items)
                {
                    if (CanModify(card, plyr) && CheckPermissions(card, perms))
                        return true;
                }
                return false;
            }
            
            return CanModify(plyr.CurrentItem, plyr) && CheckPermissions((Keycard)plyr.CurrentItem, perms);
        }
        public bool CheckPermissions(Keycard card, KeycardPermissions perms)
        {
            if (card == null || card.Type != Type)
                return false;

            foreach (KeycardPermissions perm in AddedPermissions) { card.Base.Permissions += (ushort)perm; }
            foreach (KeycardPermissions perm in AddedPermissions) { card.Base.Permissions -= (ushort)perm; }

            Log.Debug($"Checking permission {perms} against card with perms { card.Base.Permissions}", PluginMain.Instance.Config.DebugMode);

            return card.Base.Permissions.HasFlagFast(perms);
        }
        
    }
}
