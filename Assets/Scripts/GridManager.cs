using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public void Awake()
    {
        instance = this;
    }
    public enum tileTypes
    {
        none = 0,
        player,
        crate,
        wall
    }
    [Header("Grid")]
    [SerializeField] Vector2Int gridSize;
    public tileTypes[,] grid { get; set; }

    [Header("Variables")]
    [SerializeField] int tileSize = 3;

    [Header("Prefabs")]
    [SerializeField] GameObject cratePrefab;
    [SerializeField] GameObject wallPrefab;
    private void Start()
    {
        CreateStartingGrid();
    }

    void CreateStartingGrid()
    {
        // first create the grid itself
        grid = new tileTypes[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x - 1; x++)
        {
            for (int y = 0; y < gridSize.y - 1; y++)
            {
                grid[x, y] = tileTypes.crate;
            }
        }
        // set all 4 empty player tiles and clear 1 space around them
        grid[0,0] = tileTypes.player;
        ClearAroundTile(new Vector2Int(0, 0), 1);
        grid[0, gridSize.y - 1] = tileTypes.player;
        ClearAroundTile(new Vector2Int(0, gridSize.y), 1);
        grid[gridSize.x - 1, 0] = tileTypes.player;
        ClearAroundTile(new Vector2Int(gridSize.x - 1, 0), 1);
        grid[gridSize.x - 1, gridSize.y - 1] = tileTypes.player;
        ClearAroundTile(new Vector2Int(gridSize.x - 1, gridSize.y - 1), 1);
    }

    void ClearAroundTile(Vector2Int _tile, int _distance, tileTypes _newTileType = tileTypes.none)
    {
        for (int x = -_distance; x < _distance; x++)
        {
            for (int y = -_distance; y < _distance; y++)
            {
                if (grid.GetLength(0) + x > grid.GetLength(0) ||grid.GetLength(1) + y > grid.GetLength(1)) break;
                if (!CheckValidTile(new Vector2Int(grid.GetLength(0) + x, grid.GetLength(1) + y)))
                // CHECK IF ITS DESTRUCTIBLE FIRST (but later)
                grid[x, y] = _newTileType;
            }
        }
    }
    bool CheckValidTile(Vector2Int _tile)
    {
        if (_tile.x < 0 || _tile.y < 0) return false;
        if (_tile.x > grid.GetLength(0) || _tile.y > grid.GetLength(1)) return false;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (x == 0 && y == 0) Gizmos.color = Color.red;
                else Gizmos.color = Color.white;
                    
                // draw a square with the center being the middle of the square
                Gizmos.DrawWireCube(new Vector3(tileSize * x, 0, tileSize * y), new Vector3(tileSize, 0, tileSize));
            }
        }
    }
#endif
}
