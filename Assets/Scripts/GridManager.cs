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
        crate,
        wall
    }
    [Header("Grid + tiles")]
    [SerializeField] Vector2Int gridSize;
    public tileTypes[,] grid { get; set; }
    [SerializeField] Transform tilesParent;
    List<Tile> tiles;

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

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = tileTypes.crate;

                GameObject _temp = new GameObject($"X: {x}, Y: {y}");
                _temp.transform.SetParent(tilesParent);
                _temp.transform.position = new Vector3(tileSize * x, 0, tileSize * y);
                Tile _tileScript = _temp.AddComponent<Tile>();
                _tileScript.tilePosition = new Vector2Int(x, y);
                _tileScript.thisTileType = tileTypes.crate;
            }
        }
        // clear 4 corners so players can spawn there
        ClearAroundTile(new Vector2Int(0, 0), 1);
        ClearAroundTile(new Vector2Int(0, gridSize.y), 1);
        ClearAroundTile(new Vector2Int(gridSize.x - 1, 0), 1);
        ClearAroundTile(new Vector2Int(gridSize.x - 1, gridSize.y - 1), 1);

        UpdateGrid();
    }

    void UpdateGrid()
    {
        for (int x = 0; x < grid.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < grid.GetLength(1) - 1; y++)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    // if the tile is already correct, then skip the updating phase
                    if (tiles[i].thisTileType == grid[x, y]) goto SkipUpdate;
                }
                Tile _currentTile = tiles[index];
                // if the tile is already correct, then skip the updating phase
                if (_currentTile.thisTileType == grid[x, y]) goto SkipUpdate;

                // first destroy all it's previous children so new ones can be spawned in
                foreach (Transform child in _currentTile.transform)
                {
                    Destroy(child);
                }
                switch (_currentTile.thisTileType)
                {
                    case tileTypes.none:
                        break;
                    case tileTypes.crate:
                        Instantiate(cratePrefab, tilesParent.transform.position, tilesParent.transform.rotation, tilesParent);
                        break;
                    case tileTypes.wall:
                        Instantiate(wallPrefab, tilesParent.transform.position, tilesParent.transform.rotation, tilesParent);
                        break;
                    default:
                        break;
                }
                _currentTile.thisTileType = grid[x, y];

                // if the tile was already correct, it's sent here
            SkipUpdate: { }
            }
        }
    }

    void ClearAroundTile(Vector2Int _tile, int _distance, tileTypes _newTileType = tileTypes.none)
    {
        for (int x = -_distance; x < _distance; x++)
        {
            for (int y = -_distance; y < _distance; y++)
            {
                if (grid.GetLength(0) + x > grid.GetLength(0) || grid.GetLength(1) + y > grid.GetLength(1)) break;
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
