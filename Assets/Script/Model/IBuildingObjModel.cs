using System.Collections.Generic;
using System.Linq;

namespace Framework.BuildProject
{
    public interface IBuildingObjModel : IModel
    {
        void RegisterBuild(int id_, BuildingBase buildingBase_);
        void UnregisterBuild(int id_);
        BuildingBase GetBuildData(int id_);
        List<BuildingBase> GetBuildDataList(BuildingType type_);
        BuildingBase[] GetBuildingObjsDataForArchiveSystem();
        void Deinit();
    }

    public class BuildingObjModel : AbstractModel, IBuildingObjModel
    {
        private Dictionary<int, BuildingBase> m_BuildingDic;

        // 建物の登録
        public void RegisterBuild(int id_, BuildingBase buildingBase_)
        {
            if (!m_BuildingDic.ContainsKey(id_)) m_BuildingDic.Add(id_, buildingBase_);
        }

        // 建物の登録解除
        public void UnregisterBuild(int id_)
        {
            if (m_BuildingDic.TryGetValue(id_, out var obj))
            {
                obj.Reset();
                m_BuildingDic.Remove(id_);
            }
        }

        // 建物データの取得
        public BuildingBase GetBuildData(int id_)
        {
            m_BuildingDic.TryGetValue(id_, out var data);
            return data;
        }

        // 特定タイプの建物データリストを取得
        public List<BuildingBase> GetBuildDataList(BuildingType type_)
        {
            return m_BuildingDic.Values.Where(buildingObj => buildingObj.BuildingType == type_).ToList();
        }

        // アーカイブシステム用の建物データを取得
        public BuildingBase[] GetBuildingObjsDataForArchiveSystem()
        {
            return m_BuildingDic.Values.ToArray();
        }

        // 終了処理
        public void Deinit()
        {
            m_BuildingDic.Clear();
        }

        // 初期化処理
        protected override void OnInit()
        {
            m_BuildingDic = new Dictionary<int, BuildingBase>();
        }
    }
}