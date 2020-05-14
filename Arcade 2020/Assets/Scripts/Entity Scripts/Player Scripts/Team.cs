using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public PlayerCollisionController[] players;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] List<GameObject> powerUpBalls;
    public uint amountOfKeys = 0;

    private void Start() 
    {
        for(int i = 0; i < 1; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, players[0].transform.position, Quaternion.identity, transform);
            newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
            newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
            players[0].GetComponent<PlayerBallController>().balls.Add(newBall);
        }
        GameObject newBall2 = Instantiate(powerUpBalls[0], players[0].transform.position, Quaternion.identity, transform);
        players[0].GetComponent<PlayerBallController>().balls.Add(newBall2);
        newBall2.GetComponent<Ball>().players[0] = players[0].gameObject;
        newBall2.GetComponent<Ball>().players[1] = players[1].gameObject;
        GameObject newBall3 = Instantiate(powerUpBalls[1], players[0].transform.position, Quaternion.identity, transform);
        players[0].GetComponent<PlayerBallController>().balls.Add(newBall3);
        newBall3.GetComponent<Ball>().players[0] = players[0].gameObject;
        newBall3.GetComponent<Ball>().players[1] = players[1].gameObject;
    }

    public bool GetIfBothTouchingDoor()
    {
        bool temp = (players[0].touchingDoor == players[1].touchingDoor) && players[0].touchingDoor != null;
        if(players[0].touchingDoor && temp)
        {
            players[0].touchingDoor.GetComponent<Door>().LightUp(true);
        }
        return temp;
    }
    public bool GetIfBothTouchingStairs()
    {
        return (players[0].touchingStairs && players[1].touchingStairs);
    }
    public Door GetDoor()
    {
        return players[0].touchingDoor.GetComponent<Door>();
    }
    public bool GetIfBothPlayersDead()
    {
        return players[0].gameObject.GetComponent<PlayerMovementController>().isDowned && players[1].gameObject.GetComponent<PlayerMovementController>().isDowned;
    }
    public void MoveTeamToNewRoom()
    {
        Vector2 temp = GetDoor().directionModifier * 2 + (Vector2)GetDoor().otherDoor.transform.position;
        players[0].transform.position = new Vector3(temp.x, temp.y, players[0].transform.position.z);
        players[1].transform.position = new Vector3(temp.x, temp.y, players[1].transform.position.z);
    }
    public void ResetTeam()
    {
        for(int i = 0; i <= 1; i++)
        {
            players[i].touchingStairs = false;
            players[i].transform.position = new Vector2(10 + 5*i, 10 + 5*i);
        }
    }

    public void IncreaseBallAmount(GameObject player)
    {
        GameObject newBall = Instantiate(ballPrefab, player.transform.position, Quaternion.identity, transform);
        newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
        newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
        player.GetComponent<PlayerBallController>().balls.Add(newBall);
    }

    public bool PowerUpBall(GameObject player, PowerUpType typeToChangeTo)
    {
        foreach(GameObject ball in player.GetComponent<PlayerBallController>().balls)
        {
            if(ball.GetComponent<Ball>().myType == PowerUpType.Basic)
            {
                player.GetComponent<PlayerBallController>().balls.Remove(ball);
                Destroy(ball);
                GameObject newBall = Instantiate(powerUpBalls[(int)typeToChangeTo - 1], player.transform.position, Quaternion.identity, transform); 
                newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
                newBall.GetComponent<Ball>().players[1] = players[1].gameObject;              
                player.GetComponent<PlayerBallController>().balls.Add(newBall);
                return true;
            }
        }
        Debug.Log("It didnt contain a basic ball");
        return false;
    }
}
