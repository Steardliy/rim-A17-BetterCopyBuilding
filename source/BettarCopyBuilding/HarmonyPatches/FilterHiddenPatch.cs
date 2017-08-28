using Harmony;
using System;
using System.Reflection;
using Verse;

namespace BetterCopyBuilding
{
    [HarmonyPatch(typeof(ThingFilter), "RecalculateDisplayRootCategory", new Type[0])]
    static class ThingFilter_RecalculateDisplayRootCategory_Fix
    {
        public static bool Prefix(ThingFilter __instance)
        {
#if DEBUG
                Log.Message(Debug.GetMethodName(2) + "@" + MethodBase.GetCurrentMethod().Name);
#endif
            __instance.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
            return false;
        }
    }
}
