using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerBallController))]
[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerHealthController))]
public class Player : MonoBehaviour
{
    [System.NonSerialized]public PlayerBallController ballController;
    [System.NonSerialized]public PlayerCollisionController collisionController;
    [System.NonSerialized]public PlayerMovementController movementController;
    [System.NonSerialized]public PlayerHealthController healthController;

    public BoostsDisplay boosts;
    private void Awake() 
    {
        ballController = GetComponent<PlayerBallController>();
        collisionController = GetComponent<PlayerCollisionController>();
        movementController = GetComponent<PlayerMovementController>();
        healthController = GetComponent<PlayerHealthController>();
    }
    private void Update() 
    {
        /*if(Input.GetKeyDown(KeyCode.R))
        {
            boosts.AddBoost(BoostsDisplay.BoostType.SpeedBoost);
        }*/
    }
}
