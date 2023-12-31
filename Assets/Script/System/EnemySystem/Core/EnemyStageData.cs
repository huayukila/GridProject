using System;
using Framework.BuildProject;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStageData")]
public class EnemyStageData : ScriptableObject
{
    [SerializeField] public Stage[] Stages;
}

[Serializable]
public struct Stage
{
    public Wave[] Waves;
}

[Serializable]
public struct Wave
{
    public float IntervalTime;
    public Monster[] Monsters;
}

[Serializable]
public struct Monster
{
    public int SwapPoint;
    public EnemyType MonsterType;
    public int Amount;
}