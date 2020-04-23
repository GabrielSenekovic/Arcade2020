using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rig() {return this.GetComponent<Rigidbody2D>();}

    public float Speed; //? if (things[i] === theThing) { code } (theThing = 5)

    public Vector2 Dir;
  
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
        rig().velocity = Vel;
        Vel *= Acc;
        Vel *= Fric; // ?Fric = 0.09, or 0.9 or 0.000009
        //if((int)(Speed *10) == 0 ) { Speed = 0; }
    }
}