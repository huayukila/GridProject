using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public class EnemyManager : BuildController
    {
        //N,E,SW
        public Transform[] SpawnPoints;
        private IEnemySystem m_EnemySystem;

        void Start()
        {
            m_EnemySystem = this.GetSystem<IEnemySystem>();
            m_EnemySystem.SetSpawnPoint(SpawnPoints);
            this.RegisterEvent<NextStageEvent>(e => { SpawnEnemy(e.StageLevel); });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SpawnEnemy(0);
            }
        }

        void SpawnEnemy(int stageLevel_)
        {
            m_EnemySystem.SpawnEnemy(stageLevel_);
        }
    }
}