using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreggController : EnemyController
{
    private int time;
    public int creggDamage = 1;
    public enum MoveType {HORIZONTAL, VERTICAL };
    public MoveType movetype;

    void Start()
    {
        gameObject.GetComponent<EnemyHealthController>().type = EnemyType.CREGG;
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Speed = 4.0f;
        int temp = Random.Range(0,2);
        if( temp == 1) { movetype = MoveType.HORIZONTAL;} 
        else {movetype = MoveType.VERTICAL;} 

        if( movetype == MoveType.HORIZONTAL) { Dir = new Vector2(1,0); }
        else { Dir = new Vector2(0,1);}
        FindObjectOfType<AudioManager>().Play("CreggShnipp");
    }

    void FixedUpdate()
    {
        if(!isSpawning)
        {
            time++;   
            MoveObject();
        }
        else
        {
            OnSpawning();
        }
    }

     private void OnCollisionEnter2D(Collision2D other) 
    {
        if(time > 14)
        {
            Dir *= -1;
        //Debug.Log(gameObject.name + " " + Dir);
            time = 0;
        }

        if(other.gameObject.tag == "player1" || other.gameObject.tag == "player2")
        {
            other.gameObject.GetComponent<PlayerHealthController>().TakeDamage(creggDamage);
            FindObjectOfType<AudioManager>().Play("PlayerDamage");
            FindObjectOfType<AudioManager>().Play("BlobAttack");
        } 
    }
}
