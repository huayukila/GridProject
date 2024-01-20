using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class House : BuildingBase
    {
        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            MaxWorkerNum = data_.LevelDatasList[0].MaxWorker;
        }
    }
}