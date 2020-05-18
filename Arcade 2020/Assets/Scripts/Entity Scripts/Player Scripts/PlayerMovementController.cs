using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Movement
{
    [Range(1.0f,10.0f)]
    public float playerspeed = 5.0f;  
    public bool isDowned = false;
    private float dirx;
    private float diry;
    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;

    void Start()
    {
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,0); 
    }

    void Update()
    {
        if( (Input.GetKey(LEFT) || Input.GetKey(RIGHT) || Input.GetKey(UP) || Input.GetKey(DOWN)) && !isDowned)
        {
            GetComponentInChildren<Animator>().SetBool("Walking", true);
            Speed = playerspeed;
        }

        if(Input.anyKey && !isDowned)
        {
            dirx = 0; 
            diry = 0;
           if(Input.GetKey(LEFT)) { dirx =-1;}
           if(Input.GetKey(RIGHT)) { dirx = 1;}
           if(Input.GetKey(UP)) { diry = 1;}
           if(Input.GetKey(DOWN)) { diry =-1;}
            Dir = new Vector2(dirx, diry);
            if(dirx < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if(dirx > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }

        if( !(Input.GetKey(LEFT) || Input.GetKey(RIGHT) || Input.GetKey(UP) || Input.GetKey(DOWN) ) )
        {
            GetComponentInChildren<Animator>().SetBool("Walking", false);
            Vel = new Vector2(0,0);
        }
    }

    private void FixedUpdate() 
    {
        MoveObject();    
    }
}
