using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //Code that is stored on in a pick up array in an "item library" that stores all of the values of the pick ups
    //When a pickup is picked up, it calls the item library and does what the script tells you that item does
    //This saves memory, because otherwise we would store all values in each item
    public virtual void OnPickUp(GameObject player)
    {

    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<PlayerMovementController>())
        {
            OnPickUp(other.gameObject);
        }
    }
}
