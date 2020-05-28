using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){}
    public BlackHoleBall bhb;

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(bhb.isTraveling)
        {
            if(other.CompareTag("enemy"))
            {
                other.GetComponent<EnemyHealthController>().TakeDamage(bhb.damage);
            }
        }
    }
}
