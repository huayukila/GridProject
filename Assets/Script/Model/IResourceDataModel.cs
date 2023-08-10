using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{
    public interface IResourceDataModel : IModel
    {
        void DeductResources(BuildingCost[] costList);
        void DeductResources(BuildingCost buildingCost);
        void DeductResources(ResourceType resourceType,int value);
        
        void RiseResources(ResourceType resourceType,int value);
        bool IsResEnough(BuildingCost[] costList);
        bool IsResEnough(BuildingCost buildingCost);
        int GetRes(ResourceType resourceType);
    }
    public class ResourceDataModel : AbstractModel, IResourceDataModel
    {
        Dictionary<ResourceType, int> resDic;

        protected override void OnInit()
        {
            //全部のデータは今後セーフデータから読み込み
            //今は一応固定的な数値にして
            resDic = new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 100 },
                { ResourceType.Stone, 100 },
                { ResourceType.Gold, 100 },
                { ResourceType.Worker, 0 }
            };
        }
        public void DeductResources(BuildingCost[] costList)
        {
            if (IsResEnough(costList))
            {
                foreach (BuildingCost buildingCost in costList)
                {
                    resDic[buildingCost.resType] -= buildingCost.Cost;
                }
            }
        }

        public void DeductResources(BuildingCost buildingCost)
        {
            if (resDic.TryGetValue(buildingCost.resType, out int cost))
            {
                resDic[buildingCost.resType] -= cost;
            }
        }

        public int GetRes(ResourceType resourceType)
        {
            if (resDic.TryGetValue((resourceType), out int value))
            {
                return value;
            }
            return 0;
        }

        public bool IsResEnough(BuildingCost[] costList)
        {
            foreach (BuildingCost buildingCost in costList)
            {
                //まず、辞典の中にこの資源が存在しているのかを確認する
                if (resDic.TryGetValue(buildingCost.resType, out int cost))
                {
                    //この資源がたりるか
                    if (cost < buildingCost.Cost)
                    {
                        Debug.Log("資源不足");
                        return false;
                    }
                }
                else
                {
                    Debug.Log("資源不足");
                    return false;
                }
            }
            return true;
        }

        public bool IsResEnough(BuildingCost buildingCost)
        {
            if (resDic[buildingCost.resType] < buildingCost.Cost)
            {
                Debug.Log("資源不足");
                return false;
            }
            return true;
        }

        public void DeductResources(ResourceType resourceType, int value)
        {
            if (resDic.TryGetValue(resourceType,out int resValue))
            {
                if(value>resValue)
                {
                    Debug.Log(resourceType+"不足");
                }
                else
                {
                    resDic[resourceType]-=value;
                }
            }
            else
            {
                Debug.Log("この資源がまだ持っていません");
            }
        }

        public void RiseResources(ResourceType resourceType, int value)
        {
            if(resDic.ContainsKey(resourceType))
            {
                resDic[resourceType] += value;
            }
            else
            {
                resDic.Add(resourceType, value);
            }
        }
    }
}