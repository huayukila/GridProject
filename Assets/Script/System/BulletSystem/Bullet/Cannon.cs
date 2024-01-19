using UnityEngine;

namespace Framework.BuildProject
{
    public class Cannon : BulletBase
    {
        public GameObject Explosion;
        private RaycastHit[] hitInfo;
        private Collider[] m_Targets;

        private void Start()
        {
            hitInfo = new RaycastHit[1];
            m_Targets = new Collider[10];
        }

        protected override void OnUpdate()
        {
            transform.Translate(transform.forward * (Time.deltaTime * m_Speed), Space.World);

            if (Physics.RaycastNonAlloc(transform.position, transform.forward, hitInfo, m_Speed * Time.deltaTime + 1,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND)) > 0)
            {
                if (Physics.OverlapSphereNonAlloc(transform.position, 15, m_Targets,
                        LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
                {
                    foreach (var target in m_Targets)
                    {
                        if (target == null)
                            break;
                        target.GetComponent<IGetHurt>().GetDamage(m_Damage);
                    }
                }

                Instantiate(Explosion, hitInfo[0].point, Quaternion.identity);
                RecycleBullet();
            }
        }

        public override void ResetBulletObj()
        {
            base.ResetBulletObj();
            for (int i = 0; i < m_Targets.Length; i++)
            {
                m_Targets[i] = null;
            }
        }
    }
}