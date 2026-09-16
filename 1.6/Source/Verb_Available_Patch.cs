using HarmonyLib;
using Verse;

namespace ProgressionAmmunition
{
    [HarmonyPatch(typeof(Verb), nameof(Verb.Available))]
    public static class Verb_Available_Patch
    {
        public static void Postfix(Verb __instance, ref bool __result)
        {
            if (__result && !__instance.IsMeleeAttack && __instance.CasterPawn is Pawn pawn && RefillUtility.DoesPawnUseAmmo(pawn))
            {
                var comp = __instance.EquipmentSource?.TryGetComp<CompAmmo>();
                if (comp != null && comp.IsOutOfAmmo)
                {
                    __result = false;
                }
            }
        }
    }
}
