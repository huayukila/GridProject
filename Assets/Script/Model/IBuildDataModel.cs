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
        private Dictionary<string, BuildingData> m_BuildingConfigDic;

        // 建築の設定を取得
        public BuildingData GetBuildingConfig(string name_)
        {
            if (m_BuildingConfigDic.TryGetValue(name_, out var buildingData))
            {
                return buildingData;
            }

            Debug.LogWarning($"探している建物のタイプ ({name_}) は存在していない");
            return null;
        }

        // モデルの初期化
        protected override void OnInit()
        {
            LoadBuildingData();
        }

        // 建築データの読み込み
        private void LoadBuildingData()
        {
            m_BuildingConfigDic = new Dictionary<string, BuildingData>();
            var database = Resources.Load<BuildingDatabase>("BuildingDatabase");

            foreach (var data in database.BuildingDatas) m_BuildingConfigDic.Add(data.NameString, data);

            Resources.UnloadAsset(database);
        }
    }
}