using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class UpdateBuilding : BuildingBase
    {
        private float m_DurationTime; // 最後の更新からの経過時間
        private int m_UpdateTime; // 公共?量，更新時間の設定
        private bool m_IsWorking => WorkerNum > 0; // 労働者がいるかどうか

        private void Update()
        {
            // 労働者がいない場合は処理をスキップ
            if (!m_IsWorking)
            {
                m_DurationTime = Time.time;
                return;
            }

            OnUpdate();
            // 指定の更新時間が経過していない場合は処理をスキップ
            if (Time.time - m_DurationTime < m_UpdateTime) return;
            m_DurationTime = Time.time;
            OnExecute();
        }

        // 更新時の処理（オーバーライド可能）
        protected virtual void OnUpdate()
        {
            // サブクラスで具体的な処理を定義
        }

        // 定期実行の処理（オーバーライド可能）
        protected virtual void OnExecute()
        {
            // サブクラスで具体的な処理を定義
        }

        // 建物の初期化
        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            WorkerNum = 0;
            MaxWorkerNum = data_.LevelDatasList[0].MaxWorker;
            m_UpdateTime = data_.LevelDatasList[0].UpdateTime;
            m_DurationTime = 0;
        }
    }
}