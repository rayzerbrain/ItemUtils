using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using ItemUtils.Events;
using ItemUtils.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioItem = Exiled.API.Features.Items.Radio;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace ItemUtils.API.Modifiers
{
    public class DepletableModifier : ItemModifier
    {
        public float StartingEnergyMulti { get; set; } = 1;
        public bool HasInfiniteUse { get; set; } = false;

        public override void RegisterEvents()
        {
            CustomHandler.ObtainingItem += OnObtainingItem;
            PlayerHandler.UsingMicroHIDEnergy += OnUsingMicroHIDEnergy;
            PlayerHandler.UsingRadioBattery += OnUsingRadioBattery;
        }
        public override void UnregisterEvents()
        {
            CustomHandler.ObtainingItem -= OnObtainingItem;
            PlayerHandler.UsingMicroHIDEnergy -= OnUsingMicroHIDEnergy;
            PlayerHandler.UsingRadioBattery -= OnUsingRadioBattery;
        }

        public void OnObtainingItem(ObtainingItemEventArgs ev)
        {
            if (!CanModify(ev.Item, ev.Player))
                return;

            if (ev.Item is MicroHid micro)
            {
                micro.Energy *= StartingEnergyMulti;
            }
            else if (ev.Item is RadioItem radio)
            {
                radio.BatteryLevel = (byte)(StartingEnergyMulti * radio.BatteryLevel);
            }
        }
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
        {
            if (!CanModify(ev.Radio, ev.Player))
                return;

            ev.IsAllowed = !HasInfiniteUse;
        }
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev)
        {
            if (!CanModify(ev.MicroHID, ev.Player))
                return;

            ev.IsAllowed = !HasInfiniteUse;
        }
    }
}
