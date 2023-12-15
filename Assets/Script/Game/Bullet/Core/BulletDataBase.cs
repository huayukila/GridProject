using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDatabase")]
public class BulletDataBase : ScriptableObject
{
    [SerializeField] public List<BulletData> m_BulletDataList;
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
    FireBall,
    Boom,
}