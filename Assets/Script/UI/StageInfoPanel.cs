using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class StageInfoPanel : BuildController
    {
        public Text EnemyCountsTxt;
        public Text CurrentStage;

        private int m_CurrentEnemy;
        private int m_CurrentStage;

        private void Start()
        {
            m_CurrentStage = 1;
            m_CurrentEnemy = this.GetSystem<IEnemySystem>().GetCurrentStageEnemyCounts(m_CurrentStage - 1);
            CurrentStage.text = "Stage:1";
            EnemyCountsTxt.text = ":" + m_CurrentEnemy;
            this.RegisterEvent<EnemyGetKilledEvent>(e =>
            {
                m_CurrentEnemy--;
                EnemyCountsTxt.text = ":" + m_CurrentEnemy;
            }).UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StageClearEvent>(e =>
                {
                    ++m_CurrentStage;
                    CurrentStage.text = "Stage:" + m_CurrentStage;
                    m_CurrentEnemy = this.GetSystem<IEnemySystem>().GetCurrentStageEnemyCounts(m_CurrentStage - 1);
                    EnemyCountsTxt.text = ":" + m_CurrentEnemy;
                })
                .UnregisterWhenGameObjectDestroyed(gameObject);
        }
    }
}