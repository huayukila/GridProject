using UnityEngine;

public class GridObject
{
    GridUtils<GridObject> grid;
    int x;
    int z;
    Transform objectTransform;
    
    public bool Canbuild => objectTransform == null;
    public GridObject(GridUtils<GridObject> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }
    public void SetTransform(Transform transform)
    {
        this.objectTransform = transform;
    }
    public void DestoryObject()
    {
        if (objectTransform != null)
        {
            Object.Destroy(objectTransform.gameObject);
        }
    }
}
