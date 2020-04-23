using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public PlayerMovementController[] players;
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
    public void MoveTeamToNewRoom()
    {
        Vector2 temp = GetDoor().directionModifier * 5;
        players[0].transform.position = new Vector3(players[0].transform.position.x + temp.x, players[0].transform.position.y + temp.y, players[0].transform.position.z);
        players[1].transform.position = new Vector3(players[1].transform.position.x + temp.x, players[1].transform.position.y + temp.y, players[1].transform.position.z);
    }
}
