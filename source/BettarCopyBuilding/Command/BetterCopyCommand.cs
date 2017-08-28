using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace BetterCopyBuilding
{
    static class BetterCopyCommand
    {
        private static BuildableDef tmpDef;
        private static ThingDef tmpStuff;
        private static StorageSettings tmpSettings;

        public static Command GetBetterCopyCommand(BuildableDef def, ThingDef stuff, StorageSettings settings)
        {
            Command_Action copyCom = new Command_Action();

            tmpDef = def;
            tmpStuff = stuff;
            tmpSettings = settings;

            copyCom.action = new Action(CopyCommandClicked);
            copyCom.activateSound = SoundDef.Named("Click");
            copyCom.defaultDesc = "BetterCopyCommandDesc".Translate();
            copyCom.defaultLabel = "BetterCopyCommandLabel".Translate();

            return copyCom;
        }
        public static void BuildingIconCopy(List<Gizmo> list, Command com)
        {
            foreach (Command c in list)
            {
                if (c != null && c.Label == "CommandBuildCopy".Translate())
                {
                    com.icon = c.icon;
                    com.defaultIconColor = c.defaultIconColor;
                    com.iconDrawScale = c.iconDrawScale;
                    com.iconProportions = c.iconProportions;
                }
            }
        }
        public static void CopyCommandClicked()
        {
#if DEBUG
            Log.Message(MethodBase.GetCurrentMethod().Name + "():");
#endif
            Designator_StorageBuild designator = new Designator_StorageBuild(tmpDef, tmpSettings);
            designator.SetStuffDef(tmpStuff);
            Find.DesignatorManager.Select(designator);
        }
    }
}
