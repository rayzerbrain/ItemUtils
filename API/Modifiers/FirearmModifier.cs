﻿using System.Collections.Generic;
using System.Linq;

using MEC;

using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Attachments.Components;

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
        public float WeightMulti { get; set; } = 1;
        public float LengthMulti { get; set; } = 1;

        public Dictionary<AttachmentName, Dictionary<AttachmentParam, float>> ModifiedAttachments { get; set; } = new Dictionary<AttachmentName, Dictionary<AttachmentParam, float>>();
        
        public override void RegisterEvents()
        {
            PlayerHandler.Handcuffing += OnHandcuffing;
            CustomHandler.ObtainingItem += OnObtainingItem;
            ItemHandler.ChangingAttachments += OnChangingAttachments;
            ItemHandler.ChangingDurability += OnUsingAmmo;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.Handcuffing -= OnHandcuffing;
            CustomHandler.ObtainingItem -= OnObtainingItem;
            ItemHandler.ChangingAttachments -= OnChangingAttachments;
            ItemHandler.ChangingDurability -= OnUsingAmmo;
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

            Timing.CallDelayed(0.1f, () =>
            {
                Log.Debug(gun.Base.BaseLength + ", " + gun.Base.BaseWeight);
                gun.Base.BaseLength *= LengthMulti;
                gun.Base.BaseWeight *= WeightMulti;
                Log.Debug(gun.Base.BaseLength + ", " + gun.Base.BaseWeight);
                ModifyAttachments(gun.Base);
            });
        }
        public void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!CanModify(ev.Firearm, ev.Player))
                return;

            Timing.CallDelayed(0.1f, () => ModifyAttachments(ev.Firearm.Base));
        }
        private void ModifyAttachments(FirearmBase gun)
        {
            //this should affect a random attachment on the gun, but since it is reset each att change it doesn't matter which one it affects
            if (ModifiedAttachments.TryGetValue(AttachmentName.None, out Dictionary<AttachmentParam, float> defaultParams))
                ModifyParameters(gun, gun.Attachments.FirstOrDefault((att) => att != null && att.IsEnabled), defaultParams);

            foreach (Attachment att in gun.Attachments.Where((att) => att.IsEnabled && att.Name != AttachmentName.None))
            {
                if (ModifiedAttachments.TryGetValue(att.Name, out Dictionary<AttachmentParam, float> attParams))
                    ModifyParameters(gun, att, attParams);
            }
        }
        /*private void ModifyParameters(FirearmBase gun, Dictionary<AttachmentParam, float> newParams)
        {
            Dictionary<AttachmentParam, float> oldParams = AttachmentParameterDefinition.Definitions;
            AttachmentParameterDefinition.Definitions.Add(null, null);
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
        }*/

        private void ModifyParameters(FirearmBase gun, Attachment att, Dictionary<AttachmentParam, float> newParams)
        {
            Log.Debug("Changing params for " + att.Name, PluginMain.Instance.Config.DebugMode);
            foreach (KeyValuePair<AttachmentParam, float> pair in newParams)
            {
                att.SetParameterValue(pair.Key, gun.ProcessValue(pair.Value, pair.Key));
            }
        }
    }
}
