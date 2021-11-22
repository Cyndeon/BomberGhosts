using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int defaultHealth = 0;

    int currentHealth;

    private void Start()
    {
        currentHealth = defaultHealth;
    }

    public void UpdateHealth(int Ammount)
    {
        currentHealth += Ammount;
        if (currentHealth < 1)
        {
            // add some sort of death event
            Destroy(gameObject);
        }
    }
}
