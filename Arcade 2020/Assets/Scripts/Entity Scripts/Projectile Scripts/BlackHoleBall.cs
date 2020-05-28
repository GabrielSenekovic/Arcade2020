using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBall : Ball
{
    bool hasHit = false;

    public float gravSpeed = 5;

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
    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){}

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
        else
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
