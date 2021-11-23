using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    // SET TO HIDE IN INSPECTOR
    [SerializeField] public int playerCount;
    float heightOffset = .68f;
    GridManager gridM;
    private void Start()
    {
        gridM = GridManager.instance;
    }
    public void SpawnCharacters(int _playerNumber)
    {
        for (int i = 0; i < _playerNumber; i++)
        {
            Vector2Int _gridPosition = new Vector2Int();
            Vector3 _spawnPosition = new Vector3();
            switch (i)
            {
                case 0:
                    _gridPosition = new Vector2Int(1, 1);
                    break;
                case 1:
                    _gridPosition = new Vector2Int(1, gridM.gridSize.y - 2);
                    break;
                case 2:
                    _gridPosition = new Vector2Int(gridM.gridSize.x - 2, 1);
                    break;
                case 3:
                    _gridPosition = new Vector2Int(gridM.gridSize.x - 2, gridM.gridSize.y - 2);
                    break;

                default:
                    Debug.LogError($"{i} are too many players!");
                    break;
            }
            // get the correct spawn position and spawn the player
            _spawnPosition = gridM.GetTileFromList(_gridPosition).gameObject.transform.position + new Vector3(0, heightOffset, 0);
            GameObject _player = Instantiate(playerPrefab, _spawnPosition, new Quaternion(0, 90 * i, 0, 0));
            Debug.Log($"Spawned player {i}");
        }
    }
}
