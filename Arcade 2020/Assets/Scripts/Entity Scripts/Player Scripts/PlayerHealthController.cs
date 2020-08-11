using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : HealthManager
{
    public SpriteRenderer healingLight;
    public bool hasBeenHit = false;
    public bool shieldOn = false;

    public override void OnHit()
    {
        if(IFrameTime % 4 == 0)
        {
            Shader shader = sprites[0].material.shader == shaderSpritesDefault ? shaderGUItext : shaderSpritesDefault;
            Color color = sprites[0].material.shader == shaderSpritesDefault ? Color.red : Color.white;

            foreach(SpriteRenderer sprite in sprites)
            {
                sprite.material.shader = shader;
                sprite.color = color;
            }
        }
    }
    public override void TakeDamage(int damage)
    {
        if (!isIFrame && !shieldOn)
        {
            if (currentHealth - damage <= 0)
            {
                OnDeath();
                currentHealth = 0;
            }
            else
            {
                currentHealth -= damage;
                isIFrame = true;
                OnTakeDamage();
                FindObjectOfType<AudioManager>().Play("EnemyHit");
            }
        }
        transform.parent.GetComponent<Team>().ToggleShield((int)GetComponent<PlayerBallController>().identifier, false);
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
