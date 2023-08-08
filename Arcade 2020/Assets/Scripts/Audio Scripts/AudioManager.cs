using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;

        public bool loop;

        //[HideInInspector]
        public AudioSource source;
    }
    [System.Serializable]
    public class Music
    {
        public string name;

        public AudioClip intro;
        public AudioClip theme;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
    }
    public Sound[] sounds;
    public Music[] music;
    public PlayIntro musicPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("BGM");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound: " + name + "not found");
            return;
        }
        s.source.Play();
    }
    public void PlayMusic(string name)
    {
        Music m = Array.Find(music, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("sound: " + name + "not found");
            return;
        }
        musicPlayer.intro = m.intro;
        musicPlayer.mainTheme = m.theme;
        StartCoroutine(musicPlayer.playMusic());
    }

}
