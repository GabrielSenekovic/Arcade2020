using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamPowSplat : MonoBehaviour
{
    
    public enum FadeType
    {
        FADE_IN = 1,
        FADE_OUT = 2,
        ALIVE = 3
    }
    public FadeType fadeType = FadeType.ALIVE;
    public float fadeAwayFrames = 15;
    private Color color(){ return GetComponent<SpriteRenderer>().color;}

    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
         if(fadeType == FadeType.FADE_IN) // ! fade in sprite
        {
            GetComponent<SpriteRenderer>().color = new Color (color().r,color().g,color().b, color().a + (1.0f/fadeAwayFrames) ); 
            if( color().a >= 1 - (1.0f/fadeAwayFrames)) { fadeType = FadeType.FADE_OUT;} // ! when fully opaque : fade out
        }
        else if(fadeType == FadeType.FADE_OUT) // ! fade out and shrink
        {
            GetComponent<SpriteRenderer>().color = new Color (color().r,color().g,color().b, color().a - (1.0f/fadeAwayFrames) ); 
            Vector3 imageScale = transform.localScale;
            transform.localScale = new Vector3(imageScale.x - 0.01f,imageScale.y - 0.01f, 1.0f );
            if( color().a <= 0 + (1.0f/fadeAwayFrames) ) // ! when faded out remove death image 
            {
                Destroy(gameObject);
            }
        }
    }
}
