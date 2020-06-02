using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rig() {return this.GetComponent<Rigidbody2D>();}

    [System.NonSerialized]public List<Vector2> push = new List<Vector2>();

    [System.NonSerialized]public float Speed;

    protected bool isFrozen = false;

    Vector2 dir;

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
  
    Vector2 vel;
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
    [System.NonSerialized]public Vector2 Acc;
    [SerializeField]protected float Fric;

    public void AddVelocity(Vector2 vin)
    {
        Vel += vin;
    }

    public int AddPushVector(Vector2 vin)
    {
        push.Add(vin);
        return push.Count - 1;
    }

    public void RemovePushVector(int index)
    {
        push.RemoveAt(index);
    }

    public void MoveObject()
    {
        Vector2 buffer = Vector2.zero;
        foreach( Vector2 v in push)
        {
            buffer += v;
        }
        if(!isFrozen) //I needed to freeze all entities when the camera moved! If you know a better way then feel free to change
        { 
            rig().velocity = Vel + buffer;
            Vel *= Acc;
            Vel *= (1.0f/(1.0f + Fric));
        }
        else
        {
            rig().velocity = buffer;
        }
    }

    public void ToggleFrozen(bool value)
    {
        isFrozen = value;
    }
}