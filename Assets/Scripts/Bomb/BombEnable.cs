using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnable : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GetComponent<SphereCollider>().isTrigger = false;
        }
    }
}
