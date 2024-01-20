using UnityEngine;

namespace Framework.BuildProject
{
    // 建物システムのインターフェース
    interface IBuildingSystem : ISystem
    {
        void AddWorker(GameObject obj_);
        void RemoveWorker(GameObject obj_);
        void RepairAllBuilding();
        void RepairBuilding();
    }

    // 建物システムの実装
    public class BuildingSystem : AbstractSystem, IBuildingSystem
    {
        private IBuildingObjModel m_BuildingObjModel; // 建物オブジェクトモデル
        private IResourceDataModel m_ResourceDataModel; // 資源データモデル

        // 初期化処理
        protected override void OnInit()
        {
            m_BuildingObjModel = this.GetModel<IBuildingObjModel>();
            m_ResourceDataModel = this.GetModel<IResourceDataModel>();
        }

        // 働き手を追加する処理
        public void AddWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            int resWorker = m_ResourceDataModel.GetRes(ResourceType.Worker);
            if (buildingBase.WorkerNum < buildingBase.MaxWorkerNum && resWorker > 0)
            {
                buildingBase.WorkerNum++;
                m_ResourceDataModel.MinusRes(ResourceType.Worker, 1);
            }
        }

        // 働き手を削除する処理
        public void RemoveWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            if (buildingBase.WorkerNum > 0)
            {
                buildingBase.WorkerNum--;
                m_ResourceDataModel.AddRes(ResourceType.Worker, 1);
            }
        }

        // すべての建物を修理する（現在空のメソッド）
        public void RepairAllBuilding()
        {
            // 実装予定の処理
        }

        // 特定の建物を修理する（現在空のメソッド）
        public void RepairBuilding()
        {
            // 実装予定の処理
        }
    }
}