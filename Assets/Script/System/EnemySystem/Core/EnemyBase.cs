using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    public enum EnemyState
    {
        Idle,
        Attack,
        Move,
        Trace,
        Dead
    }

    public interface IGetHurt
    {
        void GetDamage(int damage);
    }

    public class EnemyBase : BuildController, IGetHurt
    {
        private static readonly int m_AnimRun = Animator.StringToHash("Goblin_run");
        private static readonly int m_AnimIdle = Animator.StringToHash("Goblin_idle");
        private static readonly int m_AnimAttack = Animator.StringToHash("Goblin_attack");
        private static readonly int m_AnimDeath = Animator.StringToHash("Goblin_death");
        public EnemyType Type; // 敵のタイプ
        public float ClaimsRadius; // 攻撃範囲
        public float AttackRadius; // 攻撃範囲
        public int MaxHp; // 最大HP
        public float Speed; // 移動速度
        public float AttackCD; // 攻撃クールダウン
        public float ClaimsCD; // 攻撃クールダウン
        public int Damage; //攻撃ダメージ
        private Animator m_Animator;
        private Collider m_Collider;
        private int m_CurrentHp;

        private float m_DurationTime;
        private int m_LayerMask; // レイヤーマスクをキャッシュ
        private float m_StartTime;
        private EnemyState m_State;
        private Collider[] m_Target;

        private void Awake()
        {
            m_Collider = GetComponent<SphereCollider>();
            m_Animator = GetComponent<Animator>();
            m_CurrentHp = MaxHp;
            m_Target = new Collider[1];
            m_LayerMask = LayerMask.GetMask(Global.TARGET_STRING_BUILDING);
        }

        private void Update()
        {
            switch (m_State)
            {
                case EnemyState.Idle:
                    // アイドル状態の処理
                    break;
                case EnemyState.Move:
                    Move();
                    CheckForClaim();
                    break;
                case EnemyState.Trace:
                    Trace();
                    CheckForAttack();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Dead:
                    // 死亡状態の処理
                    break;
            }
        }

        private void OnEnable()
        {
            m_StartTime = Time.time;
            m_State = EnemyState.Move;
            m_Animator.Play(m_AnimRun);
        }

        public void GetDamage(int damage)
        {
            if (m_State == EnemyState.Dead) return;
            m_CurrentHp -= damage;
            if (m_CurrentHp <= 0)
            {
                m_State = EnemyState.Dead;
                m_Collider.enabled = false;
                m_Animator.Play(m_AnimDeath);
                // 死亡処理
                this.SendEvent<EnemyGetKilledEvent>();
                ActionKit.Delay(3, () => this.GetSystem<IEnemySystem>().RecycleEnemy(gameObject)).Start(this);
            }
        }

        private void Move()
        {
            transform.localPosition += transform.forward * (Speed * Time.deltaTime);
        }

        private void CheckForClaim()
        {
            if (Time.time - m_StartTime < ClaimsCD) return;
            if (Physics.OverlapSphereNonAlloc(transform.localPosition, ClaimsRadius, m_Target, m_LayerMask) > 0)
            {
                UpdateRotation(m_Target[0].transform.position);
                m_State = EnemyState.Trace;
            }
        }

        private void Trace()
        {
            transform.localPosition += transform.forward * (Speed * Time.deltaTime);
            if (m_Target[0] == null ||
                Physics.OverlapSphereNonAlloc(transform.localPosition, ClaimsRadius, m_Target, m_LayerMask) == 0)
            {
                ResetToMoveState();
                return;
            }

            UpdateRotation(m_Target[0].transform.position);
        }

        private void CheckForAttack()
        {
            if (Physics.OverlapSphereNonAlloc(transform.localPosition, AttackRadius, m_Target, m_LayerMask) > 0)
            {
                m_State = EnemyState.Attack;
                m_StartTime = Time.time;
                m_Animator.Play(m_AnimIdle);
            }
        }

        private void Attack()
        {
            if (Physics.OverlapSphereNonAlloc(transform.localPosition, AttackRadius, m_Target, m_LayerMask) == 0)
            {
                ResetToMoveState();
                return;
            }

            if (Time.time - m_StartTime > AttackCD)
            {
                m_Animator.Play(m_AnimAttack);
                m_StartTime = Time.time;
                OnAttack();
            }
        }

        private void ResetToMoveState()
        {
            transform.LookAt(new Vector3(50, transform.localPosition.y, 50));
            m_State = EnemyState.Move;
            m_Animator.Play(m_AnimRun);
        }

        private void UpdateRotation(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        protected virtual void OnAttack()
        {
            // 攻撃処理
        }

        // リセット処理
        public void ResetObj()
        {
            m_Collider.enabled = true;
            m_CurrentHp = MaxHp;
            m_Target[0] = null;
            m_State = EnemyState.Move;
            gameObject.SetActive(false);
        }
    }
}