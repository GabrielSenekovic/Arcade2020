using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpnogController : Movement
{
    public GameObject[] players;
    [SerializeField]  int targetIndex;
    public int coolDown;
    [SerializeField] int time;
    public float turnDeg = 1.0f;
    public float fovDeg = 20.0f;

    [Range(1.0f,6.0f)]
    public float spnogspeed = 4.0f;  
    // Start is called before the first frame update
    void Start()
    {
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1); 
        Speed = spnogspeed;
        checkAggro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void turnSpnog()
    {
        Vector2 playerPos = new Vector2(players[targetIndex].transform.position.x, players[targetIndex].transform.position.y);
        Vector2 spnogPos = new Vector2(transform.position.x,transform.position.y);
        Vector2 spnogToPlayer = spnogPos - playerPos;
        Vector2 bigMagDir = Dir * spnogToPlayer.magnitude;
        Vector2 distBetween = spnogToPlayer - bigMagDir; 

        //if(distBetween.magnitude < 0.5f || spnogToPlayer.magnitude < 0.5f) { return; }

        float triangleHeight = Mathf.Sqrt(spnogToPlayer.magnitude * spnogToPlayer.magnitude - distBetween.magnitude * 0.5f * distBetween.magnitude * 0.5f);
        
        float angle = Mathf.Atan((distBetween.magnitude * 0.5f)/triangleHeight) * Mathf.Rad2Deg; 

        angle *= 2.0f;

        angle += 180.0f;

        Debug.Log("angle: " + angle + " distBetween: " + distBetween + " bigmagdir: " + bigMagDir);

        Debug.DrawLine
        ( 
            transform.position, 
            transform.position + new Vector3(Dir.x, Dir.y, 0),
            Color.green,
            0.1f,
            true
        );

        Vector3 Dirv3 = new Vector3(Dir.x, Dir.y, 0);

        Dirv3 = Quaternion.Euler(0, 0, angle) * Dirv3;

        //Dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //Vector2(Mathf.Cos(Mathf.Abs(angle * Mathf.Deg2Rad)), Mathf.Sin(Mathf.Abs(angle * Mathf.Deg2Rad)));
        Dir = new Vector2(Dirv3.x, Dirv3.y);
       //?transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
    }

    void checkAggro()
    {
        float dist2P1;
        dist2P1 = (players[0].transform.position - transform.position).magnitude;
        if( dist2P1 < Vector3.Distance(players[1].transform.position, transform.position) )
        {
            targetIndex = 0;
        } else { targetIndex = 1;}
    }

    void FixedUpdate() 
    {
        time++;
        if(time >= coolDown)
        {
            time = 0;
            checkAggro();
        }

        turnSpnog();

        MoveObject();
    }
}
