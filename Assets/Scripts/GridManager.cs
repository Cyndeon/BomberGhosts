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
    [Header("Grid")]
    [SerializeField] Vector2Int gridSize;
    public tileTypes[,] grid { get; set; }

    [Header("Variables")]
    [SerializeField] int tileSize = 3;
    private void Start()
    {
        grid = new tileTypes[gridSize.x, gridSize.y];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int X = 0; X < gridSize.x; X++)
        {
            for (int Y = 0; Y < gridSize.y; Y++)
            {
                Gizmos.DrawWireCube(new Vector3(tileSize * X / 2, 0, tileSize * Y / 2), new Vector3(tileSize, 0 , tileSize));
            }
        }
    }
#endif
}
