using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Movement
{
    [Range(1.0f,10.0f)]
    public float playerSpeed = 5.0f;  
    private bool isDashing = false;
    bool canDash = true;
    [SerializeField] int dashCooldown;
    [System.NonSerialized] public int dashCooldownTimer;
    [SerializeField] float playerDashSpeed;
    private float dashTime = 0.1f;
    [SerializeField] float startDashTime;
    [System.NonSerialized] public bool isDowned = false;
    private float dirx;
    private float diry;
    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode DASH;

    void Start()
    {
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,0); 
        dashTime = startDashTime;
    }

    void Update()
    {
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer--;
        }
        else
        {
            dashCooldownTimer = 0;
            canDash = true;
        }
        if(isFrozen)
        {
            return;
        }
        if( (Input.GetKey(LEFT) || Input.GetKey(RIGHT) || Input.GetKey(UP) || Input.GetKey(DOWN)) && !isDowned)
        {
            GetComponentInChildren<Animator>().SetBool("Walking", true);
            Speed = playerSpeed;
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
            Vel = Vector2.zero;
        }
        if(isDashing)
        {
            dashTime -= Time.deltaTime;
            Vel = Dir * playerDashSpeed;
            if(dashTime <= 0)
            {
                dashTime = startDashTime;
                isDashing = false;
                dashCooldownTimer = dashCooldown;
                canDash = false;
                Vel = Vector2.zero;
            }
        }
        else
        {
            if(Input.GetKeyDown(DASH) && canDash)
            {
                isDashing = true;
            }
        }
    }
    public bool HasDashed()
    {
        return dashCooldownTimer == dashCooldown;
    }

    private void FixedUpdate() 
    {
        MoveObject();  
    }
}
