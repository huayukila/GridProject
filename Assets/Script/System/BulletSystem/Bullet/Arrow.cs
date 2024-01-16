using UnityEngine;

namespace Framework.BuildProject
{
    public class Arrow : BulletBase
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            transform.Translate(transform.up*(m_Speed*Time.deltaTime), Space.World);
        }
    }
}