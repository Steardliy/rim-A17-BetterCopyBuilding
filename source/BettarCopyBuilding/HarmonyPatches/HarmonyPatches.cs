using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace BetterCopyBuilding
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.steardliy.BetterCopyBuilding");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            
#if DEBUG
            Log.Message("Harmony is initialized(by BetterCopyBuilding)");
#endif
        }
        [HarmonyPatch(typeof(Building_Storage), "GetGizmos", new Type[0])]
        static class Building_Storage_GetGizmos_Fix
        {
            public static void Postfix(Building_Storage __instance, ref IEnumerable<Gizmo> __result)
            {
#if DEBUG
                Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name);
#endif
                List<Gizmo> list = new List<Gizmo>();
                foreach (var x in __result)
                {
                    list.Add(x);
                }
                if (__instance.Faction == Faction.OfPlayer && __instance.def.blueprintClass == typeof(Blueprint_Build))
                {
                    Command com = BetterCopyCommand.GetBetterCopyCommand(__instance.def, __instance.Stuff, __instance.GetStoreSettings());
                    BetterCopyCommand.BuildingIconCopy(list, com);
                    list.Add(com);
                }
                __result = list;
            }
        }

        [HarmonyPatch(typeof(Building_Storage), "SpawnSetup", new Type[] { typeof(Map), typeof(bool) })]
        static class Building_Storage_SpawnSetup_Fix
        {
            public static void Postfix(Building_Storage __instance)
            {
#if DEBUG
                Log.Message(Debug.GetMethodName(2) + ": Postfix");
#endif
                StorageSettings s = __instance.Map.GetComponent<StorageSettingManager>().Find(__instance.Position);
                if (s == null)
                {
#if DEBUG
                    Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name + ": settings is not found at Pos=" + __instance.Position.ToString());
#endif
                    return;
                }
                __instance.settings.CopyFrom(s);
                __instance.Map.GetComponent<StorageSettingManager>().Remove(__instance.Position);
#if DEBUG
                Log.Message("allowed:" + s.filter.AllowedDefCount + "  hitpoint:" + s.filter.allowedHitPointsConfigurable + " quality:"
                            + s.filter.allowedQualitiesConfigurable);
                Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name + ": copy is completed");
#endif
            }
        }

        [HarmonyPatch(typeof(Blueprint_Build), "Destroy", new Type[] { typeof(DestroyMode) })]
        static class Blueprint_Build_Destroy_Fix
        {
            public static void Prefix(Blueprint_Build __instance, DestroyMode mode = DestroyMode.Vanish)
            {
#if DEBUG
                Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name + ": mode=" + mode.ToString());
#endif
                if (mode == DestroyMode.Cancel || mode == DestroyMode.KillFinalize || mode == DestroyMode.Deconstruct)
                {
                    ThingDef def = __instance.def.entityDefToBuild as ThingDef;
                    if (def != null && typeof(Building_Storage).IsAssignableFrom(def.thingClass))
                    {
                        __instance.Map.GetComponent<StorageSettingManager>().Remove(__instance.Position);
                    }
                }
            }
        }
        /*[HarmonyPatch(typeof(Frame), "Destroy", new Type[] { typeof(DestroyMode) })]
        static class Frame_Destroy_Fix
        {
            public static void Prefix(Frame __instance, DestroyMode mode = DestroyMode.Vanish)
            {
#if DEBUG
                Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name + ": mode=" + mode.ToString());
#endif
                if (mode == DestroyMode.Cancel || mode == DestroyMode.KillFinalize || mode == DestroyMode.Deconstruct)
                {
                    StorageSettingManager.Remove(__instance.Position);
                }
            }
        }*/
    }
}
