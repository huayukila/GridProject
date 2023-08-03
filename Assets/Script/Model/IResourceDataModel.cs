using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{
    public enum ResourceType
    {
        Mood,
        Stone,
        Gold,
        Worker
    }
    [Serializable]
    public struct BuildingCost
    {
        public ResourceType resType;
        public int Cost;
    }
    [Serializable]
    public struct BuildingLevelData
    {
        public int Level;
        public int MaxWorker;
        public int MaxHp;
        public BuildingCost[] costList;
    }
    public interface IResourceDataModel : IModel
    {
        //
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
            //�S���̃f�[�^�͍���Z�[�t�f�[�^����ǂݍ���
            //���͈ꉞ�Œ�I�Ȑ��l�ɂ���
            resDic = new Dictionary<ResourceType, int>
            {
                { ResourceType.Mood, 100 },
                { ResourceType.Stone, 100 },
                { ResourceType.Gold, 100 },
                { ResourceType.Worker, 10 }
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
                //�܂��A���T�̒��ɂ��̎��������݂��Ă���̂����m�F����
                if (resDic.TryGetValue(buildingCost.resType, out int cost))
                {
                    //���̎���������邩
                    if (cost < buildingCost.Cost)
                    {
                        Debug.Log("�����s��");
                        return false;
                    }
                }
                else
                {
                    Debug.Log("�����s��");
                    return false;
                }
            }
            return true;
        }

        public bool IsResEnough(BuildingCost buildingCost)
        {
            if (resDic[buildingCost.resType] < buildingCost.Cost)
            {
                Debug.Log("�����s��");
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
                    Debug.Log(resourceType+"�s��");
                }
                else
                {
                    resDic[resourceType]-=value;
                }
            }
            else
            {
                Debug.Log("���̎������܂������Ă��܂���");
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