using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuilDataModel : IModel
    {
        BuildingData GetBuildingConfig(BuildingType buildingType_);
    }

    public class BuildDataModel : AbstractModel, IBuilDataModel
    {
        Dictionary<BuildingType, BuildingData> m_BuildingConfigDic;

        protected override void OnInit()
        {
            BuildingDatabase database = Resources.Load<BuildingDatabase>("BuildingDatabase");
            m_BuildingConfigDic = new Dictionary<BuildingType, BuildingData>();

            foreach (var data in database.BuildingDatas)
            {
                m_BuildingConfigDic.Add(data.m_BuildingType, data);
            }

            Resources.UnloadAsset(database);
        }

        public BuildingData GetBuildingConfig(BuildingType buildingType_)
        {
            if (m_BuildingConfigDic.TryGetValue(buildingType_, out BuildingData buildingData))
            {
                return buildingData;
            }
            else
            {
                Debug.Log("íTÇµÇΩÇ¢åöï®ÇÕë∂ç›ÇµÇƒÇ¢Ç»Ç¢");
                return null;
            }
        }
    }
}