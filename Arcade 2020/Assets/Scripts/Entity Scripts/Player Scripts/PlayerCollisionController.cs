using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public GameObject touchingDoor = null;
    public bool touchingStairs = false;
    private void OnTriggerStay2D(Collider2D other)
    {
        touchingDoor = other.gameObject.GetComponent<Door>() ? other.gameObject : null;
        if (!other.gameObject.CompareTag("ball")){ return; }; //prevent code from running the following for loop unless youre colliding with a ball

        for(int i = 1; i < 3; i++)
        {
            //Go through both players
            if (gameObject.CompareTag("player" + i) && other.gameObject.GetComponent<Ball>().isTraveling && other.gameObject.GetComponent<Ball>().isOn != (Ball.OwnedByPlayer)i-1)
            {
                CatchBall(other.gameObject.GetComponent<Ball>(), (Ball.OwnedByPlayer)i-1);
                if (GetComponent<PlayerBallController>().balls.Count == transform.parent.GetComponent<Team>().GetAmountOfBalls())
                {
                    transform.parent.GetComponent<Team>().ToggleShield(i-1, true);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        touchingDoor = other.gameObject.GetComponent<Door>() ? other.gameObject : null;
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
        touchingStairs = other.gameObject.GetComponent<Stairs>();

        if(other.gameObject.GetComponent<PlayerMovementController>() && other.gameObject.GetComponent<PlayerHealthController>().currentHealth == 0)
        {
            other.gameObject.GetComponent<PlayerHealthController>().currentHealth = 1;
            other.gameObject.GetComponent<PlayerHealthController>().isIFrame = true;
            StartCoroutine(other.gameObject.GetComponent<PlayerHealthController>().OnRevive());
            other.gameObject.GetComponent<PlayerMovementController>().isDowned = false; 
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        touchingStairs = other.gameObject.GetComponent<Stairs>()?false:false;
    }

    private void CatchBall(Ball ball, Ball.OwnedByPlayer catcher)
    {
        GetComponent<PlayerBallController>().balls.Add(ball.gameObject);
        ball.isTraveling = false;
        ball.isOn = catcher;
        FindObjectOfType<AudioManager>().Play("BallPassing");
    }

}