using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBulletSystem : ISystem
    {
        GameObject GetBullet(BulletType bulletType_);
        void RecycleBullet(GameObject obj);
    }

    public class BulletSystem : AbstractSystem, IBulletSystem
    {
        private Dictionary<BulletType, SimpleObjectPool<GameObject>> m_BulletDic;
        private SimpleObjectPool<GameObject> m_ArrowPool;
        private BulletDataBase m_BulletsData;

        protected override void OnInit()
        {
            m_BulletsData = Resources.Load<BulletDataBase>("BulletDatabase");
            m_BulletDic = new Dictionary<BulletType, SimpleObjectPool<GameObject>>();
            foreach (var bulletData in m_BulletsData.m_BulletDataList)
            {
                SimpleObjectPool<GameObject> temp = new SimpleObjectPool<GameObject>(() =>
                {
                    GameObject obj = GameObject.Instantiate(bulletData.bulletPrefab, Vector3.zero, Quaternion.identity);
                    obj.SetActive(false);
                    return obj;
                }, obj => { obj.GetComponent<BulletBase>().Reset(); }, 10);
                m_BulletDic.Add(bulletData.bulletTyped, temp);
            }
        }

        public GameObject GetBullet(BulletType bulletType_)
        {
            m_BulletDic.TryGetValue(bulletType_, out SimpleObjectPool<GameObject> temp);
            return temp.Allocate();
        }

        public void RecycleBullet(GameObject obj)
        {
            m_BulletDic.TryGetValue(obj.GetComponent<BulletBase>().m_BulletType, out SimpleObjectPool<GameObject> temp);
            temp.Recycle(obj);
        }
    }
}