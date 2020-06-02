using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    [SerializeField] bool playTutorial;
    public static bool PlayTutorial
    {
        get
        {
            return Instance.playTutorial;
        }
    }

    [SerializeField] bool showSpnogTriangle;
    public static bool ShowSpnogTriangle
    {
        get
        {
            return Instance.showSpnogTriangle;
        }
    }

    private void Awake() 
    {
        Instance = this;
    }
}
