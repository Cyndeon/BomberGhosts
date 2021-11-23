using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null)
        {
            other.GetComponent<InvokeEvent>().Invoke();
        }
    }
}
