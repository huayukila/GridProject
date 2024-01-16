using UnityEngine;

namespace Framework.BuildProject
{
    public class StageManager : BuildController
    {
        //N,E,S,W
        public Transform[] SpawnPoints;
        private int m_CurrentStage;
        private int m_StageCounts;
        private int m_CurrentStageEnemyCounts;
        private IEnemySystem m_EnemySystem;

        private void Awake()
        {
            m_CurrentStage = 0;
        }

        private void Start()
        {
            m_EnemySystem = this.GetSystem<IEnemySystem>();
            m_EnemySystem.SetSpawnPoint(SpawnPoints);
            m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
            m_StageCounts = m_EnemySystem.GetHowManyStagesHad();

            this.RegisterEvent<EnemyGetKilledEvent>(e =>
                {
                    if (--m_CurrentStageEnemyCounts > 0) return;
                    if (CheckIsAllStageClear())
                    {
                        this.SendEvent<GameClearEvent>();
                    }
                    else
                    {
                        m_CurrentStage++;
                        m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
                        this.SendEvent<StageClearEvent>();
                    }
                })
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StartStage>(e => { SpawnEnemy(m_CurrentStage); })
                .UnregisterWhenGameObjectDestroyed(gameObject);
        }

        void SpawnEnemy(int stageLevel_)
        {
            m_EnemySystem.SpawnEnemy(stageLevel_, this);
        }

        bool CheckIsAllStageClear()
        {
            return m_CurrentStage + 1 == m_StageCounts;
        }
    }
}