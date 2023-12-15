using UnityEngine;

namespace Framework.BuildProject
{
    public class Arrow : BulletBase
    {
        private void FixedUpdate()
        {
            transform.LookAt(m_Target);
            transform.Translate(transform.forward * m_Speed);
        }
    }
}