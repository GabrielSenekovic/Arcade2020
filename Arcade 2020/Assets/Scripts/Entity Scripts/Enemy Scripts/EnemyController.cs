using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Movement
{
    [System.NonSerialized]public bool isSpawning = false;
    public SpriteRenderer body;
    [SerializeField]Transform shadow;
    Vector2 scale;
    float fallSpeed = 6;
    public int difficultyLevel;

    public void Spawn()
    {
        GetComponent<Collider2D>().enabled = false;
        scale = shadow.gameObject.transform.localScale;
        shadow.gameObject.transform.localScale = new Vector2(0,0);
        shadow.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0.3f);
        isSpawning = true;
        body.transform.position = new Vector2(transform.position.x, transform.position.y + 40);
    }
    public void OnSpawning()
    {
        body.transform.position = new Vector2(transform.position.x, Mathf.Lerp(body.transform.position.y, transform.position.y, fallSpeed / Mathf.Pow((body.transform.position.y - transform.position.y),2)));
        shadow.gameObject.transform.localScale = new Vector2(Mathf.Lerp(shadow.gameObject.transform.localScale.x, scale.x, fallSpeed / Mathf.Pow((body.transform.position.y - transform.position.y),2)), Mathf.Lerp(shadow.gameObject.transform.localScale.y, scale.y,fallSpeed / Mathf.Pow((body.transform.position.y - transform.position.y),2)));
        if((int)(body.transform.position.y * 100) <= (int)(transform.position.y * 100))
        {
            body.transform.position = transform.position;
            isSpawning = false;
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
