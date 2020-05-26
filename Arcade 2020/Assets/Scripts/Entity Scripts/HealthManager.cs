using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public int IFrameTime;
    public bool isIFrame;
    public int IFrameCooldown = 30;
    public bool isdead;

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    private SpriteRenderer[] sprites;

    private void Start() 
    {
        isdead = false;
        currentHealth = maxHealth;
        isIFrame = false;
        IFrameTime = 0;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void FixedUpdate() 
    {   
        if(isIFrame)
        {
            IFrameTime++;
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

        if(IFrameTime >= IFrameCooldown)
        {
            IFrameTime = 0;
            isIFrame = false;
            foreach(SpriteRenderer sprite in sprites)
            {
                sprite.material.shader = shaderSpritesDefault;
                sprite.color = Color.white;
            }
        }

        ChildUpdate();
    }

    public void TakeDamage(int damage)
    {
        if(!isIFrame)
        {
            if(currentHealth - damage <= 0)
            {
                OnDeath();
                currentHealth = 0;
            }
            else 
            {
                currentHealth -= damage;
                isIFrame = true;
                FindObjectOfType<AudioManager>().Play("EnemyHit");
            }
        }
    }
    public void Heal(int health)
    {
        currentHealth = currentHealth+health > maxHealth? maxHealth: currentHealth+=health;
    }

    virtual public void OnDeath()
    {
        
    }

    virtual public void ChildUpdate()
    {

    }
}
