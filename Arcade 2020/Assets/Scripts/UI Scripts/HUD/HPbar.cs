using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public PlayerHealthController Health;
   // public RectTransform HealthBar; //This is what you use if you want to change the size of the healthbar
    //Thats not what we're gonna do
    private Image HealthBar;
    private float currentFill;
    public float BarChangeSpeed;
    // Start is called before the first frame update

    private void Start()
    {
        HealthBar = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        currentFill = (float)Health.currentHealth / (float)Health.maxHealth;
        if (currentFill != HealthBar.fillAmount)
        {
            HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, currentFill, Time.deltaTime * BarChangeSpeed);
        }
    }

}
