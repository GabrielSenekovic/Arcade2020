﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ball
{
    [SerializeField]GameObject LightningPrefab;

    [SerializeField]float attackRadius;
    [SerializeField]float lightningTime;

    List<Movement> targetedEntities = new List<Movement>(){};
    List<GameObject> lightnings = new List<GameObject>(){};
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
        foreach(Movement entity in victim.transform.parent.transform.parent.GetComponent<EntityManager>().entities)
        {
            if(entity != null)
            {
                if((transform.position - entity.transform.position).magnitude < attackRadius)
                {
                    if(!entity.gameObject.GetComponent<PlayerMovementController>() && entity.gameObject != victim)
                    {
                        GameObject newLightning = Instantiate(LightningPrefab, victim.transform.position, Quaternion.identity, transform);

                        Vector3 vectorToTarget = entity.transform.position - newLightning.transform.position;

                      float newsize = vectorToTarget.magnitude/(newLightning.GetComponent<SpriteRenderer>().bounds.size.x);

                        newLightning.GetComponent<SpriteRenderer>().size = new Vector2(newsize, newLightning.GetComponent<SpriteRenderer>().size.y);

                        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                        Quaternion q = Quaternion.AngleAxis(angle+180, Vector3.forward);
                        newLightning.transform.rotation = Quaternion.RotateTowards(newLightning.transform.rotation, q,360);

                        entity.ToggleFrozen(true);
                        targetedEntities.Add(entity);
                        lightnings.Add(newLightning);
                    }
                }
            }
        }
        StartCoroutine(LightningOver());
    }
    public IEnumerator LightningOver()
    {
        Debug.Log("peepee");
        for(int i = 0; i < 4; i++)
        {
            foreach(Movement entity in targetedEntities)
        {
            if(entity != null)
            {
                entity.gameObject.GetComponent<SpriteRenderer>().material.shader = shaderGUItext;
                entity.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
            yield return new WaitForSecondsRealtime(lightningTime/8);
            foreach(Movement entity in targetedEntities)
        {
            if(entity != null)
            {
                entity.gameObject.GetComponent<SpriteRenderer>().material.shader = shaderSpritesDefault;
                entity.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
        foreach(GameObject lightning in lightnings)
        {
            Destroy(lightning);
        }
        targetedEntities.Clear();
        lightnings.Clear();
        GetComponent<Movement>().ToggleFrozen(false);
        Debug.Log("poopoo");
    }
}
