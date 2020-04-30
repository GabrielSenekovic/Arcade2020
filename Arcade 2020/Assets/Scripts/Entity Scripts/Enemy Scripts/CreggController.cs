using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreggController : Movement
{
    private int time;
    public enum MoveType {HORIZONTAL, VERTICAL };
    public MoveType movetype;
    void Start()
    {
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Speed = 4.0f;
        if( movetype == MoveType.HORIZONTAL) { Dir = new Vector2(1,0); }
        else { Dir = new Vector2(0,1);}
    }

    void FixedUpdate()
    {
        time++;   
        MoveObject();
    }

     private void OnCollisionEnter2D(Collision2D other) 
    {
        if(time > 14)
        {
            Dir *= -1;
        //Debug.Log(gameObject.name + " " + Dir);
            time = 0;
        }
        //* if collide player hurt (attack) 
    }
}
