using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "DataBase/BulletDatabase", fileName = "BulletDatabase")]
public class BulletDataBase : ScriptableObject
{
    [SerializeField] public List<BulletData> BulletDataList;
}

[Serializable]
public class BulletData
{
    public BulletType bulletTyped;
    public GameObject bulletPrefab;
}

public enum BulletType
{
    Arrow,
    Magic,
    Cannon,
}