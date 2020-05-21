using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{
    public GameObject currentBall;
    public void Shoot()
    {
        currentBall.GetComponent<Ball>().isTraveling = true;
        currentBall.GetComponent<CircleCollider2D>().enabled = true;
        currentBall.GetComponent<Ball>().OnShoot();
        currentBall.transform.parent = transform.parent.transform.parent.transform.parent;
        currentBall.transform.rotation = Quaternion.identity;
        currentBall.transform.position = transform.parent.transform.position;
        currentBall = null;
        GetComponentInParent<Movement>().ToggleFrozen(false);
        GetComponentInParent<PlayerBallController>().throwing = false;
    }
}
