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
    }

    public class EnemySystem : AbstractSystem, IEnemySystem
    {
        private GameObject m_EnemyPool;
        private Transform m_RootPoint;
        private Dictionary<EnemyType, SimpleObjectPool<GameObject>> m_EnemyDic;
        private EnemyStageData m_StageData;
        private Transform[] m_SpawnPoint;

        protected override void OnInit()
        {
            m_RootPoint = GameObject.Find("GameManager").GetComponent<GameManager>().m_RootNode;
            m_EnemyPool = new GameObject("EnemyPool");
            m_EnemyPool.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(m_EnemyPool);
            EnemyDataBase dataBase = Resources.Load<EnemyDataBase>("EnemyDatabase");
            m_StageData = Resources.Load<EnemyStageData>("EnemyStageData");
            m_EnemyDic = new Dictionary<EnemyType, SimpleObjectPool<GameObject>>();
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

            Resources.UnloadAsset(dataBase);
        }


        public GameObject GetEnemy(EnemyType type_)
        {
            m_EnemyDic.TryGetValue(type_, out SimpleObjectPool<GameObject> temp);
            GameObject obj = temp.Allocate();
            obj.transform.parent = m_RootPoint;
            obj.SetActive(true);
            return obj;
        }

        public void SpawnEnemy(int stage_, MonoBehaviour managerMono_)
        {
            ISequence sequence = ActionKit.Sequence();
            foreach (var wave in m_StageData.Stages[stage_].Waves)
            {
                sequence.Delay(wave.IntervalTime).Callback(() =>
                {
                    foreach (var monster in wave.Monsters)
                    {
                        if (monster.SwapPoint is 1 or 3)
                        {
                            for (int i = 0; i < monster.Amount; i++)
                            {
                                Vector3 tempVec = RandomPosition(monster.SwapPoint);
                                float tempValue = tempVec.x;
                                tempVec = new Vector3(tempVec.z, 1, tempValue);
                                tempVec += m_SpawnPoint[monster.SwapPoint].transform.position;
                                GameObject obj = GetEnemy(monster.MonsterType);
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
                            obj.transform.position = tempVec;
                            obj.transform.LookAt(new Vector3(50, 1, 50));
                        }
                    }
                });
            }

            sequence.Start(managerMono_);
        }

        public void RecycleEnemy(GameObject obj_)
        {
            obj_.transform.parent = m_EnemyPool.transform;
            m_EnemyDic.TryGetValue(obj_.GetComponent<EnemyBase>().m_Type, out SimpleObjectPool<GameObject> temp);
            obj_.SetActive(false);
            temp.Recycle(obj_);
        }

        public void SetSpawnPoint(Transform[] points_)
        {
            m_SpawnPoint = points_;
        }

        Vector3 RandomPosition(int spawnPoint)
        {
            float randX = Random.Range(-60, 60);
            float randZ = Random.Range(-10, 10);
            return new Vector3(randX, 1, randZ);
        }
    }
}