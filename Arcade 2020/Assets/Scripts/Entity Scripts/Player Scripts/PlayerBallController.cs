using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallController : MonoBehaviour
{
    public KeyCode SHOOT;
    private float time;
    public float deltatime = 0.1f;

    public List<GameObject> balls = new List<GameObject>();
    void Start(){ }
    
    void Update() 
    {
        if(Input.GetKeyDown(SHOOT) && balls.Count > 0)
        {
            balls[0].GetComponent<Ball>().isTraveling = true;
            balls[0].GetComponent<CircleCollider2D>().enabled = true;
            balls[0].GetComponent<Ball>().isOrbiting = false;
            if(gameObject.CompareTag("player1"))
            {
                Vector3 temp = (balls[0].GetComponent<Ball>().players[1].transform.position - transform.position);
                balls[0].transform.position = transform.position + temp.normalized * 1.0f;
            }
            if(gameObject.CompareTag("player2"))
            {
                Vector3 temp = (balls[0].GetComponent<Ball>().players[0].transform.position - transform.position);
                balls[0].transform.position = transform.position + temp.normalized * 1.0f;
            }
            balls.Remove(balls[0]);
        }
    }

    void FixedUpdate() //* fixed update
    {
        time += deltatime;
        for( int i = 0; i < balls.Count; i++)
        {
            if(!balls[i].GetComponent<Ball>().isOrbiting )
            {
                balls[i].GetComponent<CircleCollider2D>().enabled = false;
                balls[i].GetComponent<Ball>().isOrbiting = true;
            }
            else
            {
                Vector3 temp = new Vector3();
                temp.x = Mathf.Sin(time + 2.0f *(i + 1) * Mathf.PI * (1.0f/balls.Count) );
                temp.y = Mathf.Cos(time + 2.0f * (i + 1) * Mathf.PI * (1.0f/balls.Count) );
                balls[i].transform.position = transform.position + temp;
            }
        }
        if(time >= 2.0f * Mathf.PI)
        {
            time = 0.0f;
        }
    }
}
