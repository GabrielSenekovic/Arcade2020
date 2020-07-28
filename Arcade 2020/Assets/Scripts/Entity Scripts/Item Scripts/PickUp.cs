using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //Code that is stored on in a pick up array in an "item library" that stores all of the values of the pick ups
    //When a pickup is picked up, it calls the item library and does what the script tells you that item does
    //This saves memory, because otherwise we would store all values in each item
    Vector2 destination = Vector2.zero;
    float moveSpeed = 0.1f;
    public virtual void OnPickUp(GameObject player)
    {
        
    }
    private void FixedUpdate() 
    {
        if(GetComponent<Collider2D>().enabled)
        {return;}
        if((Vector2)transform.position != destination)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, destination.x, moveSpeed), Mathf.Lerp(transform.position.y, destination.y, moveSpeed));
            if((int)transform.position.x == (int)destination.x && (int)transform.position.y == (int)destination.y)
            {
                transform.position = destination;
            }
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }
    public void GetDropped()
    {
        GetComponent<Collider2D>().enabled = false;
        int[] temp = new int[2]{-2,2};
        destination = new Vector2(transform.position.x + temp[Random.Range(0,2)], transform.position.y + temp[Random.Range(0,2)]);
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<PlayerMovementController>())
        {
            OnPickUp(other.gameObject);
            FindObjectOfType<AudioManager>().Play("ItemPickUp");
        }
    }
}
