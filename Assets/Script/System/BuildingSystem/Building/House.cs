using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class House : BuildingBase
    {
        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            m_MaxWorkerNum = data_.m_LevelDatasList[0].m_MaxWorker;
        }
    }
}