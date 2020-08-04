using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : HealthManager
{
    // Start is called before the first frame update
    // Update is called once per frame

    public SpriteRenderer healingLight;
    public bool hasBeenHit = false;
    void Update()
    {
        
    }

    public override void OnHit()
    {
        if(IFrameTime % 4 == 0)
        {
            if(sprites[0].material.shader == shaderSpritesDefault)
            {
                foreach(SpriteRenderer sprite in sprites)
                {
                    sprite.material.shader = shaderGUItext;
                    sprite.color = Color.red;
                }
            }
            else
            {
                foreach(SpriteRenderer sprite in sprites)
                {
                    sprite.material.shader = shaderSpritesDefault;
                    sprite.color = Color.white;
                }
            }
        }
    }
    public override void OnTakeDamage()
    {
        hasBeenHit = true;
    }
    public override void HitAnimationOver()
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.material.shader = shaderSpritesDefault;
            sprite.color = Color.white;
        }
    }

    public override void OnDeath()
    {
        gameObject.GetComponent<PlayerMovementController>().isDowned = true;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
    }
    public IEnumerator OnRevive()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        FindObjectOfType<AudioManager>().Play("Revive");
        healingLight.color = Color.white;
        yield return new WaitForSeconds(1);
        healingLight.color = Color.clear;
    }
}
