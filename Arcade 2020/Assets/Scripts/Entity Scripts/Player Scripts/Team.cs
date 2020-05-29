using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public PlayerCollisionController[] players;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] List<GameObject> powerUpBalls;

    [SerializeField] float doorCooldown;

    public bool canEnterDoor = true;

    [SerializeField] UIManager UI;
    public uint amountOfKeys = 0;

    private void Start() 
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject newBall = Instantiate(powerUpBalls[0], players[0].transform.position, Quaternion.identity, transform);
            newBall.GetComponent<Ball>().players[0] = players[0].gameObject;
            newBall.GetComponent<Ball>().players[1] = players[1].gameObject;
            players[0].GetComponent<PlayerBallController>().balls.Add(newBall);
        }
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
    public bool GetIfTouchingStairs()
    {
        return (players[0].touchingStairs || players[1].touchingStairs);
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
        Vector2 temp = GetDoor().directionModifier * 4 + (Vector2)GetDoor().otherDoor.transform.position;
        players[0].transform.position = new Vector3(temp.x, temp.y, players[0].transform.position.z);
        players[1].transform.position = new Vector3(temp.x, temp.y, players[1].transform.position.z);
    }
    public void ResetTeam(Vector2 Roomsize)
    {
        for(int i = 0; i <= 1; i++)
        {
            players[i].touchingStairs = false;
            //players[i].transform.position = new Vector2(10 + 5*i, 10 + 5*i);
        }
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
       // UI.OpenBallSwitching(player.GetComponent<PlayerBallController>(), Instantiate(powerUpBalls[(int)typeToChangeTo - 1], player.transform.position, Quaternion.identity, transform).GetComponent<Ball>());
        return false;
    }

    public IEnumerator waitUntilCanEnterDoor()
    {
        canEnterDoor = false;
        yield return new WaitForSeconds(doorCooldown);
        canEnterDoor = true;
    }
}
