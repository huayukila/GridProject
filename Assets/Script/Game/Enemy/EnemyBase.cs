using UnityEngine;

namespace Framework.BuildProject
{
    public interface iGetHurt
    {
        bool GetDamage(int damage);
    }

    public class EnemyBase : BuildController, iGetHurt
    {
        public EnemyType m_Type;

        protected Transform m_Target;

        protected int m_Hp = 10;

        protected float m_Speed;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ResetObj()
        {
            m_Hp = 10;
            m_Target = null;
            m_Speed = 0f;
            gameObject.SetActive(false);
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