using UnityEngine;

namespace Framework.BuildProject
{
    public class Arrow : BulletBase
    {
        private void FixedUpdate()
        {
            if (m_TargetPos == Vector3.zero)
                return;
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, m_Target, LayerMask.GetMask("Enemy")) > 0)
            {
                m_Target[0].GetComponent<IGetHurt>().GetDamage(m_Damage);
                RecycleBullet();
            }
            transform.localPosition += transform.forward * m_Speed;
        }
    }
}