using System.Collections.Generic;
using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    public class CannonTower : UpdateBuilding
    {
        [Header("Cannon Settings")] public Transform YRotationPoint; // Y����]�|�C���g
        public Transform XRotatioinPoint; // X����]�|�C���g
        public ParticleSystem FirePointParticle; // ���˓_�̃p�[�e�B�N��
        public float LockSpeed; // ���b�N�X�s�[�h

        public float m_ElevationAngle; // �p
        private readonly float m_AttackRadius = 200; // �U���͈�
        private float m_ChangeDirDurationTime; // �����ύX��������
        private float m_CurrentYAngle; // ���݂�Y���p�x
        private int m_IdleStateYRotationDir; // �A�C�h����Ԃ�Y����]����
        private float m_StartTime; // �J�n����

        private TowerState m_State;
        private Collider[] m_Target; // �^�[�Q�b�g

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
                    // �����[�h���Ȃ̂ŁA�Ȃ�����Ȃ�
                    break;
            }
        }

        private void IdleRotation()
        {
            // �A�C�h����Ԃ̉�]����
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
            // �^�[�Q�b�g�����̌v�Z
            var targetDir = -(m_Target[0].transform.position - YRotationPoint.position).normalized;
            var dirHor = new Vector3(targetDir.x, 0, targetDir.z);

            // Y����X���̉�]
            RotateTowardsTarget(dirHor);

            // �ˌ�����
            if (IsAimedAtTarget(dirHor)) PerformShooting();
        }

        private void RotateTowardsTarget(Vector3 dirHor)
        {
            // Y���̉�]
            YRotationPoint.rotation = Quaternion.Slerp(YRotationPoint.rotation,
                Quaternion.LookRotation(dirHor), 2f * Time.deltaTime);

            // X���̉�]
            var targetRotationX = Quaternion.Euler(m_ElevationAngle - 90, 0, 0);
            XRotatioinPoint.localRotation =
                Quaternion.Slerp(XRotatioinPoint.localRotation, targetRotationX, Time.deltaTime * 2f);
        }

        private bool IsAimedAtTarget(Vector3 dirHor)
        {
            // �^�[�Q�b�g�ւ̏Ə�����
            return Vector3.Angle(new Vector3(YRotationPoint.forward.x, 0, YRotationPoint.forward.z), dirHor) < 10f;
        }

        private void PerformShooting()
        {
            // ���ˏ���
            FirePointParticle.Play();
            this.GetSystem<IBulletSystem>().GetBullet(BulletType.Cannon).Set(200, 10,
                m_Target[0].transform.position + Vector3.up * 100f, m_Target[0].transform).Shoot();

            //�����[�h
            m_State = TowerState.Reloading;
            m_Target[0] = null;
            ActionKit.Delay(5, () => { m_State = TowerState.Idle; }).Start(this);
        }

        protected override void OnExecute()
        {
            // ��ԃ`�F�b�N
            if (m_State == TowerState.Reloading)
                return;

            // �^�[�Q�b�g���o
            DetectTarget();
        }

        private void DetectTarget()
        {
            // �^�[�Q�b�g���o����
            if (Physics.OverlapSphereNonAlloc(YRotationPoint.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
                m_State = TowerState.LockTarget;
        }
    }
}