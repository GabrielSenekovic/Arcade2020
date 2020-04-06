using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Movement
{
    // Start is called before the first frame update
    void Start()
    {
        Fric = 0.99f;
        Acc = new Vector2(1,1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Vel = new Vector2(-5,0); Debug.Log("rgertjoigeoijtrgejio");
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            Vel = new Vector2(5,0);
        }

        MoveObject();
    }
}
