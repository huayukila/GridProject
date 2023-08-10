using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuildingModel : IModel
    {
        public void RegisterBuild(GameObject obj, Buildingobj buildingobj);
        public void UnregisterBuild(GameObject obj);
        public void AddWorker(GameObject obj);
        public void RemoveWorker(GameObject obj);
        public void GetDamage(GameObject obj,int damage);

        public Buildingobj GetBuildData(GameObject obj);
    }

    public class BuildingModel : AbstractModel, IBuildingModel
    {
        Dictionary<GameObject, Buildingobj> BuildingDic;
        protected override void OnInit()
        {
            BuildingDic = new Dictionary<GameObject, Buildingobj>();
        }

        public void UnregisterBuild(GameObject obj)
        {
            if (BuildingDic.ContainsKey(obj))
            {
                BuildingDic.Remove(obj);
            }
        }

        public Buildingobj GetBuildData(GameObject obj)
        {
            BuildingDic.TryGetValue(obj, out Buildingobj data);
            return data;
        }

        public void RegisterBuild(GameObject obj, Buildingobj buildingobj)
        {
            if (!BuildingDic.ContainsKey(obj))
            {
                BuildingDic.Add(obj, buildingobj);
            }
        }

        public void AddWorker(GameObject obj)
        {
            BuildingDic[obj].WorkerNum++;
        }

        public void RemoveWorker(GameObject obj)
        {
            BuildingDic[(obj)].WorkerNum--;
        }

        public void GetDamage(GameObject obj, int damage)
        {
            BuildingDic[(obj)].BuildingHp-=damage;
        }
    }
}