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

    [SerializeField] bool spawnOnlyBalls;

    public static bool SpawnOnlyBalls
    {
        get
        {
            return Instance.spawnOnlyBalls;
        }
    }

    [SerializeField] bool debugButtons;
    public static bool DebugButtons
    {
        get
        {
            return Instance.debugButtons;
        }
    }

    [SerializeField] bool debugText;
    public static bool DebugText
    {
        get
        {
            return Instance.debugText;
        }
    }

    private void Awake() 
    {
        Instance = this;
    }
}
