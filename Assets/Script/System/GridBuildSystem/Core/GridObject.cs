public class GridObject
{
    public bool IsEmpty;
    private int m_X;
    private int m_Z;
    public ResourceCost TerrainData;

    public GridObject(int x_, int z_)
    {
        IsEmpty = true;
        m_X = x_;
        m_Z = z_;
        TerrainData = new ResourceCost
        {
            resType = ResourceType.None,
            Cost = 0
        };
    }
}