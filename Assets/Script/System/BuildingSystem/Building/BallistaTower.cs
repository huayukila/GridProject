using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public enum TowerState
    {
        Idle,
        LockTarget,
        Reloading
    }

    public class BallistaTower : UpdateBuilding
    {
        [Header("")] public Transform FirePoint;
        public Transform YRotationPoint;
        public Transform XRotationPoint;
        public Transform RootTrans;
        public Transform StringStartPoint;
        public Transform StringEndPoint;
        public Transform StringTrans;
        public Transform LockTrans;
        public float LoadSpeed;

        private Collider[] m_Target;
        private float m_AttackRadius = 50;
        private bool m_IsAmmoLoaded;
        private TowerState m_State;
        private readonly float m_LockSpeed = 0.8f;

        //ターゲットするとき、向き記録するため
        private float m_YDir;

        private float m_X;

        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            m_Target = new Collider[1];
        }

        private void Start()
        {
            m_WorkerNum = 1;
            m_Target = new Collider[1];
            m_State = TowerState.Idle;
            m_X = 0;
            m_YDir = 0;
        }


        protected override void OnUpdate()
        {
            Reload();
            switch (m_State)
            {
                case TowerState.Reloading:
                    if (!m_IsAmmoLoaded)
                        break;

                    if (m_Target[0] == null)
                    {
                        m_State = TowerState.Idle;
                        break;
                    }

                    m_State = TowerState.LockTarget;
                    break;

                case TowerState.Idle:
                    m_X += Time.deltaTime;
                    float rotAngles = Mathf.Sin(m_X) * 20;
                    YRotationPoint.rotation =
                        Quaternion.Lerp(YRotationPoint.rotation, Quaternion.Euler(0, rotAngles + m_YDir, 0), 0.8f);
                    break;

                case TowerState.LockTarget:
                    Vector3 targetDir = -(m_Target[0].transform.position - LockTrans.position);

                    Quaternion rot = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));

                    XRotationPoint.rotation = Quaternion.Lerp(XRotationPoint.rotation, rot, 3f * Time.deltaTime);

                    var rotationY = YRotationPoint.rotation;
                    rotationY =
                        Quaternion.Lerp(rotationY, Quaternion.LookRotation(targetDir),
                            3f * Time.deltaTime);

                    YRotationPoint.rotation = rotationY;

                    m_YDir = rotationY.eulerAngles.y;

                    float angle = Vector3.SignedAngle(FirePoint.up, m_Target[0].transform.position - FirePoint.position,
                        Vector3.up);

                    if (!m_Target[0].CompareTag(Global.TAG_STRING_DEAD))
                    {
                        if (Mathf.Abs(angle) < 3f && m_IsAmmoLoaded)
                        {
                            Fire();
                            m_State = TowerState.Reloading;
                        }
                    }
                    else
                    {
                        m_Target[0] = null;
                        m_State = TowerState.Idle;
                    }

                    break;
            }
        }

        protected override void OnExecute()
        {
            if (m_Target[0] != null)
                return;
            if (Physics.OverlapSphereNonAlloc(FirePoint.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
            {
                m_State = TowerState.LockTarget;
                m_X = 0;
            }
        }

        void Fire()
        {
            BulletBase bullet = this.GetSystem<IBulletSystem>().GetBullet(BulletType.Arrow);
            bullet.Set(100f, 10, FirePoint.position, m_Target[0].transform.position).Shoot();
            bullet.transform.rotation = FirePoint.rotation;
            m_IsAmmoLoaded = false;
            StringTrans.position = StringStartPoint.position;
            FirePoint.gameObject.SetActive(false);
        }

        void Reload()
        {
            if (m_IsAmmoLoaded) return;
            if (Vector3.Distance(StringTrans.position, StringEndPoint.position) < 0.1f)
            {
                m_IsAmmoLoaded = true;
                FirePoint.gameObject.SetActive(true);
                return;
            }

            StringTrans.Translate(Vector3.up * (LoadSpeed * Time.deltaTime), Space.Self);
            RootTrans.Rotate(Vector3.right, (LoadSpeed * Time.deltaTime) * 70);
        }
    }
}