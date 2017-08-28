using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Verse;

namespace BetterCopyBuilding
{
    class StorageSettingManager : MapComponent
    {
        private Dictionary<IntVec3, StorageSettings> holdingSettingList = new Dictionary<IntVec3, StorageSettings>();
       
        public StorageSettingManager(Map map) : base(map) { }

        public override void FinalizeInit()
        {
            holdingSettingList.RemoveAll(list => !(Verse.Find.VisibleMap.thingGrid.ThingAt<Blueprint_Build>(list.Key) != null ||
               Verse.Find.VisibleMap.thingGrid.ThingAt<Frame>(list.Key) != null));
                
            base.FinalizeInit();
        }
        public void Add(StorageSettings settings, IntVec3 position)
        {
            StorageSettings s = new StorageSettings();
            s.CopyFrom(settings);
            holdingSettingList.Add(position, s);
#if DEBUG
            Log.Message(Debug.GetMethodName() + "set =" + s.ToString() + " pos=" + position.ToString() + " count:" + holdingSettingList.Count);
#endif
        }
        public StorageSettings Find(IntVec3 position)
        {
            StorageSettings result;
            holdingSettingList.TryGetValue(position, out result);
#if DEBUG
            Log.Message(Debug.GetMethodName() + "pos=" + position.ToString() + " count:" + holdingSettingList.Count);
#endif
            return result;
        }
        public void Remove(IntVec3 position)
        {
            
            holdingSettingList.Remove(position);
#if DEBUG
            Log.Message(Debug.GetMethodName() + "pos=" + position.ToString() + " count:" + holdingSettingList.Count);
#endif
        }
        public override void ExposeData()
        {
            Scribe_Collections.Look<IntVec3, StorageSettings>(ref holdingSettingList, "holdingSettingList");
            if(holdingSettingList == null)
            {
                holdingSettingList = new Dictionary<IntVec3, StorageSettings>();
            }
#if DEBUG
            Log.Message("holdingSettingList Expose:" + holdingSettingList.Count);
#endif
        }
    }
}
