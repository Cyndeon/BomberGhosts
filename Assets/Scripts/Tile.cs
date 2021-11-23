using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    public Vector2Int tilePosition;
    public GridManager.tileTypes thisTileType;
}
