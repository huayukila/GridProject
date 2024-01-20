using UnityEngine;

namespace Framework.BuildProject
{
    // 弾の基本クラス
    public class BulletBase : BuildController
    {
        public BulletType BulletType; // 弾の種類
        protected float m_Speed; // 弾の速度
        protected int m_Damage; // 弾のダメージ
        protected Collider[] m_Target = new Collider[1]; // ターゲットのコライダー

        protected Vector3 m_StartPos; // 弾の初期位置
        protected Transform m_TargetTrans; // ターゲットのトランスフォーム

        // 弾の発射処理
        public void Shoot()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 弾の設定関数
        /// </summary>
        /// <param name="speed_">速度</param>
        /// <param name="damage_">ダメージ</param>
        /// <param name="startPos_">開始位置</param>
        /// <param name="targetTrans_">ターゲットのトランスフォーム</param>
        /// <returns>設定された弾オブジェクト</returns>
        public BulletBase Set(float speed_, int damage_, Vector3 startPos_, Transform targetTrans_)
        {
            m_StartPos = startPos_;
            m_TargetTrans = targetTrans_;
            transform.localPosition = m_StartPos;
            transform.LookAt(m_TargetTrans.position);
            m_Speed = speed_;
            m_Damage = damage_;
            return this;
        }

        // 弾のリセット
        public virtual void ResetBulletObj()
        {
            m_StartPos = Vector3.zero;
            m_TargetTrans = null;
            transform.position = Vector3.zero;
            m_Damage = 0;
            m_Speed = 0.0f;
            gameObject.SetActive(false);
            m_Target[0] = null;
        }

        // 更新処理
        private void Update()
        {
            OnUpdate();
        }

        // フレーム毎の更新
        protected virtual void OnUpdate()
        {
            // ターゲットに衝突したかのチェック
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND, Global.TARGET_STRING_ENEMY)) > 0)
            {
                // 敵にダメージを与える処理
                if (m_Target[0].CompareTag(Global.TARGET_STRING_ENEMY))
                {
                    m_Target[0].GetComponent<IGetHurt>().GetDamage(m_Damage);
                }

                // 弾の再利用または破棄
                RecycleBullet();
            }
        }

        // 弾の再利用処理
        protected void RecycleBullet()
        {
            this.GetSystem<IBulletSystem>().RecycleBullet(gameObject);
        }
    }
}
