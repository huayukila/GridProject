using System;

public enum ResourceType
{
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
public struct BuildingCost
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
    public BuildingCost[] costList;
}
