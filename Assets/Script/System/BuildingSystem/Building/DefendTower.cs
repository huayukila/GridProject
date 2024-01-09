using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class DefendTower : UpdateBuilding
    {
        private Vector3 m_FirePoint;
        private Collider[] m_TargetPos;
        private float m_AttackRadius = 25;

        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_,
            GameObject obj)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_, obj);
            Vector3 tempPos = obj.transform.GetChild(0).transform.position;
            m_FirePoint = new Vector3(tempPos.x, 12, tempPos.z);
            m_TargetPos = new Collider[1];
            m_WorkerNum = 2;
        }

        protected override void OnUpdate()
        {
            if (Physics.OverlapSphereNonAlloc(m_GameObj.transform.position, m_AttackRadius, m_TargetPos,
                    LayerMask.GetMask("Enemy")) > 0)
            {
                this.GetSystem<IBulletSystem>().GetBullet(BulletType.Arrow).Set(1f, 10, m_FirePoint,
                    m_TargetPos[0].transform.position).Shoot();
            }
        }
    }
}