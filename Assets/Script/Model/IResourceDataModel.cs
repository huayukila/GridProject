using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IResourceDataModel : IModel
    {
        int MaxWorkerNum { get; set; }
        void MinusRes(ResourceCost[] costList_);
        void MinusRes(ResourceCost buildingCost_);
        void MinusRes(ResourceType resourceType_, int value_);

        void AddRes(ResourceType resourceType_, int value_);
        bool IsResEnough(ResourceCost[] costList_);
        bool IsResEnough(ResourceCost buildingCost_);
        int GetRes(ResourceType resourceType_);

        ResourceCost[] GetResourceDatasForArchiveSystem();
        void Deinit();
    }

    public class ResourceDataModel : AbstractModel, IResourceDataModel
    {
        private Dictionary<ResourceType, int> resDic;
        public int MaxWorkerNum { get; set; }

        // アーカイブシステム用のリソースデータ取得
        public ResourceCost[] GetResourceDatasForArchiveSystem()
        {
            return resDic.Select(res => new ResourceCost { resType = res.Key, Cost = res.Value }).ToArray();
        }

        // リソース削除
        public void Deinit()
        {
            InitializeResources();
        }

        // リソース消費
        public void MinusRes(ResourceCost[] costList_)
        {
            if (!IsResEnough(costList_)) return;

            foreach (var cost in costList_) MinusRes(cost);
        }

        public void MinusRes(ResourceCost buildingCost_)
        {
            if (resDic.TryGetValue(buildingCost_.resType, out var currentValue))
                resDic[buildingCost_.resType] = Mathf.Max(0, currentValue - buildingCost_.Cost);
        }

        // リソース取得
        public int GetRes(ResourceType resourceType_)
        {
            return resDic.TryGetValue(resourceType_, out var value) ? value : 0;
        }

        // リソース足りるかチェック
        public bool IsResEnough(ResourceCost[] costList_)
        {
            return costList_.All(IsResEnough);
        }

        public bool IsResEnough(ResourceCost buildingCost_)
        {
            return resDic.TryGetValue(buildingCost_.resType, out var currentValue) &&
                   currentValue >= buildingCost_.Cost;
        }

        // リソース減少
        public void MinusRes(ResourceType resourceType_, int value_)
        {
            if (!resDic.TryGetValue(resourceType_, out var currentValue) || currentValue < value_)
            {
                Debug.LogWarning($"{resourceType_} のリソースが足りません");
                return;
            }

            resDic[resourceType_] = Mathf.Max(0, currentValue - value_);
        }

        // リソース追加
        public void AddRes(ResourceType resourceType_, int value_)
        {
            if (resDic.ContainsKey(resourceType_))
                resDic[resourceType_] += value_;
            else
                resDic.Add(resourceType_, value_);
        }

        // 初期化処理
        protected override void OnInit()
        {
            // ここでリソースデータを初期化
            InitializeResources();
        }

        private void InitializeResources()
        {
            resDic = new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 800 },
                { ResourceType.Stone, 500 },
                { ResourceType.Gold, 160 },
                { ResourceType.Worker, 0 }
            };
            MaxWorkerNum = 0;
        }
    }
}