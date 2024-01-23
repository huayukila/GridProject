using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/BulletDatabase", fileName = "BulletDatabase")]
public class BulletDataBase : ScriptableObject
{
    [SerializeField] public List<BulletData> BulletDataList;
}

[Serializable]
public class BulletData
{
    public BulletType bulletType;
    public GameObject bulletPrefab;
}

public enum BulletType
{
    Arrow,
    Magic,
    Cannon
}