using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.BuildProject
{
    public class BulletBase : BuildController
    {
        public BulletType m_BulletType;
        protected Transform m_Target;
        protected float m_Speed;

        /// <summary>
        /// 弾発射関数
        /// </summary>
        /// <param name="pos_">発射の位置</param>
        /// <param name="target_">目標</param>
        /// <param name="speed_">弾の飛行スピード</param>
        public void Shoot(Vector3 pos_, Transform target_, float speed_)
        {
            transform.position = pos_;
            m_Target = target_;
            m_Speed = speed_;
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
            m_Target = null;
            m_Speed = 0.0f;
            gameObject.SetActive(false);
        }

        
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Enemy"))
            {
                this.GetSystem<IBulletSystem>().RecycleBullet(gameObject);
            }
        }
    }
}