using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public GameObject door;
    public Key key;

    public enum Rarity
    {
        COMMON = 20,
        UNCOMMON = 8,
        RARE = 2,
        VERYRARE = 1
    }
    [System.Serializable]public struct PickUpEntry
    {
        public PickUp item;
        public Rarity rarity;
    }
    public List<PickUpEntry> pickUps;
    
    public List<GameObject> powerUpBalls;

    public List<Obstacle> obstacles;
    public GameObject stairs;

    public GameObject piedestal;
}