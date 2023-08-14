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
            //�S���̃f�[�^�͍���Z�[�t�f�[�^����ǂݍ���
            //���͈ꉞ�Œ�I�Ȑ��l�ɂ���
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

        public bool IsResEnough(ResourceCost buildingCost)
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