using UnityEngine;

public class GridObject
{
    GridUtils<GridObject> grid;
    int x;
    int z;
    Transform objectTransform;

    public bool isNull => objectTransform == null;
    public ResourceCost TerrainData;
    public GridObject(GridUtils<GridObject> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        TerrainData = new ResourceCost() {
            resType = ResourceType.None,
            Cost = 0
        };
    }
    public void SetTransform(Transform transform)
    {
        objectTransform = transform;
    }
    public void DestoryObject()
    {
        if (objectTransform != null)
        {
            Object.Destroy(objectTransform.gameObject);
        }
    }
}
