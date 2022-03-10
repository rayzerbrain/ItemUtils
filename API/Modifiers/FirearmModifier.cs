using Exiled.Events.EventArgs;

using PlayerHandler = Exiled.Events.Handlers.Player;
using ItemHandler = Exiled.Events.Handlers.Item;
using InventorySystem.Items.Firearms.Attachments;
using System.Collections.Generic;
using ItemUtils.Events;
using ItemUtils.Events.EventArgs;
using Exiled.API.Features.Items;

namespace ItemUtils.API.Modifiers
{
    public class FirearmModifier : WeaponModifier
    {
        //attachement modification support soon (tm)
        //all needs testing
        public bool NeedsAmmo { get; set; } = true;
        public bool CanDisarm { get; set; } = true;
        public Dictionary<AttachmentNameTranslation, Dictionary<AttachmentParam, float>> ModifiedAttachments { get; set; } = new Dictionary<AttachmentNameTranslation, Dictionary<AttachmentParam, float>>();
        
        public override void RegisterEvents()
        {
            PlayerHandler.Handcuffing += OnHandcuffing;
            ItemHandler.ChangingDurability += OnUsingAmmo;
            CustomHandler.ObtainingItem += OnObtainingItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.Handcuffing -= OnHandcuffing;
            ItemHandler.ChangingDurability -= OnUsingAmmo;
            CustomHandler.ObtainingItem -= OnObtainingItem;
            base.UnregisterEvents();
        }
        public void OnUsingAmmo(ChangingDurabilityEventArgs ev)
        {
            if (!CanModify(ev.Firearm, ev.Player))
                return;

            ev.IsAllowed = NeedsAmmo;
        }
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!CanModify(ev.Cuffer.CurrentItem, ev.Cuffer))
                return;

            ev.IsAllowed = CanDisarm;
        }
        public void OnObtainingItem(ObtainingItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            Firearm gun = ev.Item as Firearm;

            foreach (FirearmAttachment att in gun.Attachments)
            {
                if (!ModifiedAttachments.ContainsKey(att.Name))
                    continue;

                for (int i = 0; i < att.Settings.SerializedParameters.Length; i++)
                {
                    if (ModifiedAttachments[att.Name].TryGetValue(att.Settings.SerializedParameters[i].Parameter, out float multi))
                        att.Settings.SerializedParameters[i].Value *= multi;
                }
            }
        }
    }
}
