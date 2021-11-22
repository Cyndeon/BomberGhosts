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
        none = 0
    }
    public tileTypes[,] grid { get; set; }

    [SerializeField] Vector2Int gridSize;
    private void Start()
    {
        grid = new tileTypes[gridSize.x, gridSize.y];
        Debug.Log(grid[gridSize.x - 1, gridSize.y - 1]);
    }
}
