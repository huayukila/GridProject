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
        private Dictionary<BulletType, SimpleObjectPool<GameObject>> m_BulletDic; // �e�ۃv�[���̎���
        private GameObject m_BulletPool; // �e�ۃv�[����GameObject

        // �e�ۂ��擾����
        public BulletBase GetBullet(BulletType bulletType_)
        {
            if (m_BulletDic.TryGetValue(bulletType_, out var temp))
            {
                var obj = temp.Allocate();
                return obj.GetComponent<BulletBase>();
            }

            return null;
        }

        // �e�ۂ����T�C�N������
        public void RecycleBullet(GameObject obj)
        {
            obj.transform.parent = m_BulletPool.transform;
            if (m_BulletDic.TryGetValue
                (
                    obj.GetComponent<BulletBase>().BulletType, 
                    out var temp)
                )
                temp.Recycle(obj);
        }

        // �}�l�[�W���[�ɂ�鏉����
        public void InitByManager()
        {
            m_BulletPool = new GameObject("BulletPool");
            m_BulletPool.transform.position = Vector3.zero;
            var m_BulletsData = Resources.Load<BulletDataBase>("BulletDatabase");
            m_BulletDic = new Dictionary<BulletType, SimpleObjectPool<GameObject>>();
            foreach (var bulletData in m_BulletsData.BulletDataList)
            {
                var temp = new SimpleObjectPool<GameObject>(() =>
                {
                    var obj = Object.Instantiate(bulletData.bulletPrefab, Vector3.zero, Quaternion.identity);
                    obj.transform.parent = m_BulletPool.transform;
                    obj.SetActive(false);
                    return obj;
                }, obj =>
                {
                    obj.GetComponent<BulletBase>().ResetBulletObj();
                }, 10);
                m_BulletDic.Add(bulletData.bulletType, temp);
            }

            Resources.UnloadAsset(m_BulletsData);
        }

        // �f�X�g���N�^�[
        public void Deinit()
        {
            Object.Destroy(m_BulletPool);
            foreach (var pool in m_BulletDic.Values) pool.Clear();
            m_BulletDic.Clear();
        }

        // �V�X�e���̏�����
        protected override void OnInit()
        {
            // �K�v�ȏ����������������ɒǉ�
        }
    }
}