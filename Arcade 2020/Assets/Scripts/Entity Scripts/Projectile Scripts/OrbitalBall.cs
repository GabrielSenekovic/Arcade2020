using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBall : Ball
{
    public List<GameObject> balls = new List<GameObject>();

    private float time;
    public float deltatime = 0.1f;
    public float orbitDist = 1.0f;

    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1);
        foreach(GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }

    public override void OnShoot()
    {
        foreach(GameObject ball in balls)
        {
            ball.SetActive(true);
        }
    }
    protected override void OnCatch()
    {
        foreach(GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }

    void FixedUpdate() //* fixed update
    {
        time += deltatime;
        for( int i = 0; i < balls.Count; i++)
        {
            Vector3 vecFromCenter = new Vector3();
            vecFromCenter.x = Mathf.Sin(time + 2.0f *(i + 1) * Mathf.PI * (1.0f/balls.Count) );
            vecFromCenter.y = Mathf.Cos(time + 2.0f * (i + 1) * Mathf.PI * (1.0f/balls.Count) );
            balls[i].transform.position = transform.position + vecFromCenter * orbitDist;
        }
        if(time >= 2.0f * Mathf.PI)
        {
            time = 0.0f;
        }
    }
}
