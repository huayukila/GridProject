using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IResourceDataModel : IModel
    {
        void MinusRes(ResourceCost[] costList);
        void MinusRes(ResourceCost buildingCost);
        void MinusRes(ResourceType resourceType, int value);

        void AddRes(ResourceType resourceType, int value);
        bool IsResEnough(ResourceCost[] costList);
        bool IsResEnough(ResourceCost buildingCost);
        int GetRes(ResourceType resourceType);
        int MaxWorkerNum { get; set; }

        ResourceCost[] GetResourceDatasForArchiveSystem();
        void Deinit();
    }

    public class ResourceDataModel : AbstractModel, IResourceDataModel
    {
        Dictionary<ResourceType, int> resDic;
        public int MaxWorkerNum { get; set; }

        protected override void OnInit()
        {
            //全部のデータは今後セーフデータから読み込み
            //今は一応固定的な数値にして
            resDic = new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 1000 },
                { ResourceType.Stone, 1000 },
                { ResourceType.Gold, 1000 },
                { ResourceType.Worker, 0 }
            };
            MaxWorkerNum = 0;
        }

        public ResourceCost[] GetResourceDatasForArchiveSystem()
        {
            int index = 0;
            ResourceCost[] tempArray = new ResourceCost[resDic.Count];
            foreach (var res in resDic)
            {
                tempArray[index].resType = res.Key;
                tempArray[index].Cost = res.Value;
                index++;
            }

            return tempArray;
        }

        public void Deinit()
        {
            resDic = new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 1000 },
                { ResourceType.Stone, 1000 },
                { ResourceType.Gold, 1000 },
                { ResourceType.Worker, 0 }
            };
            MaxWorkerNum = 0;
        }

        public void MinusRes(ResourceCost[] costList)
        {
            if (IsResEnough(costList))
            {
                foreach (ResourceCost buildingCost in costList)
                {
                    MinusRes(buildingCost);
                }
            }
        }

        public void MinusRes(ResourceCost buildingCost)
        {
            if (resDic.ContainsKey(buildingCost.resType))
            {
                resDic[buildingCost.resType] -= buildingCost.Cost;
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
                if (!IsResEnough(buildingCost))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsResEnough(ResourceCost buildingCost)
        {
            if (!resDic.ContainsKey(buildingCost.resType) || resDic[buildingCost.resType] < buildingCost.Cost)
            {
                Debug.Log("資源不足");
                return false;
            }

            return true;
        }

        public void MinusRes(ResourceType resourceType, int value)
        {
            if (resDic.TryGetValue(resourceType, out int resValue))
            {
                if (value > resValue)
                {
                    Debug.Log(resourceType + "不足");
                }
                else
                {
                    resDic[resourceType] -= value;
                }
            }
            else
            {
                Debug.Log("この資源がまだ持っていません");
            }
        }

        public void AddRes(ResourceType resourceType, int value)
        {
            if (resDic.ContainsKey(resourceType))
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