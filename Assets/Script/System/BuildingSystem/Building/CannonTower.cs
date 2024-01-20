using Kit;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public class CannonTower : UpdateBuilding
    {
        [Header("")] public Transform YRotationPoint;
        public Transform XRotatioinPoint;
        public ParticleSystem FirePointParticle;
        public float LockSpeed;

        //cannon
        public float m_ElevationAngle;

        private TowerState m_State;
        private int m_IdleStateYRotationDir;
        private float m_StartTime;
        private float m_ChangeDirDurationTime;
        private float m_CurrentYAngle;
        private float m_AttackRadius = 200;
        private Collider[] m_Target;

        private void Start()
        {
            m_Target = new Collider[1];
            m_State = TowerState.Idle;
            m_ChangeDirDurationTime = 3f;
            m_StartTime = Time.time;
            m_IdleStateYRotationDir = 1;
            m_CurrentYAngle = 0;
        }

        protected override void OnUpdate()
        {
            switch (m_State)
            {
                case TowerState.Idle:
                    m_CurrentYAngle += m_IdleStateYRotationDir * Time.deltaTime * 50f;
                    YRotationPoint.rotation = Quaternion.Slerp(YRotationPoint.rotation,
                        Quaternion.Euler(0, m_CurrentYAngle, 0), 0.8f);

                    if (Time.time - m_StartTime < m_ChangeDirDurationTime)
                        break;
                    m_IdleStateYRotationDir = Random.Range(-1, 2);
                    m_StartTime = Time.time;
                    break;
                case TowerState.LockTarget:
                    if (m_Target[0].CompareTag(Global.TAG_STRING_DEAD))
                    {
                        m_State = TowerState.Idle;
                        break;
                    }

                    Vector3 targetDir = -(m_Target[0].transform.position - YRotationPoint.position).normalized;
                    Vector3 dirHor = new Vector3(targetDir.x, 0, targetDir.z);

                    YRotationPoint.rotation = Quaternion.Slerp(YRotationPoint.rotation,
                        Quaternion.LookRotation(dirHor), 2f * Time.deltaTime);

                    Quaternion targetRotationX = Quaternion.Euler(m_ElevationAngle - 90, 0, 0);
                    XRotatioinPoint.localRotation =
                        Quaternion.Slerp(XRotatioinPoint.localRotation, targetRotationX, Time.deltaTime * 2f);

                    if (Vector3.Angle(new Vector3(YRotationPoint.forward.x, 0, YRotationPoint.forward.z),
                            dirHor) < 5f)
                    {
                        FirePointParticle.Play();
                        this.GetSystem<IBulletSystem>().GetBullet(BulletType.Cannon).Set(200, 10,
                            m_Target[0].transform.position + Vector3.up * 100f, m_Target[0].transform).Shoot();
                        
                        m_State = TowerState.Reloading;
                        ActionKit.Delay(5, () => { m_State = TowerState.Idle; }).Start(this);
                    }

                    break;
                case TowerState.Reloading:
                    break;
            }
        }

        protected override void OnExecute()
        {
            if (m_State == TowerState.Reloading)
                return;
            if (Physics.OverlapSphereNonAlloc(YRotationPoint.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
            {
                m_State = TowerState.LockTarget;
            }
        }
    }
}