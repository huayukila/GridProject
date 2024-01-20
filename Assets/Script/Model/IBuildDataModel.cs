using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuilDataModel : IModel
    {
        BuildingData GetBuildingConfig(string name_);
    }

    public class BuildDataModel : AbstractModel, IBuilDataModel
    {
        Dictionary<string, BuildingData> m_BuildingConfigDic;

        // モデルの初期化
        protected override void OnInit()
        {
            LoadBuildingData();
        }

        // 建築データの読み込み
        private void LoadBuildingData()
        {
            m_BuildingConfigDic = new Dictionary<string, BuildingData>();
            BuildingDatabase database = Resources.Load<BuildingDatabase>("BuildingDatabase");

            foreach (var data in database.BuildingDatas)
            {
                m_BuildingConfigDic.Add(data.name, data);
            }

            Resources.UnloadAsset(database);
        }

        // 建築の設定を取得
        public BuildingData GetBuildingConfig(string name_)
        {
            if (m_BuildingConfigDic.TryGetValue(name_, out BuildingData buildingData))
            {
                return buildingData;
            }
            else
            {
                Debug.LogWarning($"探している建物のタイプ ({name_}) は存在していない");
                return null;
            }
        }
    }
}