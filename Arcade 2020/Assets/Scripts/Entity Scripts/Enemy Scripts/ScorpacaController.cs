﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpacaController : Movement
{
    public enum ScorpacaMovementType
    {
    SHOOTING = 0,
    FLEEING = 1,
    FOLLOWING = 2
    }
    public ScorpacaMovementType movementType = ScorpacaMovementType.FOLLOWING;
    public GameObject[] players;
    [SerializeField]  int targetIndex;
    public int AggroCoolDown = 60;
    [SerializeField] int time;
    int attackTime = 200;
    int attackCoolDown = 200;

    [Range(0.0f,6.0f)]
    public float scorpacaSpeed = 2.0f;
    public int scorpacaDamage = 1;
    public float projectileSpeed = 4;

    public float projectileStartDist = 3;

    public GameObject projectile;

    public float fleeRange;
    public float followRange;

    Vector2 targetPosition = Vector2.zero; 
    void Start()
    {
        gameObject.GetComponent<EnemyHealthController>().type = EnemyType.Skorpacka;
        Fric = 0.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,0); 
        Speed = scorpacaSpeed;
        CheckAggro();
    }

    void TurnScorpaca()
    {   
        //? math stuff beyond this point
        Vector2 playerPos = new Vector2(players[targetIndex].transform.position.x, players[targetIndex].transform.position.y); //! targetplayer position
        Vector2 scorpacaPos = new Vector2(transform.position.x,transform.position.y); //! position of the scorpaca
        Vector2 scorpacaToPlayer = playerPos - scorpacaPos; // ? vector from target player to scorpaca
        if(scorpacaToPlayer.magnitude > 20) //* this is so they dont follow from too far away
        {
            return;
        }
        //Vector2 bigMagDir = Dir * scorpacaToPlayer.magnitude; // big magnitude direction, ie the movement direction with the magnitude of scorpacatoplayer

        //Vector2 distBetween = scorpacaToPlayer - bigMagDir; // the distance between the two "legs" of the triangle  

        //float triangleHeight = Mathf.Sqrt(scorpacaToPlayer.magnitude * scorpacaToPlayer.magnitude - distBetween.magnitude * 0.5f * distBetween.magnitude * 0.5f); // * the height of the triangle
        //float angle = Mathf.Atan((distBetween.magnitude * 0.5f)/triangleHeight) * Mathf.Rad2Deg; 
        //angle *= 2.0f;

        //! if dist> followRange : go closer. if dist < fleeRange : flee. else shoot.
        if(scorpacaToPlayer.magnitude > followRange){ movementType = ScorpacaMovementType.FOLLOWING;}
        else if(scorpacaToPlayer.magnitude < fleeRange){ movementType = ScorpacaMovementType.FLEEING;}
        else { movementType = ScorpacaMovementType.SHOOTING;}
        Debug.Log("mag: " + scorpacaToPlayer.magnitude + " type: " + movementType.ToString() + " "  + scorpacaSpeed);

        Speed = scorpacaSpeed;
        if(movementType == ScorpacaMovementType.FLEEING) 
        {
            ToggleFrozen(false);
            Dir = -(scorpacaToPlayer);      
        }
        else if(movementType == ScorpacaMovementType.FOLLOWING) 
        {
            ToggleFrozen(false);
            Dir = scorpacaToPlayer;
        }
        else if(movementType == ScorpacaMovementType.SHOOTING) 
        { 
            ToggleFrozen(true);
            if( attackTime == -1)
            {
                attackTime = 0;
            }
            if(attackTime >= attackCoolDown)
            {
                ShootProjectile();
                attackTime = -1;
            }
        }
    }

    void CheckAggro()
    {
        float dist2P1;
        dist2P1 = (players[0].transform.position - transform.position).magnitude;
        if( dist2P1 < Vector3.Distance(players[1].transform.position, transform.position) )
        {   
            if(!players[0].GetComponent<PlayerMovementController>().isDowned)
            targetIndex = 0;
        } else if(!players[1].GetComponent<PlayerMovementController>().isDowned) { targetIndex = 1;}
    }

    void ShootProjectile()
    {
        Vector2 playerPos = new Vector2(players[targetIndex].transform.position.x, players[targetIndex].transform.position.y);
        Vector2 scorpacaPosV2 = new Vector2(transform.position.x, transform.position.y);
        GameObject temp = Instantiate(projectile, transform.position, Quaternion.identity);
        temp.GetComponent<ScorpacaProjectile>().Vel = (playerPos - scorpacaPosV2).normalized * projectileSpeed;
        temp.transform.rotation = Quaternion.Euler(0,0,Mathf.Acos((playerPos - scorpacaPosV2).normalized.x) * Mathf.Rad2Deg);
        temp.GetComponent<ScorpacaProjectile>().damage = scorpacaDamage;
        temp.transform.LookAt(players[targetIndex].transform, Vector3.back);
        temp.transform.rotation =  Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
    }

    private void FixedUpdate() 
    {
        time++;
 
        if(time >= AggroCoolDown)
        {
            time = 0;
            CheckAggro();
        }

        if(attackTime >= 0)
        {
            attackTime++;
        }

        TurnScorpaca();

        Speed = 2;
        MoveObject();
    }

    void Update(){}

    private void OnCollisionEnter2D(Collision2D other) 
    {
        
    }
}
