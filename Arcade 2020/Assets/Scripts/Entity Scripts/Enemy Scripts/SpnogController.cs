using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpnogController : Movement
{
    public GameObject[] players;
    private int targetIndex;
    // Start is called before the first frame update
    void Start()
    {
        Fric = 1.0f;
        Acc = new Vector2(1,1);
        Dir = new Vector2(1,1); 
        Speed = 6.0f;
        float dist2P1;
        dist2P1 = (players[0].transform.position - transform.position).magnitude;
        if( dist2P1 < Vector3.Distance(players[1].transform.position, transform.position) )
        {
            targetIndex = 0;
        } else { targetIndex = 1;}
    }

    // Update is called once per frame
    void Update()
    {
        Dir = (players[targetIndex].transform.position - transform.position).normalized;
        Speed = 4.0f;
        
        Vector3 targ = players[targetIndex].transform.position;
        targ.z = 0f;
 
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
 
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));

        MoveObject();
    }
}
