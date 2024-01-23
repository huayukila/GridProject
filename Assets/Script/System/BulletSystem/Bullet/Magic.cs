using UnityEngine;

namespace Framework.BuildProject
{
    public class Magic : BulletBase
    {
        private float m_T;

        protected override void OnUpdate()
        {
            m_T += Time.deltaTime;
            transform.position = Utils.BezierCure(m_StartPos, m_TargetTrans, m_T * m_Speed);
            base.OnUpdate();
        }

        public override void ResetBulletObj()
        {
            base.ResetBulletObj();
            m_T = 0;
        }
    }
}