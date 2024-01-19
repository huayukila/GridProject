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
public enum PlayerState
{
    Normal,
    Build
}
public enum BuildingType
{
    Non,
    House,
    Factory,
    Wall,
    Core,
    BallistaTower,
    MagicTower,
    CannonTower,
    GoldenMine,
    StoneMine,
    Sawmill,
}
public enum Terrain
{
    Normal,
    Goldmine,
    Forests,
    StoneMine,
}
[Serializable]
public struct ResourceCost
{
    public ResourceType resType;
    public int Cost;
}