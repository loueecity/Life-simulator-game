using UnityEngine;
[System.Serializable]
public class GridCoordinate
{
    public int x;
    public int y;

    public GridCoordinate(int p1, int p2)
    {
        x = p1;
        y = p2;
    }

   //type conversion using explicit operator - converting gridcoord to vector2
    public static explicit operator Vector2(GridCoordinate gridCoordinate)
    {
        return new Vector2((float)gridCoordinate.x, (float)gridCoordinate.y);
    }

////type conversion using explicit operator - converting gridcoord to vector2int
    public static explicit operator Vector2Int(GridCoordinate gridCoordinate)
    {
        return new Vector2Int(gridCoordinate.x, gridCoordinate.y);
    }

//type conversion using explicit operator - converting gridcoord to vector3
    public static explicit operator Vector3(GridCoordinate gridCoordinate)
    {
        return new Vector3((float)gridCoordinate.x, (float)gridCoordinate.y, 0f);
    }

//type conversion using explicit operator - converting gridcoord to vector3int
    public static explicit operator Vector3Int(GridCoordinate gridCoordinate)
    {
        return new Vector3Int(gridCoordinate.x, gridCoordinate.y, 0);
    }
}
