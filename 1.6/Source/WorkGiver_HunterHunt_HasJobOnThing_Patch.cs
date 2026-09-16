using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAmmunition
{
    [HarmonyPatch(typeof(WorkGiver_HunterHunt), nameof(WorkGiver_HunterHunt.HasJobOnThing))]
    public static class WorkGiver_HunterHunt_HasJobOnThing_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (__result is false || !ProgressionAmmunitionMod.Enabled || pawn.Faction != Faction.OfPlayer)
            {
                return;
            }

            if (ProgressionAmmunitionMod.settings.onlyColonistsUseAmmo && !pawn.IsColonist)
            {
                return;
            }

            var comp = pawn.equipment?.Primary?.TryGetComp<CompAmmo>();
            if (comp != null && comp.IsOutOfAmmo) __result = false;
        }
    }
}
