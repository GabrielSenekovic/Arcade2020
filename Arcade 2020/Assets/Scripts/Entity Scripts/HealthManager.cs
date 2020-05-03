using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
[System.NonSerialized]public EnemyType type;
    public int currentHealth;
    public int maxHealth;
    public bool isdead;

    private void Start() 
    {
        isdead = false;
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if(currentHealth - damage <= 0)
        {
            OnDeath();
            currentHealth = 0;
        }
        else { currentHealth -= damage;}
    }

    virtual public void OnDeath()
    {
        
    }
}
