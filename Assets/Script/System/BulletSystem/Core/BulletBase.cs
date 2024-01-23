using UnityEngine;

namespace Framework.BuildProject
{
    // �e�̊�{�N���X
    public class BulletBase : BuildController
    {
        public BulletType BulletType; // �e�̎��
        protected int m_Damage; // �e�̃_���[�W
        protected float m_Speed; // �e�̑��x

        protected Vector3 m_StartPos; // �e�̏����ʒu
        protected Collider[] m_Target = new Collider[1]; // �^�[�Q�b�g�̃R���C�_�[
        protected Transform m_TargetTrans; // �^�[�Q�b�g�̃g�����X�t�H�[��

        // �X�V����
        private void Update()
        {
            OnUpdate();
        }

        // �e�̔��ˏ���
        public void Shoot()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        ///     �e�̐ݒ�֐�
        /// </summary>
        /// <param name="speed_">���x</param>
        /// <param name="damage_">�_���[�W</param>
        /// <param name="startPos_">�J�n�ʒu</param>
        /// <param name="targetTrans_">�^�[�Q�b�g�̃g�����X�t�H�[��</param>
        /// <returns>�ݒ肳�ꂽ�e�I�u�W�F�N�g</returns>
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

        // �e�̃��Z�b�g
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

        // �t���[�����̍X�V
        protected virtual void OnUpdate()
        {
            // �^�[�Q�b�g�ɏՓ˂������̃`�F�b�N
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, m_Target,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND, Global.TARGET_STRING_ENEMY)) > 0)
            {
                // �G�Ƀ_���[�W��^���鏈��
                if (m_Target[0].CompareTag(Global.TARGET_STRING_ENEMY))
                    m_Target[0].GetComponent<IGetHurt>().GetDamage(m_Damage);

                // �e�̍ė��p�܂��͔j��
                RecycleBullet();
            }
        }

        // �e�̍ė��p����
        protected void RecycleBullet()
        {
            this.GetSystem<IBulletSystem>().RecycleBullet(gameObject);
        }
    }
}