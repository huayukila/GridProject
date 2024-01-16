using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBulletSystem : ISystem
    {
        BulletBase GetBullet(BulletType bulletType_);
        void RecycleBullet(GameObject obj);
        void InitByManager();
        void Deinit();
    }

    public class BulletSystem : AbstractSystem, IBulletSystem
    {
        private GameObject m_BulletPool;
        private Dictionary<BulletType, SimpleObjectPool<GameObject>> m_BulletDic;

        protected override void OnInit()
        {
        }

        public BulletBase GetBullet(BulletType bulletType_)
        {
            m_BulletDic.TryGetValue(bulletType_, out SimpleObjectPool<GameObject> temp);
            GameObject obj = temp.Allocate();
            return obj.GetComponent<BulletBase>();
        }

        public void RecycleBullet(GameObject obj)
        {
            obj.transform.parent = m_BulletPool.transform;
            m_BulletDic.TryGetValue(obj.GetComponent<BulletBase>().m_BulletType, out SimpleObjectPool<GameObject> temp);
            temp.Recycle(obj);
        }

        public void InitByManager()
        {
            m_BulletPool = new GameObject("BulletPool");
            m_BulletPool.transform.position = Vector3.zero;
            BulletDataBase m_BulletsData = Resources.Load<BulletDataBase>("BulletDatabase");
            m_BulletDic = new Dictionary<BulletType, SimpleObjectPool<GameObject>>();
            foreach (var bulletData in m_BulletsData.BulletDataList)
            {
                SimpleObjectPool<GameObject> temp = new SimpleObjectPool<GameObject>(() =>
                {
                    GameObject obj = GameObject.Instantiate(bulletData.bulletPrefab, Vector3.zero, Quaternion.identity);
                    obj.transform.parent = m_BulletPool.transform;
                    obj.SetActive(false);
                    return obj;
                }, obj => { obj.GetComponent<BulletBase>().ResetBulletObj(); }, 10);
                m_BulletDic.Add(bulletData.bulletTyped, temp);
            }

            Resources.UnloadAsset(m_BulletsData);
        }

        public void Deinit()
        {
            m_BulletPool = null;
            foreach (var pool in m_BulletDic.Values)
            {
                pool.Clear();
            }

            m_BulletDic.Clear();
        }
    }
}