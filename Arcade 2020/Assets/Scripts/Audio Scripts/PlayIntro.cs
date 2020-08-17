using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntro : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip mainTheme;
    AudioSource music;

    private void Start() 
    {
        music = GetComponent<AudioSource>();
        StartCoroutine(playMusic());
    }
    
    public IEnumerator playMusic()
    {
        Debug.Log(music.clip.length);
        music.clip = intro;
        music.Play();
        yield return new WaitForSeconds(music.clip.length);
        music.Stop();
        music.loop = true;
        music.clip = mainTheme;
        music.Play();
    }
}
