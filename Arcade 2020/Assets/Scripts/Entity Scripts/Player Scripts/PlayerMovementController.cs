using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Movement
{
    
    private float dirx;
    private float diry;
    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode SHOOT;
    public List<GameObject> balls = new List<GameObject>();

    public GameObject touchingDoor = null;
    public bool touchingStairs = false;

    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,0);
    }

    void Update()
    {
        if(Input.GetKey(LEFT) || Input.GetKey(RIGHT) ||Input.GetKey(UP) ||Input.GetKey(DOWN))
        {
            Speed = 5;
        }
        if(Input.GetKeyDown(SHOOT) && balls.Count > 0)
        {
            balls[0].GetComponent<Ball>().isTraveling = true;
            balls.Remove(balls[0]);
        }

        if(Input.anyKey)
        {
            dirx = 0; 
            diry = 0;
           if(Input.GetKey(LEFT)) { dirx =-1;}
           if(Input.GetKey(RIGHT)) { dirx = 1;}
           if(Input.GetKey(UP)) { diry = 1;}
           if(Input.GetKey(DOWN)) { diry =-1;}
            Dir.x = dirx;
            Dir.y = diry;
        }

        if( !(Input.GetKey(LEFT) || Input.GetKey(RIGHT) || Input.GetKey(UP) || Input.GetKey(DOWN) ) )
        {
            Vel = new Vector2(0,0);
        }
        MoveObject();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<Door>())
        {
            touchingDoor = other.gameObject;
        }
        if(other.gameObject.GetComponent<Stairs>())
        {
            touchingStairs = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<Door>())
        {
            touchingDoor = null;
        }
        if(other.gameObject.GetComponent<Stairs>())
        {
            touchingStairs = false;
        }
    }
}
