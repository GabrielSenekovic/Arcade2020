using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ball
{
    //[SerializeField]GameObject LightningPrefab;

    [SerializeField]float attackRadius;
    [SerializeField]float lightningTime;

    List<Movement> targetedEntities = new List<Movement>(){};
    [SerializeField]GameObject[] lightnings = new GameObject[4];
    //List<GameObject> lightnings = new List<GameObject>(){};
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
 
     void Start () 
     {
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
     }

    protected override void OnAttack(GameObject victim)
    {
        GetComponent<Movement>().ToggleFrozen(true);
        transform.position = victim.transform.position;
        if(!victim.gameObject.GetComponent<EnemyHealthController>().isdead)
        {
            targetedEntities.Add(victim.GetComponent<Movement>());
        }
        victim.GetComponent<Movement>().ToggleFrozen(true);
        int i = 0;
        foreach(Movement entity in victim.transform.parent.transform.parent.GetComponent<EntityManager>().entities)
        {
            if(entity != null)
            {
                if((transform.position - entity.transform.position).magnitude < attackRadius)
                {
                    if (entity.gameObject.GetComponent<EnemyController>() && entity.gameObject != victim)
                    {
                       // GameObject newLightning = Instantiate(LightningPrefab, victim.transform.position, Quaternion.identity, transform);
                        lightnings[i].GetComponent<SpriteRenderer>().color = Color.white;
                        lightnings[i].transform.position = victim.transform.position;

                        Vector3 vectorToTarget = entity.transform.position - lightnings[i].transform.position;

                        float newsize = vectorToTarget.magnitude / (lightnings[i].GetComponent<SpriteRenderer>().bounds.size.x);

                        lightnings[i].GetComponent<SpriteRenderer>().size = new Vector2(newsize, lightnings[i].GetComponent<SpriteRenderer>().size.y);

                        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                        Quaternion q = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                        lightnings[i].transform.rotation = Quaternion.RotateTowards(lightnings[i].transform.rotation, q, 360);

                        entity.ToggleFrozen(true);
                        targetedEntities.Add(entity);
                        //lightnings.Add(newLightning);
                        FindObjectOfType<AudioManager>().Play("LightningBallZap");
                        i++;
                    }
                }
            }
        }
        StartCoroutine(LightningOver());
    }
    public IEnumerator LightningOver()
    {
        Debug.Log("Lightning over");
        for(int i = 0; i < 4; i++)
        {
            foreach(Movement entity in targetedEntities)
        {
            if(entity != null)
            {
                entity.gameObject.GetComponent<EnemyController>().body.material.shader = shaderGUItext;
                entity.gameObject.GetComponent<EnemyController>().body.color = Color.white;
            }
        }
            yield return new WaitForSecondsRealtime(lightningTime/8);
            foreach(Movement entity in targetedEntities)
        {
            if(entity != null)
            {
                entity.gameObject.GetComponent<EnemyController>().body.material.shader = shaderSpritesDefault;
                entity.gameObject.GetComponent<EnemyController>().body.color = Color.white;
            }
        }
            yield return new WaitForSecondsRealtime(lightningTime/8);
        }

        foreach(Movement entity in targetedEntities)
        {
            if( entity != null)
            {
                entity.ToggleFrozen(false);
                entity.gameObject.GetComponent<EnemyHealthController>().TakeDamage(damage);
            }
        }
        targetedEntities.Clear();
        foreach(GameObject lightning in lightnings)
        {
            lightning.GetComponent<SpriteRenderer>().color = Color.clear;
        }
        //lightnings.Clear();
        GetComponent<Movement>().ToggleFrozen(false);
    }
}
