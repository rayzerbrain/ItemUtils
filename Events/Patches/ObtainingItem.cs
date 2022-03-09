using Exiled.API.Features;
using HarmonyLib;

using InventorySystem;
using InventorySystem.Items;

using ItemUtils.Events.EventArgs;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using static HarmonyLib.AccessTools;


namespace ItemUtils.Events.Patches
{
    [HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerAddItem))]
    public class ObtainingItemPatch
    {
        // using il because armor ammo limits don't update client side using postfix
        // still doesn't work cause client is stubborn and doesn't update it's limits accordingly -_-
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            const int offset = -2;

            int index = newInstructions.FindLastIndex(i =>
                i.opcode == OpCodes.Callvirt &&
                (MethodInfo)i.operand == Method(typeof(ItemBase), nameof(ItemBase.OnAdded))) + offset;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Inventory), nameof(Inventory._hub))),
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ObtainingItemEventArgs))[0]),
                new CodeInstruction(OpCodes.Call, Method(typeof(CustomHandler), nameof(CustomHandler.OnObtainingItem))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
