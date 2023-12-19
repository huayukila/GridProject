using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public class BulletBase : BuildController
    {
        public BulletType m_BulletType;
        protected Transform m_Target;
        protected float m_Speed;
        public int m_Damage;
        /// <summary>
        /// �e���ˊ֐�
        /// </summary>
        /// <param name="pos_">���˂̈ʒu</param>
        /// <param name="target_">�ڕW</param>
        /// <param name="speed_">�e�̔�s�X�s�[�h</param>
        public void Shoot(Vector3 pos_, Transform target_)
        {
            transform.position = pos_;
            m_Target = target_;
            gameObject.SetActive(true);
        }

        public void Set(float speed_,int damage_)
        {
            m_Speed = speed_;
            m_Damage = damage_;
        }
        public void ResetObj()
        {
            m_Target = null;
            transform.position = Vector3.zero;
            m_Damage = 0;
            m_Speed = 0.0f;
            gameObject.SetActive(false);
        }


        private void OnTriggerEnter(Collider other_)
        {
            if (!other_.CompareTag("Enemy")) return;
            Debug.Log("hitEnemy");
            other_.GetComponent<iGetHurt>().GetDamage(m_Damage);
            this.GetSystem<IBulletSystem>().RecycleBullet(gameObject);
        }
    }
}