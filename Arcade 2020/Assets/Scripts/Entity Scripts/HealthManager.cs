using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
[System.NonSerialized]public EnemyType type;
    public int currentHealth;
    public int maxHealth;

    public int IFrameTime;
    public bool isIFrame;
    public int IFrameCooldown = 30;
    public bool isdead;

    private void Start() 
    {
        isdead = false;
        currentHealth = maxHealth;
        isIFrame = false;
        IFrameTime = 0;
    }

    private void FixedUpdate() 
    {   
        if(isIFrame)
        {
            IFrameTime++;
        }

        if(IFrameTime >= IFrameCooldown)
        {
            IFrameTime = 0;
            isIFrame = false;
        }

        ChildUpdate();
    }

    public void TakeDamage(int damage)
    {
        if(!isIFrame)
        {
            if(currentHealth - damage <= 0)
            {
                OnDeath();
                currentHealth = 0;
            }
            else 
            {
                currentHealth -= damage;
                isIFrame = true;
            }
        }
        FindObjectOfType<AudioManager>().Play("EnemyHit");
    }

    virtual public void OnDeath()
    {
        
    }

    virtual public void ChildUpdate()
    {

    }
}
