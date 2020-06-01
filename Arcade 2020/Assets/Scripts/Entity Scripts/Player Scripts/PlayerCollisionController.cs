using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public GameObject touchingDoor = null;
    public bool touchingStairs = false;
    private void OnTriggerStay2D(Collider2D other)
    {
        if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player1") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            if(other.gameObject.GetComponent<Ball>().isOn == Ball.OwnedByPlayer.PLAYER_ONE)
            {
                return;
            }
            this.GetComponent<PlayerBallController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
            other.gameObject.GetComponent<Ball>().isOn =  Ball.OwnedByPlayer.PLAYER_ONE;
            FindObjectOfType<AudioManager>().Play("BallPassing");
        }
        else if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player2") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            if(other.gameObject.GetComponent<Ball>().isOn == Ball.OwnedByPlayer.PLAYER_TWO)
            {
                return;
            }
            this.GetComponent<PlayerBallController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
            other.gameObject.GetComponent<Ball>().isOn = Ball.OwnedByPlayer.PLAYER_TWO;
            FindObjectOfType<AudioManager>().Play("BallPassing");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Door>())
        {
            touchingDoor = other.gameObject;
        }
        if(other.gameObject.GetComponent<PlayerMovementController>() && other.gameObject.GetComponent<PlayerHealthController>().currentHealth == 0)
        {
            other.gameObject.GetComponent<PlayerHealthController>().currentHealth = 1;
            other.gameObject.GetComponent<PlayerHealthController>().isIFrame = true;
            other.gameObject.GetComponent<PlayerMovementController>().isDowned = false; 
            other.gameObject.transform.eulerAngles = new Vector3(other.gameObject.transform.eulerAngles.x,other.gameObject.transform.eulerAngles.y,0 );
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Door>())
        {
            other.gameObject.GetComponent<Door>().LightUp(false);
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
