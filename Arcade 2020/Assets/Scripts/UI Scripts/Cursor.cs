using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField]Image heldObjectSprite;
    GameObject heldObject;
    private void Start() 
    {
        UnityEngine.Cursor.visible = false;
    }
    private void Update() 
    {
        transform.position = Input.mousePosition;
    }
    public void OnClickBallSlot(BallSlot Ball)
    {
        if(Ball.myBall == null)
        {
            if(heldObject == null)
            {
                return;
            }
            else
            {
                Ball.myBall = heldObject.GetComponent<Ball>();
                Ball.ballSprite.color = Color.white;
                Ball.ballSprite.sprite = heldObject.GetComponent<SpriteRenderer>().sprite;
                heldObjectSprite.color = Color.clear;
                heldObjectSprite.sprite = null;
                heldObject = null;
            }
        }
        else
        {
            if(heldObject == null)
            {
                heldObject = Ball.myBall.gameObject;
                heldObjectSprite.sprite = heldObject.GetComponent<SpriteRenderer>().sprite;
                heldObjectSprite.color = Color.white;
                Ball.ballSprite.sprite = null; Ball.ballSprite.color = Color.clear;
                Ball.myBall = null;
            }
            else
            {
                GameObject temp = heldObject;
                heldObject = Ball.myBall.gameObject;
                heldObjectSprite.sprite = heldObject.GetComponent<SpriteRenderer>().sprite;
                Ball.myBall = temp.GetComponent<Ball>();
                Ball.ballSprite.sprite = temp.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
