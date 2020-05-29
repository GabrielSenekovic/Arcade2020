using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthManager
{
    [System.NonSerialized]public EnemyType type;
    // Update is called once per frame
    public GameObject[] deathSprites;
    private GameObject deathImage;
    private Color color;
    public override void OnDeath()
    {
        isdead = true;
        BamPow();

    }

    private void BamPow()
    {

        deathImage = deathSprites[Random.Range(0, deathSprites.Length - 1)]; // ! spawn random death image
        deathImage = GameObject.Instantiate(deathImage,transform.position, Quaternion.identity);
        color = deathImage.GetComponent<SpriteRenderer>().color;
        deathImage.GetComponent<SpriteRenderer>().color = new Color (color.r,color.g,color.b, 0); 
        deathImage.GetComponent<BamPowSplat>().fadeType = BamPowSplat.FadeType.FADE_IN; // ! start fade 
    }

    public override void ChildUpdate(){}
}
