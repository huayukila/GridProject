using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    [CreateAssetMenu(menuName = "DataBase/EnemyDatabase", fileName = "EnemyDatabase")]
    public class EnemyDataBase : ScriptableObject
    {
        [SerializeField] public List<EnemyData> EnemyDataList;
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyType enemyType;
        public GameObject enemyPrefab;
    }

    public enum EnemyType
    {
        Goblin,
        LargeGoblin,
    }
}