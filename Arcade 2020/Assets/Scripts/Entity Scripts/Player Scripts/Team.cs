using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public PlayerCollisionController[] players;
    public uint amountOfKeys = 0;

    public bool GetIfBothTouchingDoor()
    {
        return ((players[0].touchingDoor == players[1].touchingDoor) && players[0].touchingDoor != null);
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
}
