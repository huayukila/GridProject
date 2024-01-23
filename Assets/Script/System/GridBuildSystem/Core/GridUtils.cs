using System;
using UnityEngine;

public class GridUtils<TGridObject>
{
    private readonly TGridObject[,] m_GridArray;
    private readonly int m_Height;
    private readonly Vector3 m_OriginPosition;
    private readonly int m_Width;

    /// <summary>
    ///     �R���X�g���N�^
    /// </summary>
    public GridUtils(int width, int height, float cellSize, Vector3 originPosition,
        Func<GridUtils<TGridObject>, int, int, TGridObject> createGridObject)
    {
        m_Width = width;
        m_Height = height;
        GetCellSize = cellSize;
        m_OriginPosition = originPosition;

        m_GridArray = new TGridObject[m_Width, m_Height];

        for (var x = 0; x < m_GridArray.GetLength(0); x++)
        for (var y = 0; y < m_GridArray.GetLength(1); y++)
            m_GridArray[x, y] = createGridObject(this, x, y);
    }

    public float GetCellSize { get; }

    /// <summary>
    ///     ���E���W���O���b�h���W�ɕϊ��i3D�j
    /// </summary>
    public Vector3 GetWorldPosition3D(int x, int z)
    {
        return new Vector3(x, 0, z) * GetCellSize + m_OriginPosition;
    }

    /// <summary>
    ///     ���E���W���O���b�h���W�ɕϊ��i2D�j
    /// </summary>
    public Vector3 GetWorldPosition2D(int x, int y)
    {
        return new Vector3(x, y, 0) * GetCellSize + m_OriginPosition;
    }

    /// <summary>
    ///     ���E���W���O���b�h���W�ɕϊ����Ď擾�i3D�j
    /// </summary>
    public void GetXY3D(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - m_OriginPosition).x / GetCellSize);
        z = Mathf.FloorToInt((worldPosition - m_OriginPosition).z / GetCellSize);
    }

    /// <summary>
    ///     ���E���W���O���b�h���W�ɕϊ����Ď擾�i2D�j
    /// </summary>
    public void GetXY2D(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - m_OriginPosition).x / GetCellSize);
        y = Mathf.FloorToInt((worldPosition - m_OriginPosition).y / GetCellSize);
    }

    /// <summary>
    ///     �I�u�W�F�N�g���O���b�h�}�b�v�̒��ɑł����݁i3D�j
    /// </summary>
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXY3D(worldPosition, out var x, out var z);
        SetGridObject(x, z, value);
    }

    /// <summary>
    ///     �I�u�W�F�N�g���O���b�h�}�b�v�̒��ɑł����݁i2D�j
    /// </summary>
    public void SetGridObject(Vector2 worldPosition, TGridObject value)
    {
        GetXY2D(worldPosition, out var x, out var y);
        SetGridObject(x, y, value);
    }

    /// <summary>
    ///     �O���b�h���W�ɂ���āA�I�u�W�F�N�g���l��
    /// </summary>
    public TGridObject GetGridObjectByXY(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
            return m_GridArray[x, y];
        return default;
    }

    /// <summary>
    ///     ���E���W�ɂ���āA�I�u�W�F�N�g���l���i3D�j
    /// </summary>
    public TGridObject GetGridObjectByWorldPosition(Vector3 worldPosition)
    {
        GetXY3D(worldPosition, out var x, out var z);
        return GetGridObjectByXY(x, z);
    }

    /// <summary>
    ///     ���E���W�ɂ���āA�I�u�W�F�N�g���l���i2D�j
    /// </summary>
    public TGridObject GetGridObjectByWorldPosition(Vector2 worldPosition)
    {
        GetXY2D(worldPosition, out var x, out var y);
        return GetGridObjectByXY(x, y);
    }

    /// <summary>
    ///     �O���b�h�}�b�v�̃T�C�Y���l��
    /// </summary>
    public Vector2 GetGridSize()
    {
        return new Vector2(m_Width, m_Height);
    }

    private void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < m_Width && y < m_Height) m_GridArray[x, y] = value;
    }
}