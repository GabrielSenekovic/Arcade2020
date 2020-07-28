using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBall : Ball
{
    bool hasHit = false;

    private int time = 0;
    public int cooldown;

    public SpriteRenderer orbitingSprite;

    public SpriteRenderer[] travelingSprites;

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
        foreach(SpriteRenderer sprite in travelingSprites)
        {
            sprite.color = Color.clear;
        }
    }
    public override void OnShoot()
    {
        foreach(SpriteRenderer sprite in travelingSprites)
        {
            sprite.color = Color.white;
        }
        orbitingSprite.color = Color.clear;
    }
    protected override void OnCatch()
    {
        foreach(SpriteRenderer sprite in travelingSprites)
        {
            sprite.color = Color.clear;
        }
        orbitingSprite.color = Color.white;
    }

    void FixedUpdate()
    {
        time++;
        if(Victims.Count > 0)
        if(time >= cooldown)
        {
            time = 0;
            for(int i = 0; i < Victims.Count; i++)
            {
                Victims[i].g.GetComponent<EnemyHealthController>().TakeDamage(specialDamage);
                if(Victims[i].g.GetComponent<EnemyHealthController>().isdead)
                {
                    Victims.RemoveAt(i);
                    i--;
                }
                    FindObjectOfType<AudioManager>().Play("BlackHoleBall");
                }
        }
    }

    private void OnAttackStay(GameObject vic)
    {
        bool isNew = true;
        for(int i = 0; i < Victims.Count; i++)
        {
            if(Victims[i].g != vic)
            {
                isNew = true;
            }
            else
            {
                isNew = false;
                break;
            }
        }
        if(isNew)
        {
            Vector2 pushV2 = (Vector2)(transform.position - vic.transform.position).normalized * gravSpeed; //! times some speed
            Victims.Add( new victim(vic, vic.GetComponent<Movement>().AddPushVector(pushV2)));
            Debug.Log("Adding: " + vic + " index: " + Victims[Victims.Count-1].pushIndex +" at: " + vic.transform.position);
            //Debug.Break();
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
            Debug.Log("Same object: " + vic + " index: " + targetVic.pushIndex + " at: " + vic.transform.position);
            //Debug.Break();
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
