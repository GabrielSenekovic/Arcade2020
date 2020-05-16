using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUp : PickUp
{
    [SerializeField]int healing;
    public override void OnPickUp(GameObject player)
    {
        player.GetComponent<PlayerHealthController>().Heal(healing);
        Destroy(gameObject);
    }
}
