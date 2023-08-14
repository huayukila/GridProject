using System;

public enum ResourceType
{
    None,
    Wood,
    Stone,
    Gold,
    Worker
}

public enum Dir
{
    Down,
    Left,
    Up,
    Right
}
public enum State
{
    Normal,
    Build
}
public enum ObjectType
{

}
public enum Terrain
{
    Normal,
    Goldmine,
    Forests,
    Stonemine,
}


[Serializable]
public struct ResourceCost
{
    public ResourceType resType;
    public int Cost;
}
[Serializable]
public struct BuildingLevelData
{
    public int Level;
    public int MaxWorker;
    public int MaxHp;
    public ResourceCost[] costList;
}
