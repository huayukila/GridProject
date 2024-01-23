using UnityEngine;

namespace Framework.BuildProject
{
    public class StageManager : BuildController
    {
        // 出現ポイント (北、東、南、西)
        public Transform[] SpawnPoints;
        private int m_CurrentStage;
        private int m_CurrentStageEnemyCounts;
        private IEnemySystem m_EnemySystem;
        private int m_StageCounts;

        // 起動時の初期化
        private void Awake()
        {
            m_CurrentStage = 0;
        }

        // 開始時の設定
        private void Start()
        {
            m_EnemySystem = this.GetSystem<IEnemySystem>();
            m_EnemySystem.SetSpawnPoint(SpawnPoints);
            m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
            m_StageCounts = m_EnemySystem.GetHowManyStagesHad();

            RegisterEvents();
        }

        // イベント登録
        private void RegisterEvents()
        {
            this.RegisterEvent<EnemyGetKilledEvent>(e => HandleEnemyKilled())
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StartStage>(e => SpawnEnemy(m_CurrentStage))
                .UnregisterWhenGameObjectDestroyed(gameObject);
        }

        // 敵が倒されたときの処理
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

        // 次のステージへ進む
        private void ProceedToNextStage()
        {
            m_CurrentStage++;
            m_CurrentStageEnemyCounts = m_EnemySystem.GetCurrentStageEnemyCounts(m_CurrentStage);
            this.SendEvent<StageClearEvent>();
        }

        // 敵の出現
        private void SpawnEnemy(int stageLevel)
        {
            m_EnemySystem.SpawnEnemy(stageLevel, this);
        }

        // 全ステージクリアの確認
        private bool CheckIsAllStageClear()
        {
            return m_CurrentStage + 1 == m_StageCounts;
        }
    }
}