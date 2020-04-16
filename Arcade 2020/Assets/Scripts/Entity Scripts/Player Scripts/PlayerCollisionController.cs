using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player1") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            this.GetComponent<PlayerMovementController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
        }
        else if( other.gameObject.CompareTag("ball") && this.gameObject.CompareTag("player2") && other.gameObject.GetComponent<Ball>().isTraveling)
        {
            this.GetComponent<PlayerMovementController>().balls.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().isTraveling = false;
        }
    }
}
