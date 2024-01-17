using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class MagicTower : UpdateBuilding
    {
        public Transform FirePos;

        private Collider[] m_Target;
        private float m_AttackRadius = 100;

        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            m_Target = new Collider[1];
        }

        protected override void OnExecute()
        {
            if (Physics.OverlapSphereNonAlloc(FirePos.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
            {
                this.GetSystem<IBulletSystem>().GetBullet(BulletType.Magic)
                    .Set(2, 10, FirePos.position, m_Target[0].transform).Shoot();
            }
        }
    }
}