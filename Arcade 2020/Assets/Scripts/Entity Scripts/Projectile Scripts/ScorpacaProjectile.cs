using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpacaProjectile : Movement
{
    public int damage;
    void Start()
    {
        Fric = 0.0f;
        Acc = new Vector2(1,1);
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!(other.CompareTag("player1") || other.CompareTag("player2")))
        {
            if(!other.CompareTag("enemy"))
            {
            Destroy(gameObject);
            }
        }
        else
        {
            other.GetComponent<PlayerHealthController>().TakeDamage(damage);
        }
    }
}
