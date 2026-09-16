using HarmonyLib;
using RimWorld;
using UnityEngine.UIElements;
using Verse;

namespace ProgressionAmmunition
{
    [HarmonyPatch(typeof(Verb_LaunchProjectile), "TryCastShot")]
    public static class Verb_LaunchProjectile_TryCastShot_Patch
    {
        public static bool Prefix(Verb_LaunchProjectile __instance)
        {
            if (ProgressionAmmunitionMod.Enabled && __instance.CasterPawn is Pawn pawn)
            {
                if (!ProgressionAmmunitionMod.settings.onlyColonistsUseAmmo || (pawn.IsColonist && pawn.Faction == Faction.OfPlayer))
                {
                    CompAmmo comp = __instance.EquipmentSource?.TryGetComp<CompAmmo>();
                    if (comp != null && comp.IsOutOfAmmo)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void Postfix(Verb_LaunchProjectile __instance, bool __result)
        {
            if (__result && ProgressionAmmunitionMod.Enabled && __instance.CasterPawn is Pawn pawn)
            {
                if (!ProgressionAmmunitionMod.settings.onlyColonistsUseAmmo || (pawn.IsColonist && pawn.Faction == Faction.OfPlayer))
                {
                    CompAmmo comp = __instance.EquipmentSource?.TryGetComp<CompAmmo>();
                    if (comp != null)
                    {
                        comp.ConsumeAmmo();
                        if (comp.IsOutOfAmmo)
                        {
                            MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "PA_MoteOutOfAmmo".Translate());

                            if (pawn.IsColonist && pawn.Faction == Faction.OfPlayer)
                            {
                                if (ProgressionAmmunitionMod.settings.autoRefillWithConsumable)
                                {
                                    comp.TryRefillAmmoFromConsumable();
                                }
                            }
                            else
                            {
                                if (comp.TryRefillAmmoFromConsumable())
                                    return;

                                OutOfAmmoUtility.TryStowOrDropWeapon(pawn);
                                OutOfAmmoUtility.TryEquipOtherWeapon(pawn);
                            }
                        }
                    }
                }
            }
        }
    }
}
