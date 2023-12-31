// #define ISDEBUG

using UnityEngine;

namespace Framework.BuildProject
{
    public interface iGetHurt
    {
        bool GetDamage(int damage);
    }

    public enum EnemyState
    {
        Idle,
        Attack,
        Move,
        Trace,
        Dead
    }

    public class EnemyBase : BuildController, iGetHurt
    {
        public EnemyType Type;
        public float ClaimsRadius;
        public float AttackRadius;

        protected int m_Hp = 10;
        protected float m_Speed;
        protected float m_AttackCD;
        protected float m_ClaimsCD;
        private float m_DurationTime;
        private float m_StartTime;
        protected Animator m_Animator;
        protected EnemyState m_State;

        Collider[] m_Target;

        // Start is called before the first frame update
        void Start()
        {
            m_Target = new Collider[1];
            m_Speed = 0.1f;
            m_ClaimsCD = 1f;
            m_AttackCD = 2.0f;
        }

        private void OnEnable()
        {
            m_StartTime = Time.time;
            m_State = EnemyState.Move;
        }
#if ISDEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawSphere(transform.localPosition, AttackRadius);
            Gizmos.DrawSphere(transform.localPosition, ClaimsRadius);
        }
#endif


        private void FixedUpdate()
        {
            switch (m_State)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Move:
                    transform.localPosition += transform.forward * m_Speed;
                    //í«ê’
                    if (Time.time - m_StartTime < m_ClaimsCD)
                        return;
                    if (Physics.OverlapSphereNonAlloc(transform.localPosition, ClaimsRadius, m_Target,
                            LayerMask.GetMask("Building")) > 0)
                    {
                        transform.LookAt(new Vector3(
                            m_Target[0].transform.position.x,
                            transform.localPosition.y,
                            m_Target[0].transform.position.z));
                        m_State = EnemyState.Trace;
                    }

                    break;
                case EnemyState.Trace:
                    transform.localPosition += transform.forward * m_Speed;

                    if (Physics.OverlapSphereNonAlloc(transform.localPosition, ClaimsRadius, m_Target,
                            LayerMask.GetMask("Building")) == 0)
                    {
                        transform.LookAt(new Vector3(50, transform.localPosition.y, 50));
                        m_State = EnemyState.Move;
                    }
                    else
                    {
                        transform.LookAt(new Vector3(
                            m_Target[0].transform.position.x,
                            transform.localPosition.y,
                            m_Target[0].transform.position.z));
                    }

                    if (Physics.OverlapSphereNonAlloc(transform.localPosition, AttackRadius, m_Target,
                            LayerMask.GetMask("Building")) > 0)
                    {
                        m_State = EnemyState.Attack;
                        m_StartTime = Time.time;
                    }

                    break;
                case EnemyState.Attack:
                    if (Physics.OverlapSphereNonAlloc(transform.localPosition, AttackRadius, m_Target,
                            LayerMask.GetMask("Building")) == 0)
                    {
                        transform.LookAt(new Vector3(50, transform.localPosition.y, 50));
                        m_State = EnemyState.Move;
                    }

                    if (Time.time - m_StartTime > m_AttackCD)
                    {
                        m_StartTime = Time.time;
                        OnAttack();
                    }

                    break;
                case EnemyState.Dead:
                    break;
            }
        }

        public void ResetObj()
        {
            m_Hp = 1;
            m_Target[0]=null;
            m_Speed = 0f;
            m_AttackCD = 0;

            gameObject.SetActive(false);
        }

        protected virtual void OnAttack()
        {
            Debug.Log("attack");
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