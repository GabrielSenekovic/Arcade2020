using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Movement
{
    public bool isTraveling = false;
    public bool isOrbiting;

    [Tooltip("Player1 then Player2")]
    public GameObject[] players;
   
    public enum OwnedByPlayer{ PLAYER_ONE, PLAYER_TWO};
    public OwnedByPlayer isOn;
    public int damage = 1;

    [Range(10.0f, 30.0f)]
    public float flySpeed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1);
    }

    void Update()
    {
        if(isTraveling && isOn == OwnedByPlayer.PLAYER_ONE)
        {
            Speed = flySpeed;
            Dir = (players[1].transform.position - transform.position).normalized;
        }
        else if (isTraveling && isOn == OwnedByPlayer.PLAYER_TWO)
        {
            Speed = flySpeed;
            Dir = (players[0].transform.position - transform.position).normalized;
        }
        else
        {
            Speed = 0.0f;
        }

        MoveObject();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("player1") && isTraveling)
        {
            isOn = OwnedByPlayer.PLAYER_ONE;
            isOrbiting = true;
        }
        else if(other.CompareTag("player2") && isTraveling)
        {
            isOn = OwnedByPlayer.PLAYER_TWO;
            isOrbiting = true;
        }
        else if(other.CompareTag("enemy") && isTraveling)
        {
            other.GetComponent<EnemyHealthController>().TakeDamage(damage);
        }
    }
}
