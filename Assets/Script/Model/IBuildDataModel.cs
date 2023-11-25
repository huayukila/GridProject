using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{

    public interface IBuilDataModel : IModel
    {
        BuildingData GetBuildingConfig(string Name);
    }
    public class BuildDataModel : AbstractModel, IBuilDataModel
    {
        Dictionary<string, BuildingData> BuildingConfigDic;

        public BuildingData GetBuildingConfig(string Name)
        {
            if (BuildingConfigDic.TryGetValue(Name, out BuildingData buildingData))
            {
                return buildingData;
            }
            else
            {
                Debug.Log("íTÇµÇΩÇ¢åöï®ÇÕë∂ç›ÇµÇƒÇ¢Ç»Ç¢");
                return null;
            }
        }

        protected override void OnInit()
        {
            BuildingConfigDic = new Dictionary<string, BuildingData>()
            {
                {"House",Resources.Load<BuildingData>("BuildData/House")},
                {"Factory",Resources.Load<BuildingData>("BuildData/Factory")}
            };
        }
    }
}