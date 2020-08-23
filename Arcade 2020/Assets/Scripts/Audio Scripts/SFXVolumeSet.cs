using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SFXVolumeSet : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}