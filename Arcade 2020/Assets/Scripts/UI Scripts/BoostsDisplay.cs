using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostsDisplay : MonoBehaviour
{
    public enum BoostType
    {
        SpeedBoost = 0
    }
    [SerializeField]List<Sprite> speedBoosts;
    int speedBoost_Value = 0;
    public Image speedBoost_Image;

    public void Awake()
    {
        speedBoost_Image.sprite = null;
    }
    public bool AddBoost(BoostType type)
    {
        switch(type)
        {
            case BoostType.SpeedBoost: 
            if(speedBoost_Value == 0)
            {
                speedBoost_Image.color = Color.white;
            }
            if(speedBoost_Value == 3)
            {
                speedBoost_Image.color = new Color(0.8301887f, 0.6833253f, 0, 1);
            }
            if(speedBoost_Value <= 3)
            {
                speedBoost_Image.sprite = speedBoosts[speedBoost_Value]; speedBoost_Value++; return true;
            }
            break;
            default: break;
        }
        return false;
    }
}
