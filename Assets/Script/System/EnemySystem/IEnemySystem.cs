using System.Collections.Generic;
using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    interface IEnemySystem : ISystem
    {
        GameObject GetEnemy(EnemyType type_);
        void SpawnEnemy(int stage_, MonoBehaviour managerMono_);
        void RecycleEnemy(GameObject obj_);

        void SetSpawnPoint(Transform[] points_);

        int GetCurrentStageEnemyCounts(int currentStage_);

        int GetHowManyStagesHad();
        void InitByManager();
        void Deinit();
    }

    public class EnemySystem : AbstractSystem, IEnemySystem
    {
        private GameObject m_EnemyPool;
        private Dictionary<EnemyType, SimpleObjectPool<GameObject>> m_EnemyDic;
        private EnemyStageData m_StageData;
        private Transform[] m_SpawnPoint;
        private ISequence m_ActionSequence;
        private int[] m_StageEnemyCounts;
        private int m_StageCounts;
        private List<GameObject> m_ActiveEnemysList;

        protected override void OnInit()
        {
            m_ActiveEnemysList = new List<GameObject>();
            // this.RegisterEvent<StopOrContinueGameEvent>(e =>
            // {
            //     if (m_ActionSequence != null)
            //     {
            //         m_ActionSequence.Paused = !m_ActionSequence.Paused;
            //     }
            // });
        }


        public GameObject GetEnemy(EnemyType type_)
        {
            m_EnemyDic.TryGetValue(type_, out SimpleObjectPool<GameObject> temp);
            GameObject obj = temp.Allocate();
            obj.transform.position = Vector3.zero;
            obj.SetActive(true);
            return obj;
        }

        public void SpawnEnemy(int stage_, MonoBehaviour managerMono_)
        {
            m_ActionSequence = ActionKit.Sequence();
            foreach (var wave in m_StageData.Stages[stage_].Waves)
            {
                m_ActionSequence.Delay(wave.IntervalTime).Callback(() =>
                {
                    foreach (var monster in wave.Monsters)
                    {
                        if (monster.SwapPoint is 1 or 3)
                        {
                            for (int i = 0; i < monster.Amount; i++)
                            {
                                Vector3 tempVec = RandomPosition(monster.SwapPoint);
                                float tempValue = tempVec.x;
                                tempVec = new Vector3(tempVec.z, 0, tempValue);
                                tempVec += m_SpawnPoint[monster.SwapPoint].transform.position;
                                GameObject obj = GetEnemy(monster.MonsterType);
                                m_ActiveEnemysList.Add(obj);
                                obj.transform.position = tempVec;
                                obj.transform.LookAt(new Vector3(50, 1, 50));
                            }

                            continue;
                        }

                        for (int i = 0; i < monster.Amount; i++)
                        {
                            Vector3 tempVec = RandomPosition(monster.SwapPoint);
                            tempVec += m_SpawnPoint[monster.SwapPoint].transform.position;
                            GameObject obj = GetEnemy(monster.MonsterType);
                            m_ActiveEnemysList.Add(obj);
                            obj.transform.position = tempVec;
                            obj.transform.LookAt(new Vector3(50, 1, 50));
                        }
                    }
                });
            }

            m_ActionSequence.Callback(() => { m_ActionSequence = null; });
            m_ActionSequence.Start(managerMono_);
        }

        public void RecycleEnemy(GameObject obj_)
        {
            m_ActiveEnemysList.Remove(obj_);
            obj_.transform.parent = m_EnemyPool.transform;
            m_EnemyDic.TryGetValue(obj_.GetComponent<EnemyBase>().Type, out SimpleObjectPool<GameObject> temp);
            obj_.SetActive(false);
            temp.Recycle(obj_);
        }

        public void SetSpawnPoint(Transform[] points_)
        {
            m_SpawnPoint = points_;
        }

        public int GetCurrentStageEnemyCounts(int currentStage_)
        {
            return m_StageEnemyCounts[currentStage_];
        }

        public int GetHowManyStagesHad()
        {
            return m_StageCounts;
        }

        public void InitByManager()
        {
            m_EnemyPool = new GameObject("EnemyPool");
            m_EnemyPool.transform.position = Vector3.zero;
            m_StageData = Resources.Load<EnemyStageData>("EnemyStageData");
            m_EnemyDic = new Dictionary<EnemyType, SimpleObjectPool<GameObject>>();
            EnemyDataBase dataBase = Resources.Load<EnemyDataBase>("EnemyDatabase");
            foreach (var enemyData in dataBase.EnemyDataList)
            {
                SimpleObjectPool<GameObject> temp = new SimpleObjectPool<GameObject>(() =>
                {
                    GameObject obj = GameObject.Instantiate(enemyData.enemyPrefab, Vector3.zero, Quaternion.identity);
                    obj.transform.parent = m_EnemyPool.transform;
                    obj.SetActive(false);
                    return obj;
                }, e => { e.GetComponent<EnemyBase>().ResetObj(); }, 100);
                m_EnemyDic.Add(enemyData.enemyType, temp);
            }

            m_StageEnemyCounts = new int[m_StageData.Stages.Length];

            int index = 0;
            foreach (var stage in m_StageData.Stages)
            {
                foreach (var wave in stage.Waves)
                {
                    foreach (var monster in wave.Monsters)
                    {
                        m_StageEnemyCounts[index] += monster.Amount;
                    }
                }

                index++;
            }

            m_StageCounts = index;
            Resources.UnloadAsset(dataBase);
        }

        public void Deinit()
        {
            foreach (var pool in m_EnemyDic.Values)
            {
                pool.Clear();
            }

            m_EnemyDic.Clear();
            m_EnemyPool = null;
            m_SpawnPoint = null;
            m_ActionSequence = null;
            m_StageEnemyCounts = null;
            m_StageCounts = 0;
            Resources.UnloadAsset(m_StageData);
        }

        Vector3 RandomPosition(int spawnPoint)
        {
            float randX = Random.Range(-60, 60);
            float randZ = Random.Range(-10, 10);
            return new Vector3(randX, 1, randZ);
        }
    }
}