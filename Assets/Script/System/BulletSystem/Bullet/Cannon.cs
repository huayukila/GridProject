using UnityEngine;

namespace Framework.BuildProject
{
    // 「Cannon」クラスは「BulletBase」を継承しています
    public class Cannon : BulletBase
    {
        public GameObject Explosion; // 爆発エフェクトのプレハブ
        private RaycastHit[] m_hitInfo; // レイキャストのヒット情報
        private Collider[] m_Targets; // ターゲットのコライダーの配列

        // 初期化処理
        private void Start()
        {
            m_hitInfo = new RaycastHit[1];
            m_Targets = new Collider[10];
        }

        // フレーム毎の更新処理
        protected override void OnUpdate()
        {
            // 弾の前進
            transform.Translate(transform.forward * (Time.deltaTime * m_Speed), Space.World);

            // 地面に当たったかの確認
            if (Physics.RaycastNonAlloc(transform.position, transform.forward, m_hitInfo, m_Speed * Time.deltaTime + 1,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND)) > 0)
            {
                // 敵に対する範囲攻撃の確認
                if (Physics.OverlapSphereNonAlloc(transform.position, 15, m_Targets,
                        LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
                {
                    // 敵にダメージを与える
                    foreach (var target in m_Targets)
                    {
                        if (target == null)
                            break;
                        target.GetComponent<IGetHurt>().GetDamage(m_Damage);
                    }
                }

                // 爆発エフェクトの生成と弾の回収
                Instantiate(Explosion, m_hitInfo[0].point, Quaternion.identity);
                RecycleBullet();
            }
        }

        // 弾のリセット処理
        public override void ResetBulletObj()
        {
            base.ResetBulletObj();
            // ターゲット配列のリセット
            for (int i = 0; i < m_Targets.Length; i++)
            {
                m_Targets[i] = null;
            }
        }
    }
}