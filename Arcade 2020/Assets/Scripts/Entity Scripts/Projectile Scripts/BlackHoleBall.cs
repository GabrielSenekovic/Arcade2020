using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBall : Ball
{
    bool hasHit = false;

    private int time = 0;
    public int cooldown = 30;

    public float gravSpeed = 5;

    public int specialDamage = 1;

    struct victim 
    {
        public GameObject g;
        public int pushIndex;

        public victim(GameObject go, int pi)
        {
            g = go;
            pushIndex = pi;
        }
    };
    private List<victim> Victims = new List<victim>();

    private void Start() {
        damage= 0;
    }

    void FixedUpdate()
    {
        time++;
        if(Victims.Count > 0)
        if(time >= cooldown)
        {
            time = 0;
            foreach( victim v in Victims)
            {
                v.g.GetComponent<EnemyHealthController>().TakeDamage(specialDamage);
            }
        }
    }

    private void OnAttackStay(GameObject vic)
    {
        bool isNew = false;
        for(int i = 0; i < Victims.Count; i++)
        {
            if(Victims[i].g != vic)
            {
                continue;
            }
            else
            {
                isNew = true;
                break;
            }
        }
        if(isNew)
        {
            Vector2 pushV2 = (Vector2)(transform.position - vic.transform.position).normalized * gravSpeed; //! times some speed
            Victims.Add( new victim(vic, vic.GetComponent<Movement>().AddPushVector(pushV2)));
        }
        else if(Victims.Count > 0)
        {
            victim targetVic = Victims[0];
            foreach( victim v in Victims)
            {
                if(v.g == vic)
                {
                    targetVic = v;
                    break;
                }
            }
            if(targetVic.g != null)
            targetVic.g.GetComponent<Movement>().push[targetVic.pushIndex] = (Vector2)(transform.position - vic.transform.position).normalized * gravSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(isTraveling)
        {
            if(other.CompareTag("enemy"))
            {
                OnAttackStay(other.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("enemy"))
        {
            foreach(victim v in Victims)
            {
                if(v.g == other.gameObject)
                {
                    other.gameObject.GetComponent<Movement>().RemovePushVector(v.pushIndex);
                    Victims.Remove(v);
                    break;
                }
            }
        }
    }
}
