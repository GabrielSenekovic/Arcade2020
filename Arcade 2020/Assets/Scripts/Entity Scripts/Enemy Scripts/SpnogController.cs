using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpnogController : Movement
{
    public enum SpnogMovementType
    {
    Charging = 0,
    Spinning = 1
    }
    public SpnogMovementType movementType = SpnogMovementType.Spinning;
    public GameObject[] players;
    [SerializeField]  int targetIndex;
    public int coolDown;
    [SerializeField] int time;
    int attackTime = 0;
    int attackCoolDown = 100;
    public float fovDeg = 20.0f;

    [Range(0.0f,6.0f)]
    public float spnogspeed = 3.0f;
    public int spnogDamage = 1;
    [SerializeField] float rotationSpeed;

    public int maxAngle;
    public int minAngle;

    Vector2 targetPosition = Vector2.zero; 
    // Start is called before the first frame update

    void Start()
    {
        gameObject.GetComponent<EnemyHealthController>().type = EnemyType.Spnog;
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1); 
        Speed = spnogspeed;
        checkAggro();
        FindObjectOfType<AudioManager>().Play("SpnogNoice");
    }

    // Update is called once per frame
    void Update(){}

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

    void TurnSpnog()
    {
        Vector2 playerPos = new Vector2(players[targetIndex].transform.position.x, players[targetIndex].transform.position.y);
        Vector2 spnogPos = new Vector2(transform.position.x,transform.position.y);
        Vector2 spnogToPlayer = playerPos - spnogPos;
        if(spnogToPlayer.magnitude > 10)
        {
            return;
        }
        Vector2 bigMagDir = Dir * spnogToPlayer.magnitude;

        Vector2 distBetween = spnogToPlayer - bigMagDir; 

        //!DrawThings(playerPos, spnogPos, distBetween, bigMagDir);

        float triangleHeight = Mathf.Sqrt(spnogToPlayer.magnitude * spnogToPlayer.magnitude - distBetween.magnitude * 0.5f * distBetween.magnitude * 0.5f);
        float angle = Mathf.Atan((distBetween.magnitude * 0.5f)/triangleHeight) * Mathf.Rad2Deg; 
        angle *= 2.0f;

        if(movementType == SpnogMovementType.Charging)
        {
            if (angle >= maxAngle && targetPosition != Vector2.zero)
            {
                rig().constraints = RigidbodyConstraints2D.FreezeAll;
                targetPosition = Vector2.zero;
                movementType = SpnogMovementType.Spinning;
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

        if(movementType == SpnogMovementType.Spinning)
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
                movementType = SpnogMovementType.Charging;
            }
        }
    }

    void checkAggro()
    {
        float dist2P1;
        dist2P1 = (players[0].transform.position - transform.position).magnitude;
        if( dist2P1 < Vector3.Distance(players[1].transform.position, transform.position) )
        {   
            if(!players[0].GetComponent<PlayerMovementController>().isDowned)
            targetIndex = 0;
        } else if(!players[1].GetComponent<PlayerMovementController>().isDowned) { targetIndex = 1;}
    }

    void FixedUpdate() 
    {
        time++;
        attackTime++;
 
        if(time >= coolDown)
        {
            time = 0;
            checkAggro();
        }

        TurnSpnog();

        MoveObject();
    }


    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "player1" || other.gameObject.tag == "player2")
        {
            if( attackTime >= attackCoolDown)
            {
                attackTime = 0;
                other.gameObject.GetComponentInParent<PlayerHealthController>().TakeDamage(spnogDamage);
                FindObjectOfType<AudioManager>().Play("PlayerDamage");
            }
        } 
    }
}
