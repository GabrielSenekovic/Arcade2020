using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBall : Ball
{
    public List<GameObject> balls = new List<GameObject>();
    public List<GameObject> chains = new List<GameObject>();

    private float time;
    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1, 1);
        Dir = new Vector2(1, 1);
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }
    public override void OnShoot()
    {
        foreach (GameObject ball in balls)
        {
            ball.SetActive(true);
        }
    }
    protected override void OnCatch()
    {
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        if (isTraveling && isOn == OwnedByPlayer.PLAYER_ONE)
        {
            time++;
        }
        else if (isTraveling && isOn == OwnedByPlayer.PLAYER_TWO)
        {
            time++;
        }
        else
        {
            time = 0;
            for(int i = 0; i < balls.Count; i++)
            {
                balls[i].transform.position = transform.position;
            }
        }
        if(time > 10)
        {
            balls[0].GetComponent<MiniOrbitalBall>().Dir = (transform.position - balls[0].transform.position).normalized;
            balls[0].GetComponent<MiniOrbitalBall>().Speed = flySpeed;
            balls[0].GetComponent<MiniOrbitalBall>().MoveObject();
        }
        if(time > 20)
        {
            balls[1].GetComponent<MiniOrbitalBall>().Dir = (balls[0].transform.position - balls[1].transform.position).normalized;
            balls[1].GetComponent<MiniOrbitalBall>().Speed = flySpeed;
            balls[1].GetComponent<MiniOrbitalBall>().MoveObject();
        }
    }
}
