using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField] List<GameObject> bombTypes = new List<GameObject>();

    [SerializeField] int currentBombID;

    [SerializeField] int gridSize = 2;   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // will change in the future to work together with the grid.
        {
            GameObject currentBomb;
            currentBomb = Instantiate(bombTypes[currentBombID], gridSize * new Vector3(Mathf.RoundToInt(transform.position.x / gridSize), .5f, Mathf.RoundToInt(transform.position.z / gridSize)), Quaternion.identity);
        }
    }
}
