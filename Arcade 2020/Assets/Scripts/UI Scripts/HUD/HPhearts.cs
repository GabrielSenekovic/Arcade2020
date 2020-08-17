using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPhearts : MonoBehaviour
{
    [SerializeField]Heart heartPrefab;
    public PlayerHealthController Health;
    List<Heart> hearts = new List<Heart>() { };
    [SerializeField] Sprite wholeHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;
    int currentHeartFill = 0;

    public void Start()
    {
        Debug.Log("Instantiating hearts");
        for(int i = 0; i < Health.maxHealth; i+=2)
        {
            hearts.Add(Instantiate(heartPrefab, transform));
        }
        currentHeartFill = Health.maxHealth;
    }

    private void Update()
    {
        if(currentHeartFill != Health.currentHealth)
        {
            for(int i = 0; i < hearts.Count; i++)
            {
                if(i+2*i <= Health.currentHealth)
                {
                    Debug.Log(i);
                    hearts[i].GetComponent<Image>().sprite = wholeHeart;
                }
                else
                {
                    Debug.Log(i);
                    hearts[i].GetComponent<Image>().sprite = emptyHeart;
                }
            }
            if (Health.currentHealth % 2 == 1)
            {
                //if current health is odd
                hearts[(int)((Health.currentHealth + 1) / 2) - 1].GetComponent<Image>().sprite = halfHeart;
            }
            currentHeartFill = Health.currentHealth;
        }
    }
}
