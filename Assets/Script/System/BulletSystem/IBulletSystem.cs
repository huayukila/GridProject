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
        private GameObject m_BulletPool; // 弾丸プールのGameObject
        private Dictionary<BulletType, SimpleObjectPool<GameObject>> m_BulletDic; // 弾丸プールの辞書

        // システムの初期化
        protected override void OnInit()
        {
            // 必要な初期化処理をここに追加
        }

        // 弾丸を取得する
        public BulletBase GetBullet(BulletType bulletType_)
        {
            if(m_BulletDic.TryGetValue(bulletType_, out SimpleObjectPool<GameObject> temp))
            {
                GameObject obj = temp.Allocate();
                return obj.GetComponent<BulletBase>();
            }
            return null;
        }

        // 弾丸をリサイクルする
        public void RecycleBullet(GameObject obj)
        {
            obj.transform.parent = m_BulletPool.transform;
            if(m_BulletDic.TryGetValue(obj.GetComponent<BulletBase>().BulletType, out SimpleObjectPool<GameObject> temp))
            {
                temp.Recycle(obj);
            }
        }

        // マネージャーによる初期化
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
                m_BulletDic.Add(bulletData.bulletType, temp);
            }
            Resources.UnloadAsset(m_BulletsData);
        }

        // デストラクター
        public void Deinit()
        {
            GameObject.Destroy(m_BulletPool);
            foreach (var pool in m_BulletDic.Values)
            {
                pool.Clear();
            }
            m_BulletDic.Clear();
        }
    }
}
