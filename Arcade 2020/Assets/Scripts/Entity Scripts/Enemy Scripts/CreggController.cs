using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreggController : Movement
{
    public enum MoveType {HORIZONTAL, VERTICAL };
    private MoveType movetype;
    void Start()
    {
        Speed = 7.0f;
        if( movetype == MoveType.HORIZONTAL) { Dir = new Vector2(1,0); }
        else { Dir = new Vector2(0,1);}
    }

    void Update()
    {
        

        MoveObject();
    }

     private void OnCollisionEnter2D(Collision2D other) 
    {
        Dir *= -1;
        //* if collide player hurt (attack) 
    }
}
