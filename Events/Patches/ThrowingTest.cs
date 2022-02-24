using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using static InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler;

namespace ItemUtils.Events.Patches
{
    [HarmonyPatch(typeof(ThrowableNetworkHandler), nameof(ThrowableNetworkHandler.ServerProcessMessages))]
    public static class ThrowingTest
    {
        public static bool Prefix()
        {
            Log.Debug("Throwing event called for item ");
            return true;
        }
    }
}
