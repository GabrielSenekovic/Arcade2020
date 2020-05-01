using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public GameObject touchingDoor = null;
    public bool touchingStairs = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player1") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            this.GetComponent<PlayerBallController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
            other.gameObject.GetComponent<Ball>().isOn =  Ball.OwnedByPlayer.PLAYER_ONE;
        }
        else if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player2") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            this.GetComponent<PlayerBallController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
            other.gameObject.GetComponent<Ball>().isOn = Ball.OwnedByPlayer.PLAYER_TWO;
        }
        if(other.gameObject.GetComponent<Door>())
        {
            touchingDoor = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Door>())
        {
            touchingDoor = null;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<Stairs>())
        {
            touchingStairs = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<Stairs>())
        {
            touchingStairs = false;
        }
    }
}
