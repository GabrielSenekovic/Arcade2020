using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallController : MonoBehaviour
{
    public Ball.OwnedByPlayer identifier;
    public KeyCode SHOOT;

    public bool throwing = false;
    private float time;
    public float deltatime = 0.1f;

    public float orbitDist = 1.0f;

    public List<GameObject> balls = new List<GameObject>();

    [SerializeField]Transform ballHand;
    void Start(){ }
    
    void Update() 
    {
        if(Input.GetKeyDown(SHOOT) && balls.Count > 0 && !gameObject.GetComponent<PlayerMovementController>().isDowned && !throwing)
        {
            int player = gameObject.CompareTag("player1") ? 1 : 0;
            ThrowBall(player);
            transform.parent.GetComponent<Team>().ToggleShield(player, false);
        }
    }

    void FixedUpdate() //* fixed update
    {
        time += deltatime;
        for( int i = 0; i < balls.Count; i++)
        {
            if(!balls[i].GetComponent<Ball>().isOrbiting)
            {
                balls[i].GetComponent<CircleCollider2D>().enabled = false;
                balls[i].GetComponent<Ball>().isOrbiting = true;
            }
            else
            {
                Vector3 vecFromPlayer = new Vector3();
                vecFromPlayer.x = Mathf.Sin(time + 2.0f *(i + 1) * Mathf.PI * (1.0f/balls.Count) );
                vecFromPlayer.y = Mathf.Cos(time + 2.0f * (i + 1) * Mathf.PI * (1.0f/balls.Count) );
                balls[i].transform.position = transform.position + vecFromPlayer * orbitDist;
            }
        }
        if(time >= 2.0f * Mathf.PI)
        {
            time = 0.0f;
        }
    }
    void ThrowBall(int player)
    {
        Vector3 temp = (balls[0].GetComponent<Ball>().players[player].transform.position - transform.position);
        balls[0].transform.position = transform.position + temp.normalized * orbitDist;
        if (balls[0].GetComponent<Ball>().players[player].transform.position.x < transform.position.x)
        {
            Vector3 tempscale = transform.localScale;
            transform.localScale = new Vector3(-1, tempscale.y, tempscale.z);
        }
        else
        {
            Vector3 tempscale = transform.localScale;
            transform.localScale = new Vector3(1, tempscale.y, tempscale.z);
        }

        balls[0].transform.parent = ballHand; throwing = true;
        balls[0].GetComponent<Ball>().isOrbiting = false;
        balls[0].transform.localPosition = new Vector2(5, 0);
        GetComponentInChildren<PlayerAnimationListener>().currentBall = balls[0];
        balls.Remove(GetComponentInParent<PlayerBallController>().balls[0]);
        GetComponentInChildren<Animator>().SetTrigger("Throw");
        GetComponent<Movement>().ToggleFrozen(true);
    }
}
