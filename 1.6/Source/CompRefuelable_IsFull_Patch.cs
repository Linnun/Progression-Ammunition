using HarmonyLib;
using RimWorld;

namespace ProgressionAmmunition
{
    [HarmonyPatch(typeof(CompRefuelable), nameof(CompRefuelable.IsFull), MethodType.Getter)]
    public static class CompRefuelable_IsFull_Patch
    {
        public static bool Prefix(CompRefuelable __instance, ref bool __result)
        {
            if (ProgressionAmmunitionMod.settings.refillBuildingsAreInfinite && __instance.parent is Building_AmmoRecharger)
            {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
