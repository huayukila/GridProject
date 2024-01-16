using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class UpdateBuilding : BuildingBase
    {
        public int m_UpdateTime;
        private float m_DurationTime;
        private bool isWorking => m_WorkerNum > 0;

        public void Update()
        {
            if (!isWorking)
            {
                m_DurationTime = Time.time;
                return;
            }

            OnUpdate();
            if (Time.time - m_DurationTime < m_UpdateTime) return;
            m_DurationTime = Time.time;
            OnExecute();
        }

        protected virtual void OnUpdate()
        {
            
        }

        protected virtual void OnExecute()
        {
            
        }

        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            m_WorkerNum = 0;
            m_MaxWorkerNum = data_.m_LevelDatasList[0].m_MaxWorker;
            m_UpdateTime = data_.m_LevelDatasList[0].m_UpdateTime;
            m_DurationTime = 0;
        }
    }
}