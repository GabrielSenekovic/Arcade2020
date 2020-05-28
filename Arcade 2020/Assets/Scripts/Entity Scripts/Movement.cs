using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rig() {return this.GetComponent<Rigidbody2D>();}

    private List<Vector2> push = new List<Vector2>();

    public float Speed;

    protected bool isFrozen = false;

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

    public int AddPushVector(Vector2 vin)
    {
        push.Add(vin);
        return push.Count;
    }

    public void RemovePushVector(int index)
    {
        push.RemoveAt(index);
    }

    public void MoveObject()
    {
        if(!isFrozen) //I needed to freeze all entities when the camera moved! If you know a better way then feel free to change
        {
            rig().velocity = Vel;
            Vel *= Acc;
            Vel *= (1.0f/(1.0f + Fric)); 
            foreach( Vector2 v in push)
            {
                AddVelocity(v);
            }
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