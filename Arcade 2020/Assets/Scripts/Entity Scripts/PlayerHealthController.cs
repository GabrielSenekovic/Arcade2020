using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : HealthManager
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
        gameObject.GetComponent<PlayerMovementController>().isDowned = true;
    }
}
