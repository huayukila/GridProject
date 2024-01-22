using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    public class Goblin : EnemyBase
    {
        protected override void OnAttack()
        {
            ActionKit.Delay(0.5f, () =>
                {
                    RaycastHit[] m_HitInfo = new RaycastHit[1];
                    if (Physics.RaycastNonAlloc(transform.position, transform.forward, m_HitInfo, 10f,
                            LayerMask.GetMask(Global.TARGET_STRING_BUILDING)) > 0)
                    {
                        m_HitInfo[0].transform.parent.GetComponent<IGetHurt>().GetDamage(Damage);
                    }
                })
                .Start(this);
        }
    }
}