using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int playerID = 0;

    [SerializeField] float playerSpeed = 5;

    private void Update()
    {
        switch (playerID)
        {
            case 0:
                transform.position += new Vector3(Input.GetAxisRaw("Player1Horizontal"), 0, Input.GetAxisRaw("Player1Vertical")).normalized * Time.deltaTime * playerSpeed;
                break;
            case 1:
                transform.position += new Vector3(Input.GetAxisRaw("Player2Horizontal"), 0, Input.GetAxisRaw("Player2Vertical")).normalized * Time.deltaTime * playerSpeed;
                break;
                //possible to also make controller controlls
            default:
                Debug.LogError("playerID is out of range");
                break;
        }
    }
}
