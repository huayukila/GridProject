public class GridObject
{
    int m_X;
    int m_Z;
    public bool IsEmpty;
    public ResourceCost TerrainData;

    public GridObject(int x_, int z_)
    {
        IsEmpty = true;
        m_X = x_;
        m_Z = z_;
        TerrainData = new ResourceCost()
        {
            resType = ResourceType.None,
            Cost = 0
        };
    }
}