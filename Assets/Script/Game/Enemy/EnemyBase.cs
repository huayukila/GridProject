using UnityEngine;

namespace Framework.BuildProject
{
    public interface iGetHurt
    {
        bool GetDamage(int damage);
    }

    public enum EnemyState
    {
        Idled,
        Attack,
        Move,
        Dead
    }

    public class EnemyBase : BuildController, iGetHurt
    {
        public EnemyType m_Type;

        protected Transform m_Target;

        protected int m_Hp = 10;

        protected float m_Speed;

        protected float m_AttackCD;

        
        private float m_DurationTime;
        private float m_StartTime;

        protected EnemyState m_State;
        // Start is called before the first frame update
        void Start()
        {
            m_Speed = 0.1f;
        }

        private void OnEnable()
        {
            m_StartTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (Time.time - m_StartTime < m_AttackCD)
                return;
            m_StartTime = Time.time;
            OnAttack();
            transform.localPosition += transform.forward * m_Speed;
        }

        public void ResetObj()
        {
            m_Hp = 1;
            m_Target = null;
            m_Speed = 0f;
            m_AttackCD = 0;

            gameObject.SetActive(false);
        }

        protected void OnAttack()
        {
            
        }

        public bool GetDamage(int damage)
        {
            m_Hp -= damage;
            if (m_Hp <= 0)
            {
                //pool recycle
                Debug.Log("is dead");
                this.GetSystem<IEnemySystem>().RecycleEnemy(gameObject);
                return true;
            }

            return false;
        }
    }
}