using HarmonyLib;

using InventorySystem;
using InventorySystem.Items;

using ItemUtils.Events.EventArgs;

namespace ItemUtils.Events.Patches
{
    [HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerAddItem))]
    public class ObtainingItemPatch
    {
        //no il in case of interference with possible exiled patches
        //also im lazy
        public static void Postfix(ReferenceHub __0, ref ItemBase __result)
        {
            ItemBase item = __result;
            /*Timing.CallDelayed(0.5f, () =>
            {*/
                ObtainingItemEventArgs ev = new ObtainingItemEventArgs(__0, item);
                CustomHandler.OnObtainingItem(ev);
            //});
        }
    }
}
