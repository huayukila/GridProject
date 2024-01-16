using UnityEngine;

namespace Framework.BuildProject
{
    public class BulletBase : BuildController
    {
        public BulletType m_BulletType;
        protected Vector3 m_TargetPos;
        protected float m_Speed;
        protected int m_Damage;
        protected Collider[] m_Target = new Collider[1];

        public void Shoot()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// íeê›íuä÷êî
        /// </summary>
        /// <param name="speed_"></param>
        /// <param name="damage_"></param>
        /// <param name="pos_"></param>
        /// <param name="targetPos_"></param>
        /// <returns></returns>
        public BulletBase Set(float speed_, int damage_, Vector3 pos_, Vector3 targetPos_)
        {
            transform.localPosition = pos_;
            m_TargetPos = targetPos_;
            transform.LookAt(m_TargetPos);
            m_Speed = speed_;
            m_Damage = damage_;
            return this;
        }

        public void ResetBulletObj()
        {
            m_TargetPos = Vector3.zero;
            transform.position = Vector3.zero;
            m_Damage = 0;
            m_Speed = 0.0f;
            gameObject.SetActive(false);
            m_Target[0] = null;
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            if (m_TargetPos == Vector3.zero)
                return;
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND, Global.TARGET_STRING_ENEMY)) > 0)
            {
                if (m_Target[0].CompareTag(Global.TARGET_STRING_ENEMY))
                {
                    m_Target[0].GetComponent<IGetHurt>().GetDamage(m_Damage);
                }

                RecycleBullet();
            }
        }

        protected void RecycleBullet()
        {
            this.GetSystem<IBulletSystem>().RecycleBullet(gameObject);
        }
    }
}