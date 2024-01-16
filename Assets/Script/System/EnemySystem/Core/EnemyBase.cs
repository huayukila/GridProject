// #define ISDEBUG

using Kit;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public interface IGetHurt
    {
        void GetDamage(int damage);
    }

    public enum EnemyState
    {
        Idle,
        Attack,
        Move,
        Trace,
        Dead
    }

    public class EnemyBase : BuildController, IGetHurt
    {
        public EnemyType Type;
        public float ClaimsRadius;
        public float AttackRadius;
        public int MaxHp;
        public float Speed;
        public float AttackCD;
        public float ClaimsCD;

        private float m_DurationTime;
        private float m_StartTime;
        protected Animator m_Animator;
        protected EnemyState m_State;

        protected int CurrentHp;
        Collider[] m_Target;


        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            CurrentHp = MaxHp;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_Target = new Collider[1];
        }

        private void OnEnable()
        {
            m_StartTime = Time.time;
            m_State = EnemyState.Move;
            m_Animator.Play("Goblin_run");
        }
#if ISDEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawSphere(transform.localPosition, AttackRadius);
            Gizmos.DrawSphere(transform.localPosition, ClaimsRadius);
        }
#endif


        private void Update()
        {
            switch (m_State)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Move:
                    transform.localPosition += transform.forward * (Speed * Time.deltaTime);

                    if (Time.time - m_StartTime < ClaimsCD)
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
                    transform.localPosition += transform.forward * (Speed * Time.deltaTime);

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
                        m_Animator.Play("Goblin_idle");
                    }

                    break;
                case EnemyState.Attack:
                    if (Physics.OverlapSphereNonAlloc(transform.localPosition, AttackRadius, m_Target,
                            LayerMask.GetMask("Building")) == 0)
                    {
                        transform.LookAt(new Vector3(50, transform.localPosition.y, 50));
                        m_State = EnemyState.Move;
                        m_Animator.Play("Goblin_run");
                    }

                    if (Time.time - m_StartTime > AttackCD)
                    {
                        m_Animator.Play("Goblin_attack");
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
            tag = Global.TARGET_STRING_ENEMY;
            CurrentHp = MaxHp;
            m_Target[0] = null;
            m_State = EnemyState.Move;
            gameObject.layer = 7;
            gameObject.SetActive(false);
        }

        protected virtual void OnAttack()
        {
        }


        public void GetDamage(int damage)
        {
            if (m_State == EnemyState.Dead)
                return;
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                tag = Global.TAG_STRING_DEAD;
                m_Animator.Play("Goblin_death");
                m_State = EnemyState.Dead;
                gameObject.layer = 0;
                this.SendEvent<EnemyGetKilledEvent>();
                //pool recycle
                ActionKit.Delay(3, () => this.GetSystem<IEnemySystem>().RecycleEnemy(gameObject)).Start(this);
            }
        }
    }
}