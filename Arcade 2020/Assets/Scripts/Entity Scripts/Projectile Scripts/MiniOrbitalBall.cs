using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniOrbitalBall : Movement
{
    public int damage = 1;

    [Range(10.0f, 30.0f)]
    public float flySpeed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("enemy"))
        {
            other.GetComponent<EnemyHealthController>().TakeDamage(damage);
        }
    }
}