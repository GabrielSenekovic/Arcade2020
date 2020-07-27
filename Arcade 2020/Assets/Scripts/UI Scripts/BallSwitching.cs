using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSwitching : MonoBehaviour
{
    [SerializeField] BallSlot[] slots;
    PlayerBallController currentPlayer;
    [SerializeField] Sprite unuseableSprite;

    [SerializeField] ProjectileRepository projectiles;

    public void Open(PlayerBallController player, Ball pickedUpBall)
    {
        currentPlayer = player;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].myBall = i < player.balls.Count ? player.balls[i].GetComponent<Ball>() : null;
            if(i < player.balls.Count)
            {
                 if(slots[i].myBall.GetComponent<SpriteRenderer>())
                {
                    slots[i].ballSprite.sprite = slots[i].myBall.GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    slots[i].ballSprite.sprite = slots[i].myBall.GetComponentInChildren<SpriteRenderer>().sprite;
                }
            }
            else
            {
                slots[i].ballSprite.sprite = unuseableSprite;
            }
        }

        slots[slots.Length-1].myBall = pickedUpBall;

        if(slots[slots.Length-1].myBall.GetComponent<SpriteRenderer>())
        {
            slots[slots.Length-1].ballSprite.sprite = slots[slots.Length-1].myBall.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            slots[slots.Length-1].ballSprite.sprite = slots[slots.Length-1].myBall.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }
    public void Close()
    {
        List<GameObject> temp = new List<GameObject>();
        for(int i = 0; i < currentPlayer.balls.Count; i++)
        {
            temp.Add(slots[i].myBall.gameObject);
        }
        currentPlayer.transform.parent.transform.gameObject.GetComponent<Team>().RefillBallsOnPlayer(currentPlayer, temp);
        DiscardBall();
        currentPlayer = null;
    }
    public void DiscardBall()
    {
        Ball discardedBall = slots[slots.Length-1].myBall;
        projectiles.balls.Add(discardedBall);
        PowerUp discardedPickup = projectiles.GetPowerUp(discardedBall.myType);
        discardedPickup.transform.position = currentPlayer.transform.position;
        discardedPickup.GetDropped();
        discardedBall.transform.position = new Vector2(10000,10000);
        discardedBall.gameObject.SetActive(false);
        discardedBall.players[0] = null;
        discardedBall.players[1] = null;
        slots[slots.Length-1].myBall = null;
    }
}
