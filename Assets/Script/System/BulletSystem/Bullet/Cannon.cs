using UnityEngine;

namespace Framework.BuildProject
{
    // �uCannon�v�N���X�́uBulletBase�v���p�����Ă��܂�
    public class Cannon : BulletBase
    {
        public GameObject Explosion; // �����G�t�F�N�g�̃v���n�u
        private RaycastHit[] m_hitInfo; // ���C�L���X�g�̃q�b�g���
        private Collider[] m_Targets; // �^�[�Q�b�g�̃R���C�_�[�̔z��

        // ����������
        private void Start()
        {
            m_hitInfo = new RaycastHit[1];
            m_Targets = new Collider[10];
        }

        // �t���[�����̍X�V����
        protected override void OnUpdate()
        {
            // �e�̑O�i
            transform.Translate(transform.forward * (Time.deltaTime * m_Speed), Space.World);

            // �n�ʂɓ����������̊m�F
            if (Physics.RaycastNonAlloc(transform.position, transform.forward, m_hitInfo, m_Speed * Time.deltaTime + 1,
                    LayerMask.GetMask(Global.TARGET_STRING_GROUND)) > 0)
            {
                // �G�ɑ΂���͈͍U���̊m�F
                if (Physics.OverlapSphereNonAlloc(transform.position, 15, m_Targets,
                        LayerMask.GetMask(Global.TARGET_STRING_ENEMY)) > 0)
                {
                    // �G�Ƀ_���[�W��^����
                    foreach (var target in m_Targets)
                    {
                        if (target == null)
                            break;
                        target.GetComponent<IGetHurt>().GetDamage(m_Damage);
                    }
                }

                // �����G�t�F�N�g�̐����ƒe�̉��
                Instantiate(Explosion, m_hitInfo[0].point, Quaternion.identity);
                RecycleBullet();
            }
        }

        // �e�̃��Z�b�g����
        public override void ResetBulletObj()
        {
            base.ResetBulletObj();
            // �^�[�Q�b�g�z��̃��Z�b�g
            for (int i = 0; i < m_Targets.Length; i++)
            {
                m_Targets[i] = null;
            }
        }
    }
}