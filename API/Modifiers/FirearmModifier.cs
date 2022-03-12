using System.Collections.Generic;
using System.Linq;

using MEC;

using InventorySystem.Items.Firearms.Attachments;

using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

using ItemUtils.Events;
using ItemUtils.Events.EventArgs;

using FirearmBase = InventorySystem.Items.Firearms.Firearm;
using PlayerHandler = Exiled.Events.Handlers.Player;
using ItemHandler = Exiled.Events.Handlers.Item;


namespace ItemUtils.API.Modifiers
{
    public class FirearmModifier : WeaponModifier
    {
        public bool NeedsAmmo { get; set; } = true;
        public bool CanDisarm { get; set; } = true;
        public Dictionary<AttachmentNameTranslation, Dictionary<AttachmentParam, float>> ModifiedAttachments { get; set; } = new Dictionary<AttachmentNameTranslation, Dictionary<AttachmentParam, float>>();
        
        public override void RegisterEvents()
        {
            PlayerHandler.Handcuffing += OnHandcuffing;
            ItemHandler.ChangingDurability += OnUsingAmmo;
            ItemHandler.ChangingAttachments += OnChangingAttachments;
            CustomHandler.ObtainingItem += OnObtainingItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.Handcuffing -= OnHandcuffing;
            ItemHandler.ChangingDurability -= OnUsingAmmo;
            ItemHandler.ChangingAttachments -= OnChangingAttachments;
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
            if (!CanModify(ev.Item, ev.Player) || !(ev.Item is Firearm gun))
                return;

            Timing.CallDelayed(0.1f, () => ModifyAttachments(gun.Base));
        }
        public void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!CanModify(ev.Firearm, ev.Player))
                return;

            Timing.CallDelayed(0.1f, () => ModifyAttachments(ev.Firearm.Base));
        }
        private void ModifyAttachments(FirearmBase gun)
        {
            if (ModifiedAttachments.TryGetValue(AttachmentNameTranslation.None, out Dictionary<AttachmentParam, float> defaultParams))
                ModifyParameters(gun, defaultParams);

            foreach (FirearmAttachment att in gun.Attachments.Where((att) => att.IsEnabled && att.Name != AttachmentNameTranslation.None))
            {
                if (ModifiedAttachments.TryGetValue(att.Name, out Dictionary<AttachmentParam, float> attParams))
                    ModifyParameters(gun, attParams);
            }
        }
        private void ModifyParameters(FirearmBase gun, Dictionary<AttachmentParam, float> newParams)
        {
            Dictionary<AttachmentParam, float> oldParams = gun.DictionarizedAttachmentParameters;

            foreach (KeyValuePair<AttachmentParam, float> newPair in newParams)
            {
                if (oldParams.ContainsKey(newPair.Key))
                {
                    Log.Debug($"Old value was {oldParams[newPair.Key]}", PluginMain.Instance.Config.DebugMode);
                    oldParams[newPair.Key] = AttachmentsUtils.ProcessValue(gun, newPair.Value, newPair.Key);
                }
                else
                    oldParams.Add(newPair.Key, newPair.Value);
                Log.Debug($"{newPair.Key} is now {oldParams[newPair.Key]}", PluginMain.Instance.Config.DebugMode);
            }
        }
    }
}
