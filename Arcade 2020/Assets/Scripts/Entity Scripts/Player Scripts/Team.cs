<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Team : MonoBehaviour
{
    [SerializeField] ProjectileRepository projectiles;
    [System.NonSerialized] public Player[] players;

    [SerializeField] GameObject ballPrefab;

    [SerializeField] float doorCooldown;

    public bool canEnterDoor = true;

    [SerializeField] UIManager UI;
    public uint amountOfKeys = 0;

    [SerializeField]GameObject shield;
    Color shieldColor;
    public void PreStart()
    {
        players = GetComponentsInChildren<Player>();
        shieldColor = shield.GetComponentInChildren<SpriteRenderer>().color;
        ToggleShield(0, true);
    }
    private void Start() 
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, players[0].transform.position, Quaternion.identity, transform);
            newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
            newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
            players[0].GetComponent<PlayerBallController>().balls.Add(newBall);
            newBall.GetComponent<Ball>().isOn = Ball.OwnedByPlayer.PLAYER_ONE;
        }
    }
    public void MoveTeamToNewRoom()
    {
        players[0].healthController.hasBeenHit = false; players[1].healthController.hasBeenHit = false;
        Vector2 temp = GetDoor().directionModifier * 4 + (Vector2)GetDoor().otherDoor.transform.position;
        players[0].transform.position = new Vector3(temp.x, temp.y, players[0].transform.position.z);
        players[1].transform.position = new Vector3(temp.x, temp.y, players[1].transform.position.z);
    }
    public void ResetTeam(Vector2 Roomsize)
    {
        for(int i = 0; i <= 1; i++)
        {
            players[i].collisionController.touchingStairs = false;
            //players[i].transform.position = new Vector2(10 + 5*i, 10 + 5*i);
        }
        FindObjectOfType<AudioManager>().Play("StairUp");
        players[0].transform.position = new Vector2(Roomsize.x/2 + 8, Roomsize.y/2);
        players[1].transform.position = new Vector2(Roomsize.x/2 - 8, Roomsize.y/2);
    }

    public void IncreaseBallAmount(GameObject player)
    {
        GameObject newBall = Instantiate(ballPrefab, player.transform.position, Quaternion.identity, transform);
        newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
        newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
        player.GetComponent<PlayerBallController>().balls.Add(newBall);
    }

    public void PowerUpBall(GameObject player, PowerUp.PowerUpType typeToChangeTo, PowerUp powerUp)
    {
        projectiles.powerUps.Add(powerUp); powerUp.transform.position = new Vector2(10000, 10000);

        foreach(GameObject ball in player.GetComponent<PlayerBallController>().balls)
        {
            if(ball.GetComponent<Ball>().myType == PowerUp.PowerUpType.Basic)
            {
                //if there is a basic ball to replace
                player.GetComponent<PlayerBallController>().balls.Remove(ball);
                Destroy(ball);
                GameObject newBall = projectiles.GetBall(typeToChangeTo).gameObject;
                newBall.transform.position = player.transform.position;
                newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
                newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
                newBall.SetActive(true);           
                player.GetComponent<PlayerBallController>().balls.Add(newBall);
                return;
            }
        }
        UI.OpenBallSwitching(player.GetComponent<PlayerBallController>(), projectiles.GetBall(typeToChangeTo));
    }
   
    public void PowerUpShield(GameObject player, ShieldUpgrade.ShieldUpgradeType effect, ShieldUpgrade shieldUpgrade)
    {
        return;
    }
    public void RefillBallsOnPlayer(PlayerBallController player, List<GameObject> balls)
    {
        player.balls.Clear();
        //gives a high amount of balls to one player
        for(int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<Ball>().isOn = player.identifier;
            balls[i].transform.position = player.transform.position;
            balls[i].GetComponent<Ball>().players[0] = players[0].gameObject;
            balls[i].GetComponent<Ball>().players[1] = players[1].gameObject;
            balls[i].SetActive(true);
            player.balls.Add(balls[i]);
        }
    }

    public IEnumerator WaitUntilCanEnterDoor()
    {
        canEnterDoor = false;
        yield return new WaitForSeconds(doorCooldown);
        canEnterDoor = true;
    }
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField] ProjectileRepository projectiles;
    [System.NonSerialized] public Player[] players;

    [SerializeField] GameObject ballPrefab;

    [SerializeField] float doorCooldown;

    public bool canEnterDoor = true;

    [SerializeField] UIManager UI;
    public uint amountOfKeys = 0;

    private void Awake()
    {
        players = GetComponentsInChildren<Player>();
    }
    private void Start() 
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, players[0].transform.position, Quaternion.identity, transform);
            newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
            newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
            players[0].GetComponent<PlayerBallController>().balls.Add(newBall);
            newBall.GetComponent<Ball>().isOn = Ball.OwnedByPlayer.PLAYER_ONE;
        }
    }

    public bool GetIfBothTouchingDoor()
    {
        bool temp = (players[0].collisionController.touchingDoor == players[1].collisionController.touchingDoor) && players[0].collisionController.touchingDoor != null;
        if(players[0].collisionController.touchingDoor && temp)
        {
            players[0].collisionController.touchingDoor.GetComponent<Door>().LightUp(true);
        }
        return temp;
    }
    public bool GetIfTouchingStairs()
    {
        return (players[0].collisionController.touchingStairs || players[1].collisionController.touchingStairs);
    }
    public Door GetDoor()
    {
        return players[0].collisionController.touchingDoor.GetComponent<Door>();
    }
    public bool HasTakenDamage()
    {
        return players[0].healthController.hasBeenHit && players[1].healthController.hasBeenHit;
    }
    public bool GetIfBothPlayersDead()
    {
        return players[0].gameObject.GetComponent<PlayerMovementController>().isDowned && players[1].gameObject.GetComponent<PlayerMovementController>().isDowned;
    }
    public void MoveTeamToNewRoom()
    {
        players[0].healthController.hasBeenHit = false; players[1].healthController.hasBeenHit = false;
        Vector2 temp = GetDoor().directionModifier * 4 + (Vector2)GetDoor().otherDoor.transform.position;
        players[0].transform.position = new Vector3(temp.x, temp.y, players[0].transform.position.z);
        players[1].transform.position = new Vector3(temp.x, temp.y, players[1].transform.position.z);
    }
    public void ResetTeam(Vector2 Roomsize)
    {
        for(int i = 0; i <= 1; i++)
        {
            players[i].collisionController.touchingStairs = false;
            //players[i].transform.position = new Vector2(10 + 5*i, 10 + 5*i);
        }
        FindObjectOfType<AudioManager>().Play("StairUp");
        players[0].transform.position = new Vector2(Roomsize.x/2 + 8, Roomsize.y/2);
        players[1].transform.position = new Vector2(Roomsize.x/2 - 8, Roomsize.y/2);
    }

    public void IncreaseBallAmount(GameObject player)
    {
        GameObject newBall = Instantiate(ballPrefab, player.transform.position, Quaternion.identity, transform);
        newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
        newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
        player.GetComponent<PlayerBallController>().balls.Add(newBall);
    }

    public bool PowerUpBall(GameObject player, PowerUp.PowerUpType typeToChangeTo, PowerUp powerUp)
    {
        projectiles.powerUps.Add(powerUp); powerUp.transform.position = new Vector2(10000, 10000);

        foreach(GameObject ball in player.GetComponent<PlayerBallController>().balls)
        {
            if(ball.GetComponent<Ball>().myType == PowerUp.PowerUpType.Basic)
            {
                //if there is a basic ball to replace
                player.GetComponent<PlayerBallController>().balls.Remove(ball);
                Destroy(ball);
                GameObject newBall = projectiles.GetBall(typeToChangeTo).gameObject;
                newBall.transform.position = player.transform.position;
                newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
                newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
                newBall.SetActive(true);           
                player.GetComponent<PlayerBallController>().balls.Add(newBall);
                return true;
            }
        }
        UI.OpenBallSwitching(player.GetComponent<PlayerBallController>(), projectiles.GetBall(typeToChangeTo));
        return false;
    }
    public void RefillBallsOnPlayer(PlayerBallController player, List<GameObject> balls)
    {
        player.balls.Clear();
        //gives a high amount of balls to one player
        for(int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<Ball>().isOn = player.identifier;
            balls[i].transform.position = player.transform.position;
            balls[i].GetComponent<Ball>().players[0] = players[0].gameObject;
            balls[i].GetComponent<Ball>().players[1] = players[1].gameObject;
            balls[i].SetActive(true);
            player.balls.Add(balls[i]);
        }
    }

    public IEnumerator WaitUntilCanEnterDoor()
    {
        canEnterDoor = false;
        yield return new WaitForSeconds(doorCooldown);
        canEnterDoor = true;
    }
>>>>>>> Johannes-Thebuilder
    public void AddKey()
    {
        amountOfKeys++;
        UI.keyAmount.text = ": " + amountOfKeys;
    }
    
    public void ToggleShield(int player, bool value)
    {
        shield.transform.parent = players[player].transform;
        shield.transform.position = players[player].transform.position;
        players[player].healthController.shieldOn = value;
        shield.GetComponentInChildren<SpriteRenderer>().color = value? shieldColor: Color.clear;
    }
}
//Get functions below
public partial class Team : MonoBehaviour
{
    public bool GetIfBothTouchingDoor()
    {
        bool temp = (players[0].collisionController.touchingDoor == players[1].collisionController.touchingDoor) && players[0].collisionController.touchingDoor != null;
        if (players[0].collisionController.touchingDoor && temp)
        {
            players[0].collisionController.touchingDoor.GetComponent<Door>().LightUp(true);
        }
        return temp;
    }
    public bool GetIfTouchingStairs()
    {
        return players[0].collisionController.touchingStairs || players[1].collisionController.touchingStairs;
    }
    public Door GetDoor()
    {
        return players[0].collisionController.touchingDoor.GetComponent<Door>();
    }
    public int GetAmountOfBalls()
    {
        return players[0].ballController.balls.Count + players[1].ballController.balls.Count;
    }
    public bool GetIfHasTakenDamage()
    {
        return players[0].healthController.hasBeenHit || players[1].healthController.hasBeenHit;
    }
    public bool GetIfBothPlayersDead()
    {
        return players[0].gameObject.GetComponent<PlayerMovementController>().isDowned && players[1].gameObject.GetComponent<PlayerMovementController>().isDowned;
    }
}
