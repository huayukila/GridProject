using System.Collections.Generic;
using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    public class CannonTower : UpdateBuilding
    {
        [Header("Cannon Settings")] public Transform YRotationPoint; // Y軸回転ポイント
        public Transform XRotatioinPoint; // X軸回転ポイント
        public ParticleSystem FirePointParticle; // 発射点のパーティクル
        public float LockSpeed; // ロックスピード

        public float m_ElevationAngle; // 仰角
        private readonly float m_AttackRadius = 200; // 攻撃範囲
        private float m_ChangeDirDurationTime; // 方向変更持続時間
        private float m_CurrentYAngle; // 現在のY軸角度
        private int m_IdleStateYRotationDir; // アイドル状態のY軸回転方向
        private float m_StartTime; // 開始時間

        private TowerState m_State;
        private Collider[] m_Target; // ターゲット

        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
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
                    IdleRotation();
                    break;
                case TowerState.LockTarget:
                    LockOnTarget();
                    break;
                case TowerState.Reloading:
                    // リロード中なので、なんもしない
                    break;
            }
        }

        private void IdleRotation()
        {
            // アイドル状態の回転処理
            m_CurrentYAngle += m_IdleStateYRotationDir * Time.deltaTime * 50f;
            YRotationPoint.rotation = Quaternion.Slerp(YRotationPoint.rotation,
                Quaternion.Euler(0, m_CurrentYAngle, 0), 0.8f);

            if (Time.time - m_StartTime < m_ChangeDirDurationTime)
                return;

            m_IdleStateYRotationDir = Random.Range(-1, 2);
            m_StartTime = Time.time;
        }

        private void LockOnTarget()
        {
            // ターゲット方向の計算
            var targetDir = -(m_Target[0].transform.position - YRotationPoint.position).normalized;
            var dirHor = new Vector3(targetDir.x, 0, targetDir.z);

            // Y軸とX軸の回転
            RotateTowardsTarget(dirHor);

            // 射撃判定
            if (IsAimedAtTarget(dirHor)) PerformShooting();
        }

        private void RotateTowardsTarget(Vector3 dirHor)
        {
            // Y軸の回転
            YRotationPoint.rotation = Quaternion.Slerp(YRotationPoint.rotation,
                Quaternion.LookRotation(dirHor), 2f * Time.deltaTime);

            // X軸の回転
            var targetRotationX = Quaternion.Euler(m_ElevationAngle - 90, 0, 0);
            XRotatioinPoint.localRotation =
                Quaternion.Slerp(XRotatioinPoint.localRotation, targetRotationX, Time.deltaTime * 2f);
        }

        private bool IsAimedAtTarget(Vector3 dirHor)
        {
            // ターゲットへの照準判定
            return Vector3.Angle(new Vector3(YRotationPoint.forward.x, 0, YRotationPoint.forward.z), dirHor) < 10f;
        }

        private void PerformShooting()
        {
            // 発射処理
            FirePointParticle.Play();
            this.GetSystem<IBulletSystem>().GetBullet(BulletType.Cannon).Set(200, 10,
                m_Target[0].transform.position + Vector3.up * 100f, m_Target[0].transform).Shoot();

            //リロード
            m_State = TowerState.Reloading;
            m_Target[0] = null;
            ActionKit.Delay(5, () => { m_State = TowerState.Idle; }).Start(this);
        }

        protected override void OnExecute()
        {
            // 状態チェック
            if (m_State == TowerState.Reloading)
                return;

            // ターゲット検出
            DetectTarget();
        }

        private void DetectTarget()
        {
            // ターゲット検出処理
            if (Physics.OverlapSphereNonAlloc(YRotationPoint.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
                m_State = TowerState.LockTarget;
        }
    }
}