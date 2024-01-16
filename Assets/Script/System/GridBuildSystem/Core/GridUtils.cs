using System;
using UnityEngine;

public class GridUtils<TGridObject>
{
    int width;
    int height;
    float cellSize;
    //���_���W
    Vector3 originPosition;
    //�O���b�h�I�u�W�F�N�g�ۑ��p����
    private TGridObject[,] gridArray;

    public float GetCellSize => cellSize;

    //�\���֐�
    public GridUtils(int _width, int _height, float _cellSize, Vector3 _originPosition,
        Func<GridUtils<TGridObject>, int, int, TGridObject> createGridObject)
    {
        //�O���b�h�}�b�v�̃T�C�Y
        width = _width;
        height = _height;
        //�O���b�h�̃T�C�Y
        cellSize = _cellSize;
       
        originPosition = _originPosition;

        gridArray = new TGridObject[width, height];

        //�O���b�h����̒��ɃO���b�h�I�u�W�F�N�g����
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        //�e�X�g�p
        // GameObject TextRoot = new("TextRoot");
        // for (int x = 0; x < gridArray.GetLength(0); x++)
        // {
        //     for (int z = 0; z < gridArray.GetLength(1); z++)
        //     {
        //         Debug.DrawLine(GetWorldPosition3D(x, z), GetWorldPosition3D(x, z + 1), Color.white, 100f);
        //         Debug.DrawLine(GetWorldPosition3D(x, z), GetWorldPosition3D(x + 1, z), Color.white, 100f);
        //
        //         UtilsClass.Instance.DrawTextOnObjectHead(GetWorldPosition3D(x, z) + new Vector3(cellSize / 2, 0, cellSize / 2),
        //             new Vector3(0, 1, 0), "(" + x + "," + z + ")").transform.SetParent(TextRoot.transform);
        //     }
        //     Debug.DrawLine(GetWorldPosition3D(0, height), GetWorldPosition3D(width, height), Color.white, 100f);
        //     Debug.DrawLine(GetWorldPosition3D(width, 0), GetWorldPosition3D(width, height), Color.white, 100f);
        // }
    }

    /// <summary>
    /// �O���b�h���W�𐢊E���W�ɕϊ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition3D(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }
    /// <summary>
    /// �O���b�h���W�𐢊E���W�ɕϊ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition2D(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + originPosition;
    }

    /// <summary>
    /// ���E���W���O���b�h���W�ɕϊ�
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void GetXY3D(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }
    /// <summary>
    /// ���E���W���O���b�h���W�ɕϊ�
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetXY2D(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    /// <summary>
    /// �I�u�W�F�N�g���O���b�h�}�b�v�̒��ɑł�����
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="value"></param>
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXY3D(worldPosition, out int x, out int z);
        SetGridObject(x, z, value);
    }
    /// <summary>
    /// �I�u�W�F�N�g���O���b�h�}�b�v�̒��ɑł�����
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="value"></param>
    public void SetGridObject(Vector2 worldPosition, TGridObject value)
    {
        GetXY3D(worldPosition, out int x, out int y);
        SetGridObject(x, y, value);
    }
    /// <summary>
    /// �O���b�h���W�ɂ���āA�I�u�W�F�N�g���l��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TGridObject GetGridObjectByXY(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    /// <summary>
    /// ���E���W�ɂ���āA�I�u�W�F�N�g���l��
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public TGridObject GetGridObjectByWorldPosition(Vector3 worldPosition)
    {
        GetXY3D(worldPosition, out int x, out int z);
        return GetGridObjectByXY(x, z);
    }
    /// <summary>
    /// / ���E���W�ɂ���āA�I�u�W�F�N�g���l��
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public TGridObject GetGridObjectByWorldPosition(Vector2 worldPosition)
    {
        GetXY2D(worldPosition, out int x, out int y);
        return GetGridObjectByXY(x, y);
    }
    /// <summary>
    /// �O���b�h�}�b�v�̃T�C�Y���l��
    /// </summary>
    /// <returns></returns>
    public Vector2 GetGridSize()
    {
        return new Vector2(width, height);
    }

    private void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }
}
