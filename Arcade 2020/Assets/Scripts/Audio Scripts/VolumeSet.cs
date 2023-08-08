using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetLevel(float sliderValue) 
    { 
        audioMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20); 
    }
}
