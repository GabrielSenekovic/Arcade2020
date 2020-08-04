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

    protected Shader shaderGUItext;
    protected Shader shaderSpritesDefault;

    protected SpriteRenderer[] sprites;

    public GameObject spritesSource;

    private void Start() 
    {
        isdead = false;
        currentHealth = maxHealth;
        isIFrame = false;
        IFrameTime = 0;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Diffuse");
        sprites = spritesSource.GetComponentsInChildren<SpriteRenderer>();
    }

    private void FixedUpdate() 
    {   
        if(isIFrame)
        {
            IFrameTime++;
            OnHit();
        }

        if(IFrameTime >= IFrameCooldown)
        {
            IFrameTime = 0;
            isIFrame = false;
            HitAnimationOver();
        }

        ChildUpdate();
    }

    public virtual void OnHit()
    {

    }
    public virtual void OnTakeDamage()
    {

    }
    public virtual void HitAnimationOver()
    {

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
                OnTakeDamage();
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
