using UnityEngine;

namespace Framework.BuildProject
{
    public class StageManager : BuildController
    {
        // �o���|�C���g (�k�A���A��A��)
        public Transform[] SpawnPoints;
        private int m_CurrentStage;
        private int m_CurrentStageEnemyCounts;
        private IEnemySystem m_EnemySystem;
        private int m_StageCounts;

        // �N�����̏�����
        private void Awake()
        {
            m_CurrentStage = 0;
        }

        // �J�n���̐ݒ�
        private void Start()
        {
            m_EnemySystem = this.GetSystem<IEnemySystem>();
            m_EnemySystem.SetSpawnPoint(SpawnPoints);
            m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
            m_StageCounts = m_EnemySystem.GetHowManyStagesHad();

            RegisterEvents();
        }

        // �C�x���g�o�^
        private void RegisterEvents()
        {
            this.RegisterEvent<EnemyGetKilledEvent>(e => HandleEnemyKilled())
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StartStage>(e => SpawnEnemy(m_CurrentStage))
                .UnregisterWhenGameObjectDestroyed(gameObject);
        }

        // �G���|���ꂽ�Ƃ��̏���
        private void HandleEnemyKilled()
        {
            m_CurrentStageEnemyCounts--;
            if (m_CurrentStageEnemyCounts <= 0)
            {
                if (CheckIsAllStageClear())
                    this.SendEvent<GameClearEvent>();
                else
                    ProceedToNextStage();
            }
        }

        // ���̃X�e�[�W�֐i��
        private void ProceedToNextStage()
        {
            m_CurrentStage++;
            m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
            this.SendEvent<StageClearEvent>();
        }

        // �G�̏o��
        private void SpawnEnemy(int stageLevel)
        {
            m_EnemySystem.SpawnEnemy(stageLevel, this);
        }

        // �S�X�e�[�W�N���A�̊m�F
        private bool CheckIsAllStageClear()
        {
            return m_CurrentStage + 1 == m_StageCounts;
        }
    }
}