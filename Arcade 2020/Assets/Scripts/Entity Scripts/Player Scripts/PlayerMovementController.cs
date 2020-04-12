using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Movement
{
    
    private float dirx;
    private float diry;

    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,0);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.S))
        {
            Speed = 5;
        }

        if(Input.anyKey)
        {
            dirx = 0; 
            diry = 0;
           if(Input.GetKey(KeyCode.A)) { dirx =-1;}
           if(Input.GetKey(KeyCode.D)) { dirx = 1;}
           if(Input.GetKey(KeyCode.W)) { diry = 1;}
           if(Input.GetKey(KeyCode.S)) { diry =-1;}
            Dir.x = dirx;
            Dir.y = diry;
        }

        if( !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ) )
        {
            Vel = new Vector2(0,0);
        }
        MoveObject();
    }
}
