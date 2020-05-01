using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpnogMovementType
{
    Charging = 0,
    Spinning = 1
}
public class SpnogController : Movement
{
    public SpnogMovementType type = SpnogMovementType.Spinning;
    public GameObject[] players;
    [SerializeField]  int targetIndex;
    public int coolDown;
    [SerializeField] int time;
    public float turnDeg = 1.0f;
    public float fovDeg = 20.0f;

    [Range(0.0f,6.0f)]
    public float spnogspeed = 4.0f;
    [SerializeField] float rotationSpeed;

    public int maxAngle;
    public int minAngle;

    Vector2 targetPosition = Vector2.zero; 
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

    void DrawThings(Vector2 PlayerPosition, Vector2 SpnogPosition, Vector2 DistanceBetween, Vector2 BigMac)
    {
        Debug.DrawLine
        ( 
            PlayerPosition, 
            SpnogPosition,
            Color.red,
            0.0f,
            true
        );
        Debug.DrawLine
        ( 
            PlayerPosition, 
            BigMac + SpnogPosition,
            Color.blue,
            0.0f,
            true
        );
         Debug.DrawLine
        ( 
            SpnogPosition, 
            BigMac + SpnogPosition,
            Color.magenta,
            0.0f,
            true
        );
    }

    void turnSpnog()
    {
        Vector2 playerPos = new Vector2(players[targetIndex].transform.position.x, players[targetIndex].transform.position.y);
        Vector2 spnogPos = new Vector2(transform.position.x,transform.position.y);
        Vector2 spnogToPlayer = playerPos - spnogPos;
        
        Vector2 bigMagDir = Dir * spnogToPlayer.magnitude;

        Vector2 distBetween = spnogToPlayer - bigMagDir; 

        DrawThings(playerPos, spnogPos, distBetween, bigMagDir);

        float triangleHeight = Mathf.Sqrt(spnogToPlayer.magnitude * spnogToPlayer.magnitude - distBetween.magnitude * 0.5f * distBetween.magnitude * 0.5f);
        float angle = Mathf.Atan((distBetween.magnitude * 0.5f)/triangleHeight) * Mathf.Rad2Deg; 
        angle *= 2.0f;

        if(type == SpnogMovementType.Charging)
        {
            if (angle >= maxAngle && targetPosition != Vector2.zero)
            {
                rig().constraints = RigidbodyConstraints2D.FreezeAll;
                targetPosition = Vector2.zero;
                type = SpnogMovementType.Spinning;
            }
            else if(angle < maxAngle)
            {
                rig().constraints = RigidbodyConstraints2D.FreezeRotation;
                if(targetPosition == Vector2.zero)
                {
                    targetPosition = spnogToPlayer;
                }
                Dir = targetPosition;
            }
        }

        if(type == SpnogMovementType.Spinning)
        {
            Vector3 Dirv3 = new Vector3(Dir.x, Dir.y, 0);
            Dirv3 = Quaternion.Euler(0, 0, rotationSpeed) * Dirv3;

            if((spnogPos + bigMagDir - playerPos).magnitude < (spnogPos + (Vector2)(Dirv3 * spnogToPlayer.magnitude) - playerPos).magnitude)
            {
                Dirv3 = Quaternion.Euler(0, 0, -rotationSpeed) * new Vector3(Dir.x, Dir.y, 0);
            }

            Dir = new Vector2(Dirv3.x, Dirv3.y);
            if(angle < minAngle)
            {
                type = SpnogMovementType.Charging;
            }
        }
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
