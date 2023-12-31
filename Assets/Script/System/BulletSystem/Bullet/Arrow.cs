namespace Framework.BuildProject
{
    public class Arrow : BulletBase
    {
        private void FixedUpdate()
        {
            if(m_Target==null)
                return;
            transform.LookAt(m_Target);
            transform.Translate(transform.forward * m_Speed);
        }
    }
}