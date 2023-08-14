using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{
    public interface IResourceDataModel : IModel
    {
        void DeductResources(ResourceCost[] costList);
        void DeductResources(ResourceCost buildingCost);
        void DeductResources(ResourceType resourceType,int value);
        
        void RiseResources(ResourceType resourceType,int value);
        bool IsResEnough(ResourceCost[] costList);
        bool IsResEnough(ResourceCost buildingCost);
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
        public void DeductResources(ResourceCost[] costList)
        {
            if (IsResEnough(costList))
            {
                foreach (ResourceCost buildingCost in costList)
                {
                    resDic[buildingCost.resType] -= buildingCost.Cost;
                }
            }
        }

        public void DeductResources(ResourceCost buildingCost)
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

        public bool IsResEnough(ResourceCost[] costList)
        {
            foreach (ResourceCost buildingCost in costList)
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

        public bool IsResEnough(ResourceCost buildingCost)
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