using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    interface IEnemySystem : ISystem
    {
        GameObject GetEnemy(EnemyType type_);
        void SpawnEnemy(int stage_);
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
                }, e => { e.GetComponent<EnemyBase>().ResetObj(); }, 10);
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

        public void SpawnEnemy(int stage_)
        {
            foreach (var wave in m_StageData.Stages[stage_].Waves)
            {
                foreach (var monster in wave.Monsters)
                {
                    Transform spawnPoint = m_SpawnPoint[monster.SwapPoint];
                    for (int i = 0; i < monster.Amount; i++)
                    {
                        GameObject temp = GetEnemy(monster.MonsterType);
                        temp.transform.position = spawnPoint.position;
                    }
                }
            }
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
    }
}