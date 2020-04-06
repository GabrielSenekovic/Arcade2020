using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rig() { return this.GetComponent<Rigidbody2D>();}

    public float Speed
    {
        get { return Speed; }
        set{ Speed = value; }
    }

    public Vector2 Dir
    {
        get { return Dir;}
        set { Dir = value;}
    }

     public Vector2 Vel
    {
        get
        {
            Vel = Speed * Dir;
            return Vel; 
        }
        set 
        {
            Vel = value;
            Speed = Vel.magnitude;
            Dir = Vel.normalized; 
        }
    }
       public Vector2 Acc
    {
        get { return Acc; }
        set { Acc = value; }
    }

        public float Fric
    {
        get { return Fric; }
        set { Fric = value; }
    }

    public void AddVelocity(Vector2 vin)
    {
        Vel += vin;
    }

    public void MoveObject()
    {
        rig().velocity = Vel;
        rig().velocity *= Acc;
        rig().velocity *= Fric; // ?Fric = 0.09, or 0.9 or 0.000009
        if((int)(Speed *100) == 0 ) { Speed = 0; }
    }
}