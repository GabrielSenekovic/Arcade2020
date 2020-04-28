﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rig() {return this.GetComponent<Rigidbody2D>();}

    public float Speed;

    [System.NonSerialized]public EnemyType type;

    bool isFrozen = false;
    [SerializeField] Vector2 dir;
    public Vector2 Dir
    {
        get
        {
            dir = dir.normalized;
            return dir;
        }
        set
        {
            dir = value.normalized;
        }
    }
  
    [SerializeField] Vector2 vel;
    public Vector2 Vel
    {
        get
        {
            vel = Speed * Dir;
            return vel; 
        }
        set 
        {
            vel = value;
            Speed = vel.magnitude;
            Dir = vel.normalized; 
        }
    }
       public Vector2 Acc;
        public float Fric;

    public void AddVelocity(Vector2 vin)
    {
        Vel += vin;
    }

    public void MoveObject()
    {
        if(!isFrozen) //I needed to freeze all entities when the camera moved! If you know a better way then feel free to change
        {
            rig().velocity = Vel;
            Vel *= Acc;
            Vel *= Fric; // ?Fric = 0.09, or 0.9 or 0.000009
            //if((int)(Speed *10) == 0 ) { Speed = 0; }
        }
        else
        {
            rig().velocity = Vector2.zero;
        }
    }

    public void ToggleFrozen(bool value)
    {
        isFrozen = value;
    }
}