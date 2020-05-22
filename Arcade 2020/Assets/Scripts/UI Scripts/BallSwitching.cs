using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSwitching : MonoBehaviour
{
    [SerializeField] BallSlot[] slots;
    PlayerBallController currentPlayer;

    public void Open(PlayerBallController player, Ball pickedUpBall)
    {
        currentPlayer = player;
        for(int i = 0; i < slots.Length - 1; i++)
        {
            slots[i].myBall = player.balls[i].GetComponent<Ball>();
            slots[i].ballSprite.sprite = slots[i].myBall.GetComponent<SpriteRenderer>().sprite;
        }
        slots[slots.Length-1].myBall = pickedUpBall;
        slots[slots.Length-1].ballSprite.sprite = slots[slots.Length-1].myBall.GetComponent<SpriteRenderer>().sprite;
    }
    public void Close()
    {
        for(int i = 0; i < slots.Length - 1; i++)
        {
            currentPlayer.balls[i] = slots[i].myBall.gameObject;
        }
        slots[slots.Length-1] = null;
        currentPlayer = null;
    }
}
