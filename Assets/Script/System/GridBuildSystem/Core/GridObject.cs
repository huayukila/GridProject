public class GridObject
{
    GridUtils<GridObject> grid;
    int x;
    int z;
    public bool IsEmpty;
    public ResourceCost TerrainData;

    public GridObject(GridUtils<GridObject> grid, int x, int z)
    {
        IsEmpty = true;
        this.grid = grid;
        this.x = x;
        this.z = z;
        TerrainData = new ResourceCost()
        {
            resType = ResourceType.None,
            Cost = 0
        };
    }
}