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
    [SerializeField] List<Tile> tiles;

    [Header("Variables")]
    [SerializeField] int tileSize = 3;
    [SerializeField] float gridManualUpdateCheckTimeSeconds = 10f;

    [Header("Prefabs")]
    [SerializeField] GameObject cratePrefab;
    [SerializeField] GameObject wallPrefab;
    private void Start()
    {
        CreateStartingGrid();
        StartCoroutine(GridUpdateCheck());
    }
    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.A))
            SetRandomWalls(Random.Range(0, 10));
    }
    IEnumerator GridUpdateCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(gridManualUpdateCheckTimeSeconds);
            UpdateGrid();
        }
    }
    void CreateStartingGrid()
    {
        // first create the grid itself
        grid = new tileTypes[gridSize.x, gridSize.y];
        tiles = new List<Tile>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // create empty objects for the crates to spawn in later
                GameObject _temp = new GameObject($"X: {x}, Y: {y}");
                _temp.transform.SetParent(tilesParent);
                _temp.transform.position = new Vector3(tileSize * x, 0, tileSize * y);
                Tile _tileScript = _temp.AddComponent<Tile>();
                _tileScript.tilePosition = new Vector2Int(x, y);
                _tileScript.thisTileType = tileTypes.crate;

                tiles.Add(_tileScript);
            }
        }
        // clear 4 corners so players can spawn there
        ClearAroundTile(new Vector2Int(1, 1), 1);
        ClearAroundTile(new Vector2Int(1, gridSize.y - 1), 1);
        ClearAroundTile(new Vector2Int(gridSize.x - 1, 1), 1);
        ClearAroundTile(new Vector2Int(gridSize.x - 1, gridSize.y - 1), 1);

        UpdateGrid();
    }
    void UpdateGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Tile _currentTile = GetTileFromList(new Vector2Int(x, y));

                // if the tile is already what it should be, skip the updating phase
                if (_currentTile.thisTileType == grid[x, y])
                    goto SkipUpdate;


                // update the grid of the new tile type
                grid[x, y] = _currentTile.thisTileType;

                // destroy all it's previous children so new ones can be spawned in
                foreach (Transform child in _currentTile.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                switch (grid[x, y])
                {
                    case tileTypes.none:
                        Debug.Log("Nothing here");
                        break;
                    case tileTypes.crate:
                        Instantiate(cratePrefab, _currentTile.transform.position, _currentTile.transform.rotation, _currentTile.transform);
                        break;
                    case tileTypes.wall:
                        Instantiate(wallPrefab, _currentTile.transform.position, _currentTile.transform.rotation, _currentTile.transform);
                        break;
                    default:
                        break;
                }

            // if the tile was already correct, it's sent here
            SkipUpdate: { }
            }
        }
    }
    void SetRandomWalls(int _amount)
    {
        var crateList = new List<Tile>();
        // first get all tiles that are crates at the moment
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].thisTileType == tileTypes.crate)
                crateList.Add(tiles[i]);
        }
        for (int i = 0; i < _amount; i++)
        {
            if (crateList.Count < _amount) _amount = crateList.Count;
            int _rand = Random.RandomRange(0, crateList.Count - 1);
            GetTileFromList(crateList[_rand].tilePosition).thisTileType = tileTypes.wall;
        }
        UpdateGrid();
    }

    void ClearAroundTile(Vector2Int _tile, int _distance, tileTypes _newTileType = tileTypes.none)
    {
        for (int x = -_distance; x < _distance; x++)
        {
            for (int y = -_distance; y < _distance; y++)
            {
                if (CheckValidTile(new Vector2Int(_tile.x + x, _tile.y + y)))
                {
                    // CHECK IF ITS DESTRUCTIBLE FIRST for bombs
                    GetTileFromList(new Vector2Int(_tile.x + x, _tile.y + y)).thisTileType = _newTileType;
                }
            }
        }
    }
    Tile GetTileFromList(Vector2Int _position)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            //check if the tile position is the same as the requested tile position
            if (tiles[i].tilePosition == new Vector2Int(_position.x, _position.y))
            {
                return tiles[i];
            }
        }
        Debug.LogError("Could not find requested tile");
        return new Tile();
    }
    bool CheckValidTile(Vector2Int _tile)
    {
        if (_tile.x < 0 || _tile.y < 0) return false;
        if (_tile.x > grid.GetLength(0) - 1 || _tile.y > grid.GetLength(1) - 1) return false;
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
