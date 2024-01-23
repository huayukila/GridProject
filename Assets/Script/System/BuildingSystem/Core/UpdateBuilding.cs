using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class UpdateBuilding : BuildingBase
    {
        private float m_DurationTime; // �Ō�̍X�V����̌o�ߎ���
        private int m_UpdateTime; // ����?�ʁC�X�V���Ԃ̐ݒ�
        private bool m_IsWorking => WorkerNum > 0; // �J���҂����邩�ǂ���

        private void Update()
        {
            // �J���҂����Ȃ��ꍇ�͏������X�L�b�v
            if (!m_IsWorking)
            {
                m_DurationTime = Time.time;
                return;
            }

            OnUpdate();
            // �w��̍X�V���Ԃ��o�߂��Ă��Ȃ��ꍇ�͏������X�L�b�v
            if (Time.time - m_DurationTime < m_UpdateTime) return;
            m_DurationTime = Time.time;
            OnExecute();
        }

        // �X�V���̏����i�I�[�o�[���C�h�\�j
        protected virtual void OnUpdate()
        {
            // �T�u�N���X�ŋ�̓I�ȏ������`
        }

        // ������s�̏����i�I�[�o�[���C�h�\�j
        protected virtual void OnExecute()
        {
            // �T�u�N���X�ŋ�̓I�ȏ������`
        }

        // �����̏�����
        public override void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            base.Init(data_, gridObjList_, gridXY_, dir_);
            WorkerNum = 0;
            MaxWorkerNum = data_.LevelDatasList[0].MaxWorker;
            m_UpdateTime = data_.LevelDatasList[0].UpdateTime;
            m_DurationTime = 0;
        }
    }
}