using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class MagicTower : UpdateBuilding
    {
        public Transform FirePos; // 発射位置

        private Collider[] m_Target; // ターゲットリスト
        private float m_AttackRadius = 100; // 攻撃範囲

        // 建物の初期化処理
        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            m_Target = new Collider[1]; // ターゲット配列を初期化
        }

        // 定期実行の処理
        protected override void OnExecute()
        {
            // 敵を検出
            if (Physics.OverlapSphereNonAlloc(FirePos.position, m_AttackRadius, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) <= 0) return;
            // 魔法の弾丸を取得し発射
            this.GetSystem<IBulletSystem>().GetBullet(BulletType.Magic)
                .Set(2, 10, FirePos.position, m_Target[0].transform).Shoot();
            m_Target[0] = null;
        }
    }
}