using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Movement
{
    public PowerUpType myType;
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
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1);
    }

    public virtual void OnShoot(){}
    protected virtual void OnCatch(){}
    protected virtual void OnAttack(GameObject victim){}
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
        if(isTraveling)
        {
            if(other.CompareTag("player1") || other.CompareTag("player2"))
            {
                OnCatch();
            }
            else if(other.CompareTag("enemy"))
            {
                other.GetComponent<EnemyHealthController>().TakeDamage(damage);
                OnAttack(other.gameObject);
            }
        }
    }
}
